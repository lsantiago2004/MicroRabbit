using MicroRabbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Domain.Core.Commands
{
    //Command is going to be of Type Message
    //We going to have messages that are sent across when we hit send command
    //In other words, a message across our Bus and then the message ends up maybe in a 
    //different microservice, or a different project or maybe a window service and so on.
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; protected set; }

        protected Command()
        {
            Timestamp = DateTime.Now;

        }
    }
}
