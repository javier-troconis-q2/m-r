using System;
using System.Collections.Generic;
using SimpleCQRS.Commands;
using SimpleCQRS.Events;

namespace CQRSGui
{
    public class FakeBus : IEventPublisher, ICommandSender
    {
        readonly Dictionary<Type, List<dynamic>> handlers = new Dictionary<Type, List<dynamic>>(); 

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

        public void Publish(object @event)
        {
            List<dynamic> list;
            if (handlers.TryGetValue(@event.GetType(), out list))
                foreach (var handler in list)
                    handler((dynamic)@event);
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