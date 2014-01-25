﻿using System.Threading.Tasks;
using FluentValidation;

namespace Tripod.Domain.Security
{
    public class VerifyConfirmEmailSecret : IDefineCommand
    {
        public string Secret
        {
            get { return _secret; }
            [UsedImplicitly] set { _secret = value != null ? value.Trim() : null; }
        }
        private string _secret;
        public string Ticket { get; [UsedImplicitly] set; }
        public EmailConfirmationPurpose Purpose { get; [UsedImplicitly] set; }
        public string Token { get; internal set; }
    }

    [UsedImplicitly]
    public class ValidateVerifyConfirmEmailSecretCommand : AbstractValidator<VerifyConfirmEmailSecret>
    {
        public ValidateVerifyConfirmEmailSecretCommand(IProcessQueries queries)
        {
            RuleFor(x => x.Ticket)
                .NotEmpty()
                .MustFindEmailConfirmationByTicket(queries)
                .MustNotBeRedeemedConfirmEmailTicket(queries)
                .MustNotBeExpiredConfirmEmailTicket(queries)
                .MustBePurposedConfirmEmailTicket(x => x.Purpose, queries)
                    .WithName(EmailConfirmation.Constraints.Label);

            RuleFor(x => x.Secret)
                .NotEmpty()
                .MustBeVerifiedConfirmEmailSecret(x => x.Ticket, queries)
                    .WithName(EmailConfirmation.Constraints.SecretLabel);
        }
    }

    [UsedImplicitly]
    public class HandleVerifyConfirmEmailSecretCommand : IHandleCommand<VerifyConfirmEmailSecret>
    {
        private readonly IReadEntities _entities;

        public HandleVerifyConfirmEmailSecretCommand(IWriteEntities entities)
        {
            _entities = entities;
        }

        public async Task Handle(VerifyConfirmEmailSecret command)
        {
            var entity = await _entities.Query<EmailConfirmation>().ByTicketAsync(command.Ticket);
            command.Token = entity.Token;
        }
    }
}
