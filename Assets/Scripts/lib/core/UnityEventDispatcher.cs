using System.Collections.Generic;
using FairyGUI;
using System;

namespace mm
{
    public class UnityEventDispatcher : mmIEventDispatcher
    {
        private Dictionary<string, EventCallback1> eventMap;
        private EventDispatcher eventDispatcher;
        public UnityEventDispatcher()
        {
            eventMap = new Dictionary<string, EventCallback1>();
            eventDispatcher = new EventDispatcher();
        }

        public void on(string type, EventCtx callback)
        {
            //             _clickHandler = (EventContext context) =>
            // {
            //     _owner.DispatchEvent(CLICK_EVENT, context.data, this);
            // };
            if (eventMap.ContainsKey(type))
            {
                // EventCallback1 eventCallBack;
                // eventMap.TryGetValue(type, out eventCallBack);
                eventMap.Remove(type);
            }
            EventCallback1 e = (EventContext context) =>
            {
                callback.Invoke(context.data);
            };
            eventMap.Add(type, e);
            eventDispatcher.AddEventListener(type, e);
        }
        public void off(string type, EventCtx callback)
        {
            EventCallback1 eventCallBack;
            eventMap.TryGetValue(type, out eventCallBack);
            eventDispatcher.RemoveEventListener(type, eventCallBack);
        }
        public void emit(string type, Object param = null)
        {
            eventDispatcher.DispatchEvent(type, param);
        }
    }
}