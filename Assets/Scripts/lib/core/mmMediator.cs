namespace mm
{
    public class mmMediator : mmIMediator
    {
        public string mediatorName { get; protected set; }
        public AppWindow viewComponent { get; set; }
        public string[] eventList { get; set; }
        public mmMediator(string name, AppWindow view)
        {
            this.mediatorName = name;
            this.viewComponent = view;
            this.eventList = new string[0];
        }
        public void onRegister()
        {

        }
        public void onEvent(string eventName, object param)
        {
            this.viewComponent.onEvent(eventName, param);
        }
        public virtual void onRemove()
        {

        }
    }
}