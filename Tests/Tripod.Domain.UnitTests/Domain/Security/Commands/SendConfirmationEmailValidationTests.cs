﻿using System;
using System.Globalization;
using System.Linq;
using FluentValidation.Results;
using Moq;
using Should;
using Xunit;
using Xunit.Extensions;

namespace Tripod.Domain.Security
{
    public class SendConfirmationEmailValidationTests : FluentValidationTests
    {
        [Theory, InlineData(null), InlineData(""), InlineData("\t  \r\n")]
        public void IsInvalid_EmailAddress_IsNullOrWhitespace(string emailAddress)
        {
            var queries = new Mock<IProcessQueries>(MockBehavior.Strict);
            var validator = new ValidateSendConfirmationEmailCommand(queries.Object);
            var command = new SendConfirmationEmail { EmailAddress = emailAddress };

            var result = validator.Validate(command);

            result.IsValid.ShouldBeFalse();
            Func<ValidationFailure, bool> targetError = x => x.PropertyName == command.PropertyName(y => y.EmailAddress);
            result.Errors.Count(targetError).ShouldEqual(1);
            result.Errors.Single(targetError).ErrorMessage
                .ShouldEqual(Resources.notempty_error.Replace("{PropertyName}", EmailAddress.Constraints.Label));
        }

        [Theory, InlineData("invalid"), InlineData("invalid@"), InlineData("invalid@gmail"), InlineData("invalid@gmail."), InlineData("invalid@.com")]
        public void IsInvalid_EmailAddress_DoesNotMatchPattern(string emailAddress)
        {
            var queries = new Mock<IProcessQueries>(MockBehavior.Strict);
            var validator = new ValidateSendConfirmationEmailCommand(queries.Object);
            var command = new SendConfirmationEmail
            {
                EmailAddress = emailAddress,
            };

            var result = validator.Validate(command);

            result.IsValid.ShouldBeFalse();
            Func<ValidationFailure, bool> targetError = x => x.PropertyName == command.PropertyName(y => y.EmailAddress);
            result.Errors.Count(targetError).ShouldEqual(1);
            result.Errors.Single(targetError).ErrorMessage.ShouldEqual(Resources.email_error
                .Replace("{PropertyName}", EmailAddress.Constraints.Label)
                .Replace("{PropertyValue}", command.EmailAddress)
            );
        }

        [Fact]
        public void IsInvalid_WhenEmailAddressLength_IsGreaterThan_MaxLength()
        {
            var emailAddress = "a0@gmail.com";
            while (emailAddress.Length <= EmailAddress.Constraints.ValueMaxLength)
                emailAddress = emailAddress.Replace("0", "a0");
            var queries = new Mock<IProcessQueries>(MockBehavior.Strict);
            var validator = new ValidateSendConfirmationEmailCommand(queries.Object);
            var command = new SendConfirmationEmail { EmailAddress = emailAddress };

            var result = validator.Validate(command);

            result.IsValid.ShouldBeFalse();
            Func<ValidationFailure, bool> targetError = x => x.PropertyName == command.PropertyName(y => y.EmailAddress);
            result.Errors.Count(targetError).ShouldEqual(1);
            result.Errors.Single(targetError).ErrorMessage.ShouldEqual(Resources.Validation_MaxLength
                .Replace("{PropertyName}", EmailAddress.Constraints.Label)
                .Replace("{MaxLength}", EmailAddress.Constraints.ValueMaxLength.ToString(CultureInfo.InvariantCulture))
                .Replace("{TotalLength}", command.EmailAddress.Length.ToString(CultureInfo.InvariantCulture))
            );
        }

        [Fact]
        public void IsInvalid_WhenIsExpectingEmail_IsFalse()
        {
            var queries = new Mock<IProcessQueries>(MockBehavior.Strict);
            var validator = new ValidateSendConfirmationEmailCommand(queries.Object);
            var command = new SendConfirmationEmail();

            var result = validator.Validate(command);

            result.IsValid.ShouldBeFalse();
            Func<ValidationFailure, bool> targetError = x => x.PropertyName == command.PropertyName(y => y.IsExpectingEmail);
            result.Errors.Count(targetError).ShouldEqual(1);
            result.Errors.Single(targetError).ErrorMessage.ShouldEqual(Resources.Validation_SendConfirmationEmail_IsExpectingEmail
                .Replace("{PropertyName}", EmailAddress.Constraints.Label.ToLower())
            );
        }

        [Fact]
        public void IsValid_WhenAllRulesPass()
        {
            var queries = new Mock<IProcessQueries>(MockBehavior.Strict);
            var validator = new ValidateSendConfirmationEmailCommand(queries.Object);
            var command = new SendConfirmationEmail
            {
                EmailAddress = "valid@gmail.com",
                IsExpectingEmail = true,
            };

            var result = validator.Validate(command);

            result.IsValid.ShouldBeTrue();
        }
    }
}
