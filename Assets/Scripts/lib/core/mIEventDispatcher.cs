using System;

namespace mm
{
    interface mmIEvent
    {

    }
    public interface mmIEventDispatcher
    {
        void on(string type, EventCtx callback);
        void off(string type, EventCtx callback);
        void emit(string type, Object param = null);
    }
}