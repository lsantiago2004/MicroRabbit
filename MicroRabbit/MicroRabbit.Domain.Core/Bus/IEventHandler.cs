using MicroRabbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Domain.Core.Bus
{
    //It can handle any generic event, so it takes any type of event.
    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : Event
    {
        //lets handle any event that comes in.
        Task Handle(TEvent @event);
    }
    //Empty interface
    public interface IEventHandler
    {

    }
}
