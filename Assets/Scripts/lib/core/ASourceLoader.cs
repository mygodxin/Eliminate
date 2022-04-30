using FairyGUI;

namespace mm
{
    public abstract class ASourceLoader : IUISource
    {
        public static string EVT_SourceLoader_CompleteEvent = "EVT_SourceLoader_CompleteEvent";
        public string fileName { get; set; }
        public bool loaded
        {
            get
            {
                return this._loaded;
            }
        }
        public bool loading
        {
            get
            {
                return _loading;
            }
        }
        public int retry;
        private bool _loaded;
        private bool _loading;
        /**
         * 若正在加载过程中，重复调用，将只会注册不同的回调函数，但不会重复换起加载。
         * 加载成功后，将自动清除所有回调。
         *  若想维护监听状态，则不要传入回调函数，使用事件机制来处理回调。
         */
        public void Load(UILoadCallback callback = null)
        {
            this.retry++;
            this._loading = true;
            this._loaded = false;
            this.Start();
        }
        /**
         * 由具体业务抽象实现加载过程
         */
        protected abstract void Start();
        public void Complete()
        {
            this._loaded = true;
            this._loading = false;
        }
        protected void Success()
        {
            this.Complete();
        }
        protected abstract void Failed();
    }
}