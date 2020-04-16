using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Domain.CommandHandlers
{
    public class TransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
    {
        private readonly IEventBus _bus;

        public TransferCommandHandler(IEventBus bus)
        {
            _bus = bus;
        }
        public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            //publish event to RabbitMQ
            //The idea is this, (our banking web MVC app maybe let's say). The customer will login and they will
            //have a chance to view all their account information through our baking micro service as well as initiate a transfer.
            //Now that transfer is a command that sent through the bus from our service and then is 
            //handled in our commandhandler and then our command handler transfercommandhandler
            //will actually finally publish an event to RabbitMQ.
            _bus.Publish(new TransferCreatedEvent(request.From, request.To, request.Amount));

            return Task.FromResult(true);
        }
    }
}
