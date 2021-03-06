﻿using System;
using FluentValidation;
using FluentValidation.Validators;

namespace Tripod.Domain.Security
{
    public static class MustNotBeRedeemedVerifyEmailTicketExtensions
    {
        /// <summary>
        /// Validates that the EmailVerification with this Ticket is not redeemed.
        /// </summary>
        /// <typeparam name="T">The command with the EmailVerification Ticket to validate.</typeparam>
        /// <param name="ruleBuilder">Fluent rule builder options.</param>
        /// <param name="queries">Query processor instance, for locating EmailVerification by Ticket.</param>
        /// <returns>Fluent rule builder options.</returns>
        public static IRuleBuilderOptions<T, string> MustNotBeRedeemedVerifyEmailTicket<T>
            (this IRuleBuilder<T, string> ruleBuilder, IProcessQueries queries)
        {
            return ruleBuilder.SetValidator(new MustNotBeRedeemedVerifyEmailTicket(queries));
        }
    }

    internal class MustNotBeRedeemedVerifyEmailTicket : PropertyValidator
    {
        private readonly IProcessQueries _queries;

        internal MustNotBeRedeemedVerifyEmailTicket(IProcessQueries queries)
            : base(() => Resources.Validation_EmailVerificationTicket_IsRedeemed)
        {
            if (queries == null) throw new ArgumentNullException("queries");
            _queries = queries;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var ticket = (string)context.PropertyValue;
            if (string.IsNullOrWhiteSpace(ticket)) return true;
            var entity = _queries.Execute(new EmailVerificationBy(ticket)).Result;
            if (entity == null) return true;
            if (!entity.RedeemedOnUtc.HasValue) return true;

            context.MessageFormatter.AppendArgument("PropertyName", context.PropertyDescription.ToLower());
            return false;
        }
    }
}
