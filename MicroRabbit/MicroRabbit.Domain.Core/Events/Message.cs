using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Domain.Core.Events
{
    //Is going to implement IRequest. We going to need MediatR
    //Any request using MediatR is expecting a bool back (request command was sent, or message was process and so on).
    public abstract class Message : IRequest<bool>
    {
        public string MessageType { get; protected set; }
        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
