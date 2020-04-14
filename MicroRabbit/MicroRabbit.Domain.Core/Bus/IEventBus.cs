using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Domain.Core.Bus
{
    public interface IEventBus
    {
        //Who ever implement this interface (MassTransit, NServiceBus, RabbitMQ, etc) need to implement this methods.
        //We going to be using our MediatR library, to send command to varios places throught the Bus.
        Task SendCommand<T>(T command) where T : Command;

        //We have services that publishing events
        void Publish<T>(T @event) where T : Event;

        //but we want some services to subscribes to those events,
        //not every service wants to subscribe to the same event (it can)
        //but we want services to subscribe to different published events.

        //Type T (Event Type) and TH (EventHandler)
        void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>;


    }
}
