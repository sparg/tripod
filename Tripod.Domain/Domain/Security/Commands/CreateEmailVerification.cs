﻿using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNet.Identity;

namespace Tripod.Domain.Security
{
    public class CreateEmailVerification : BaseCreateEntityCommand<EmailVerification>, IDefineCommand
    {
        public string EmailAddress { get; [UsedImplicitly] set; }
        public EmailVerificationPurpose Purpose { get; [UsedImplicitly] set; }
    }

    public class ValidateCreateEmailVerificationCommand : AbstractValidator<CreateEmailVerification>
    {
        public ValidateCreateEmailVerificationCommand(IProcessQueries queries)
        {
            RuleFor(x => x.EmailAddress)
                .MustBeVerifiableEmailAddress(queries)
                    .WithName(EmailAddress.Constraints.Label);

            RuleFor(x => x.Purpose)
                .MustBeValidVerifyEmailPurpose()
                    .WithName(EmailVerification.Constraints.Label);
        }
    }

    [UsedImplicitly]
    public class HandleCreateEmailVerificationCommand : IHandleCommand<CreateEmailVerification>
    {
        private readonly UserManager<User, int> _userManager;
        private readonly IProcessQueries _queries;
        private readonly IWriteEntities _entities;

        public HandleCreateEmailVerificationCommand(UserManager<User, int> userManager, IProcessQueries queries, IWriteEntities entities)
        {
            _userManager = userManager;
            _queries = queries;
            _entities = entities;
        }

        public async Task Handle(CreateEmailVerification command)
        {
            // find or create the email address
            var emailAddress = await _entities.Get<EmailAddress>().ByValueAsync(command.EmailAddress)
                ?? new EmailAddress
                {
                    Value = command.EmailAddress,
                    HashedValue = await _queries.Execute(new HashedEmailValueBy(command.EmailAddress)),
                };

            // create random secret and ticket
            // note that changing the length of the secret requires updating the
            // email messages sent to the address, since those mention secret length
            var secret = _queries.Execute(new RandomSecret(10, 12));
            var ticket = _queries.Execute(new RandomSecret(20, 25));

            // make sure ticket is unique
            while (_entities.Query<EmailVerification>().ByTicket(ticket) != null)
                ticket = _queries.Execute(new RandomSecret(20, 25));

            // serialize a new user token to a string
            var token = _userManager.UserConfirmationTokens.Generate(new UserToken
            {
                UserId = command.EmailAddress,
                Value = ticket,
                CreationDate = DateTime.UtcNow,
            });

            // create the verification
            var verification = new EmailVerification
            {
                Owner = emailAddress,
                Purpose = command.Purpose,
                Secret = secret,
                Ticket = ticket,
                Token = token,

                // change this, and you have to change the content of the email messages to reflect new expiration
                ExpiresOnUtc = DateTime.UtcNow.AddMinutes(30),
            };
            _entities.Create(verification);

            if (command.Commit) await _entities.SaveChangesAsync();

            command.CreatedEntity = verification;
        }
    }
}