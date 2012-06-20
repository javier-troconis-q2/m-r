using System;
using System.Collections.Generic;
using SimpleCQRS.Commands;
using SimpleCQRS.Events;

namespace CQRSGui
{
    public interface IEventPublisher<in TEvent>
    {
        void Publish(TEvent @event, EventMetadata metadata);
    }


    public class FakeBus<TEvent> : IEventPublisher<TEvent>, ICommandSender
    {
        readonly Dictionary<Type, List<dynamic>> handlers = new Dictionary<Type, List<dynamic>>(); 
        readonly Dictionary<Type, List<dynamic>> metadataHandlers = new Dictionary<Type, List<dynamic>>(); 

        public void RegisterHandler<T>(Action<T> handler)
        {
            List<dynamic> list;
            if (!handlers.TryGetValue(typeof(T), out list))
            {
                list = new List<dynamic>();
                handlers.Add(typeof(T), list);
            }
            list.Add(handler);
        }

        public void RegisterHandler<T>(Action<T, EventMetadata> handler)
        {
            List<dynamic> list;
            if (!metadataHandlers.TryGetValue(typeof(T), out list))
            {
                list = new List<dynamic>();
                metadataHandlers.Add(typeof(T), list);
            }
            list.Add(handler);
        }

        public void Publish(TEvent @event, EventMetadata metadata)
        {
            List<dynamic> list;
            Type type = @event.GetType();
            if (handlers.TryGetValue(type, out list))
                foreach (var handler in list)
                    handler((dynamic)@event);
            if (metadataHandlers.TryGetValue(type, out list))
                foreach (var handler in list)
                    handler((dynamic)@event, metadata);
        }

        public void Send(object command)
        {
            List<dynamic> list;
            if (!handlers.TryGetValue(command.GetType(), out list) || list.Count > 1)
                throw new Exception("A command should be sent to only one recipient");
            dynamic handler = list[0];
            handler((dynamic)command);
        }
    }
}