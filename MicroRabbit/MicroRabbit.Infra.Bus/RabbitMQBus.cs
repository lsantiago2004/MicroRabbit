using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        //going to use a dictionary, to hold our Handlers for all events
        //Will be of Type string and a List of the type of Handler it is.
        private readonly Dictionary<string, List<Type>> _handlers;
        //List of EventTypes
        private readonly List<Type> _eventTypes;

        public RabbitMQBus(IMediator mediator)
        {
            _mediator = mediator;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }

        //Related to our Bus sending Commands across and then Publish and Subscribe related to the Events.
        //It will use the MediatR to send the command
        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        //This will be use for different microservices to publish events to the rabbit MQ server.
        //Similar to the publish and consume messages in RabbitMQ. We going to use the same code
        //to simply publish to our Queue.
        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //Get the event name. Whoever is using this publish method
                //sent a generic event, so we can grab the name of that event by knowing the Type (using reflection).
                var eventName = @event.GetType().Name;

                //Declare a queue in RabbitMQ server. Create a queue with same name of the event.
                //SO we can have various events and they're assigned a Queue.
                //We default durability to 'false', exclusive, Autodelete, etc
                channel.QueueDeclare(eventName, false, false, false, null);
                //The message is pretty much our event. Lets serialize it.
                var message = JsonConvert.SerializeObject(@event);
                //Encode the message into a body
                var body = Encoding.UTF8.GetBytes(message);
                //Use channel to publish the message (basic publish)
                //We dont have an address, but we give it the eventName for the Queue name
                channel.BasicPublish("", eventName, null, body);
            }

        }
        //Takes an event and event handler. Every event is a type of event, so we need to handle that 
        //type of event. The idea is to make use of our local variable (our dictionary) to store all of our handlers
        //and the event types and that way we have unique handlers there whenever someone subscribes to an event using the required handler.
        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            //using generics and reflection
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);
            //if doesnt already contain that type of event, go ahead and add that event type.
            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            //lets take care of our handlers, thats a Dictionary. Lets check the dictionary keys if they already exist with 
            //that event name.
            if(!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }
            //basic validation. So in case the handlers with that event name, like course created or money deposit event
            //the handler for that is 'Money Deposit event handler' is already there with same name, throw exception
            if(_handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
            }

            //Assign the handler and add it to the list of types using our dictionary
            _handlers[eventName].Add(handlerType);

            //Once we add our handlers, we can start our consumption of these messages.
            //So when someone subscribe to our events, we kick off a kind
            //of a consume handler.
            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory() {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            //create a delegate (pointer to a method).
            //Is basically listening for any message in our Queue
            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, true, consumer);


        }
        //A message has comming to our queue (someone publish a message, is sitting on our queue, someone subscribe to that queue or event
        //so we need a way to pick up that message and convert it to our actual object and then send it through our Bus
        //to whoever is handling that type of event)
        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                //get al the subscribers for that event
                var subscriptions = _handlers[eventName];
                //loop through each subscription in subscriptions
                foreach(var subscription in subscriptions)
                {
                    //create handler using dynamic approach. Create instance of that type
                    var handler = Activator.CreateInstance(subscription);
                    //if null continue looping until found one.
                    if (handler == null) continue;
                    //Now we can look to all of our events memeber , is our local dictionary
                    //Get the first where the name of that event is equal to the incoming eventname.
                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                    //Now we have the handler and the eventtype (ex. CourseCreated). We can take the message and desirialize it
                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    //
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    //Lets now invoke the main method that is the handler to do the work on this type of event.
                    //Routing to the right handler in all our microservice messages
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                }
            }
        }
    }
}
