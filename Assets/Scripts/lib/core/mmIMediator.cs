namespace mm
{
    public interface mmIMediator
    {

        string mediatorName { get; }
        AppWindow viewComponent { get; set; }
        string[] eventList { get; set; }
        void onRegister();
        void onEvent(string eventName, object param = null);
        void onRemove();
    }
}