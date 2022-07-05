using MediatR;
using Shared.Infra.Cqrs;
using MKT.Integration.Application.Commands.Plataform;
using System.Threading;
using System.Threading.Tasks;

namespace MKT.Integration.Application.Events
{
    public class UpdateSupplierRequestEvent : BaseNotification
    {
        public string IdIntegrationServiceDesk { get; set; }
        public long IdSupplierRequest { get; set; }
        public UpdateSupplierRequestEvent(string IdIntegrationServiceDesk, long IdSupplierRequest)
        {
            this.IdIntegrationServiceDesk = IdIntegrationServiceDesk;
            this.IdSupplierRequest = IdSupplierRequest;
        }
    }

    public class UpdateSupplierRequestEventHandle : BaseEventHandler<UpdateSupplierRequestEvent>
    {
        public IMediator Mediator { get; set; }

        public UpdateSupplierRequestEventHandle(IMediator mediator)
        {
            Mediator = mediator;
        }

        public override async Task Handle(UpdateSupplierRequestEvent notification, CancellationToken cancellationToken)
        {
            await Mediator.Send(new UpdateSupplierRequestCommand.UpdateSupplierRequestContract { IdSupplierRequest = notification.IdSupplierRequest, IdIntegrationServiceDesk = notification.IdIntegrationServiceDesk });
        }
    }
}
