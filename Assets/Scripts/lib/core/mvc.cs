using System.Collections.Generic;
using System;

namespace mm
{
    public class mvc
    {
        private static mmIEventDispatcher eventDispatcher;
        private static Dictionary<string, bool> commandMap = new Dictionary<string, bool>();
        private static Dictionary<string, mmIMediator> mediatorMap = new Dictionary<string, mmIMediator>();
        private static Dictionary<string, EventCtx> inMediatorMap = new Dictionary<string, EventCtx>();
        public static void init(mmIEventDispatcher evtInst)
        {
            if (eventDispatcher == null)
            {
                eventDispatcher = evtInst;
            }
        }

        public static void send(string eventName, object param = null)
        {
            eventDispatcher.emit(eventName, param);
        }
        public static void on(string eventName, EventCtx callback)
        {
            eventDispatcher.on(eventName, callback);
        }
        public static void off(string eventName, EventCtx callback)
        {
            eventDispatcher.off(eventName, callback);
        }
        public static void once(string eventName, EventCtx callback)
        {
            EventCtx onceListener = delegate
            {
                // callback?.Invoke(eventName,);
            };
            on(eventName, onceListener);
        }
        public static void registerCommand(string eventName, ICommond commandClassRef)
        {
            mvc.commandMap.Add(eventName, true);
            // mvc.eventDispatcher.on(eventName, null, mvc.executeCommand(eventName, commandClassRef));
            // private static IEvent executeCommand(string eventName, Commond CommandClassRef = null, object param = null)
            // {
            //     // return CommandClassRef.execute(eventName, param);
            // }
        }
        public static void removeCommand(string eventName)
        {
            if (commandMap.ContainsKey(eventName))
            {
                commandMap.Add(eventName, false);
                // eventDispatcher.off(eventName, executeCommand(eventName));
            }
        }
        public static bool hasCommand(string eventName)
        {
            return commandMap.ContainsKey(eventName);
        }
        public static void registerMediator(mmIMediator mediator)
        {
            var name = mediator.mediatorName;
            if (mediatorMap.ContainsKey(name))
                return;
            mediatorMap[name] = mediator;
            var interests = mediator.eventList;
            var len = interests != null ? interests.Length : 0;
            for (var i = 0; i < len; i++)
            {
                var inter = interests[i];
                // if (typeof inter === 'string')
                // {
                EventCtx eventCtx = delegate (Object param)
                {
                    mediator.onEvent(inter, param);
                };
                eventDispatcher.on(inter, eventCtx); //mediator.onEvent(mediator.mediatorName));
                inMediatorMap[inter] = eventCtx;
                // }
                // else
                // {
                //     var _a = inter, name_1 = _a.name, handler = _a.handler;
                //     if (typeof handler == 'function')
                //     {
                //         inMediatorMap[name_1] = true;
                //         eventDispatcher.on(name_1, mediator, handler);
                //     }
                //     else
                //     {
                //         for (var j = 0; j < handler.length; j++)
                //         {
                //             inMediatorMap[name_1] = true;
                //             eventDispatcher.on(name_1, mediator, handler[j]);
                //         }
                //     }
                // }
            }
            mediator.onRegister();
        }

        private void ss()
        {

        }
        public static mmIMediator removeMediator(string mediatorName)
        {
            var mediator = mediatorMap[mediatorName];
            if (mediator == null)
                return null;
            var interests = mediator.eventList;
            var i = interests.Length;
            while (--i > -1)
            {
                var inter = interests[i];
                // if (typeof inter == 'string')
                // {
                EventCtx eventCtx;
                inMediatorMap.TryGetValue(inter, out eventCtx);
                eventDispatcher.off(inter, eventCtx);
                inMediatorMap[inter] = null;
                // }
                // else
                // {
                //     var _a = inter;
                //     var name_2 = _a.name;
                //     var handler = _a.handler;
                //     if (typeof handler == 'function')
                //     {
                //         inMediatorMap[name_2] = false;
                //         eventDispatcher.off(name_2, mediator, handler);
                //     }
                //     else
                //     {
                //         for (var j = 0; j < handler.length; j++)
                //         {
                //             inMediatorMap[name_2] = false;
                //             eventDispatcher.off(name_2, mediator, handler[j]);
                //         }
                //     }
                // }
            }
            mediatorMap.Remove(mediatorName);
            mediator.onRemove();
            return mediator;
        }
        public static mmIMediator retrieveMediator(string mediatorName)
        {
            return mediatorMap[mediatorName];
        }
        public static bool hasMediator(string mediatorName)
        {
            return mediatorMap[mediatorName] != null;
        }
        public static bool hasInMediator(string eventName)
        {
            EventCtx eventCtx;
            inMediatorMap.TryGetValue(eventName, out eventCtx);
            return eventCtx == null;
        }
    }
}