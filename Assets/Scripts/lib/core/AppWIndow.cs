using FairyGUI;
using System.Collections.Generic;
using System;
namespace mm
{
    public class AppWindow : Window
    {
        public static string configLoadingWaiting;
        private string _uiName;
        private string _uiPack;
        public string transShowName;

        public string transHideName;

        public bool isCenter;
        public bool isPopup;
        public Object openData;
        private AppWindow loadingWaitPane;
        private FairyChild fairyAdapter;
        public bool isFullScreen;
        private mmMediator meditor;
        private Dictionary<string, AppWindow> subWindowsList;
        public AppWindow ownerWindow;
        public string winName;
        public string winPack;
        public string[] eventList;

        public AppWindow(string name, string pack) : base()
        {
            if (name != null)
                this._uiName = name;
            if (pack != null)
                this._uiPack = pack;
            this.transShowName = "show";
            this.transHideName = "hide";
            this.modal = true;
            this.isCenter = true;
            this.subWindowsList = new Dictionary<string, AppWindow> { };
            // FairyGUI.Stage.inst.onStageResized
            this.initConfig();
        }

        public static AppWindow show(string type, Object param = null)
        {
            var win = mm.Global.getFairyInstence(type, new Object[1] { param });
            // if (param != undefined)
            //     win.showByParams(param);
            // else
            //     win.show();
            //fix:为避免showByParams与show交叉使用，改为统一
            if (win is AppWindow)
                return (win as AppWindow).showByParams(param);
            return null;
        }

        public AppWindow showByParams(Object param)
        {
            this.openData = param;
            this._show();
            return this;
        }

        protected virtual void _show()
        {
            this.registerMediator();
            // GRoot r = GRoot.inst.ShowPopup();
            if (this.isPopup)
                GRoot.inst.ShowPopup(this);
            else if (this.parent == GRoot.inst)
            {
                if (this._inited)
                {
                    this.RefreshUi();
                    // this.OnShowAniComplete();
                }
            }
            else
            {
                base.Show();
            }
        }

        private void registerMediator()
        {

        }

        public virtual string[] registerEvent()
        {
            return new string[] { };
        }

        // private void onStageEvent(string type)
        // {
        //     switch (type)
        //     {
        //         case k7.EVT_STAGE_ADDED:
        //             mvc.send(k7.EVT_FAIRY_SHOW, { view: this.contentPane, path: k7.getFairyPath(this) });
        //             break;
        //         case k7.EVT_STAGE_REMOVED:
        //             mvc.send(k7.EVT_FAIRY_HIDE, { view: this.contentPane, path: k7.getFairyPath(this) });
        //             break;
        //         case k7.EVT_STAGE_RESIZE:
        //             this.onResize();
        //             break;
        //     }
        // }
        public virtual void initConfig()
        {
        }
        protected virtual void _hide()
        {
            this.closeAllSubWindow();
            base.Hide();
            this.refreshOwnerWindow();
        }
        public void init()
        {
            base.Init();
            if (this._loading && AppWindow.configLoadingWaiting != null)
            {
                this.showLoadingWait();
            }
        }
        public void showLoadingWait()
        {
            if (this.loadingWaitPane == null)
                this.loadingWaitPane = UIPackage.CreateObjectFromURL(AppWindow.configLoadingWaiting) as AppWindow;
            this.layoutLoadingWaitPane();
            GRoot.inst.AddChild(this.loadingWaitPane);
        }
        public void layoutLoadingWaitPane()
        {
            this.loadingWaitPane.MakeFullScreen();
        }
        public void closeLoadingWait()
        {
            if (this.loadingWaitPane != null && this.loadingWaitPane.parent != null)
                this.loadingWaitPane.RemoveFromParent();
        }
        override protected void OnInit()
        {
            base.OnInit();
            this.closeLoadingWait();
            if (this.contentPane == null)
            {
                if (this._uiPack != null && UIPackage.GetByName(this._uiPack) == null)
                {
                    UIPackage.AddPackage(Global.fairyPath + this._uiPack);
                }
                this.contentPane = UIPackage.CreateObject(this._uiPack, this._uiName).asCom;
            }
            // this.topArea = this.contentPane.GetChild("top");
            // this.bottomArea = this.contentPane.GetChild("bottom");
            // this.centerArea = this.contentPane.GetChild("center");
            this.fairyAdapter = new FairyChild(this.contentPane, this);
            // this.mediatorAdapter = new MediatorUiAdapter(this._uiPack + '/' + this.name, this);
            this.meditor = new mmMediator(this._uiPack + '/' + this._uiName, this);
            this.meditor.eventList = this.registerEvent();
            // mvc.registerMediator(this.meditor);
            this.BindChild();
        }
        override protected void DoShowAnimation()
        {
            this.RegisterMediators();
            this.OnResize();
            this.RefreshUi();
            this.touchable = false;
            base.DoShowAnimation();
            // if (this.showAnimation)
            // {
            //     base.DoShowAnimation(this, this.onShowAniComplete);
            // }
            // else
            // {
            //     k7.Fairy.playTransition(this, this.transShowName, this, this.onShowAniComplete) || this.onShowAniComplete();
            // }
        }
        override protected void DoHideAnimation()
        {
            this.RemoveMediators();
            this.touchable = false;
            base.DoHideAnimation();
            // if (this.hideAnimation)
            // {
            //     this.hideAnimation(this, this.onHideAniComplete);
            // }
            // else
            // {
            //     k7.Fairy.playTransition(this, this.transHideName, this, this.onHideAniComplete) || this.onHideAniComplete();
            // }
        }
        override protected void OnShown()
        {
            this.touchable = true;
            // mvc.send(k7.EVT_UI_ONREADY, this);
            base.OnShown();
        }
        private void OnResize()
        {
            if (this.isFullScreen)
                this.MakeFullScreen();
            else if (this.isCenter)
                this.Center();
        }
        virtual public void BindChild() { }
        virtual public void RefreshUi() { }
        virtual public void OnClickButton(GButton button) { }
        public void OnCloseWindow(AppWindow window) { }
        public void onSubWindowClose(AppWindow win) { }
        /** 注册一个子窗口，随后可以用字符串打开该窗口，并绑定了子窗口该子窗口，详见bindSubWindow */
        public void RegisterSubWindow(AppWindow WinClass, string name, string pack = null)
        {
            if (this.subWindowsList[name] == null)
            {
                if (pack == null)
                    pack = this._uiPack;
                this.bindSubWindow(new AppWindow(name, pack));
            }
        }
        /** 绑定一个窗口实例为当前窗口的子窗口，启动关闭将会有冒泡联动通知（比如：用于刷新） */
        public void bindSubWindow(AppWindow win)
        {
            if (this.subWindowsList[win.name] == null)
            {
                win.ownerWindow = this;
                this.subWindowsList[win.name] = win;
            }
        }
        /** 打开一个子窗口 */
        public AppWindow showSubWindow(string name, Object openData)
        {
            var win = this.subWindowsList[name];
            if (win == null)
                return win;
            win.openData = openData;
            win._show();
            return win;
        }
        /** 关闭所有子窗口 */
        public void closeAllSubWindow()
        {
            foreach (var name in this.subWindowsList)
            {
                name.Value.Hide();
            }
        }
        /** 刷新父窗口界面(使用场景举例：子界面某操作更新大厅数据) */
        public void refreshOwnerWindow()
        {
            // this.ownerWindow && this.ownerWindow.onSubWindowClose(this);
        }
        public void RegisterMediators()
        {
            mvc.registerMediator(this.meditor);

        }
        public void RemoveMediators()
        {
            mvc.removeMediator(this.meditor.mediatorName);
        }
        // public void setRoot(view) { this.fairyAdapter.setRoot(view); }
        public GComponent GetComp(string path) { return this.fairyAdapter.GetComp(path); }
        public GLabel GetLabel(string path) { return this.fairyAdapter.GetLabel(path); }
        public GProgressBar GetProgressBar(string path) { return this.fairyAdapter.GetProgressBar(path); }
        public GTextField GetTextField(string path) { return this.fairyAdapter.GetTextField(path); }
        public GRichTextField GetRichTextField(string path) { return this.fairyAdapter.GetRichTextField(path); }
        public GTextInput GetTextInput(string path) { return this.fairyAdapter.GetTextInput(path); }
        public GLoader GetLoader(string path) { return this.fairyAdapter.GetLoader(path); }
        public GLoader3D GetLoader3D(string path) { return this.fairyAdapter.GetLoader3D(path); }
        public GList getList(string path) { return this.fairyAdapter.GetList(path); }
        public GGraph getGraph(string path) { return this.fairyAdapter.GetGraph(path); }
        public GGroup getGroup(string path) { return this.fairyAdapter.GetGroup(path); }
        public GSlider getSlider(string path) { return this.fairyAdapter.GetSlider(path); }
        public GComboBox getComboBox(string path) { return this.fairyAdapter.GetComboBox(path); }
        public GImage getImage(string path) { return this.fairyAdapter.GetImage(path); }
        public GMovieClip getMovieClip(string path) { return this.fairyAdapter.GetMovieClip(path); }
        public Controller getController(string path) { return this.fairyAdapter.GetController(path); }
        public Transition GetTransitionA(string path) { return this.fairyAdapter.GetTransition(path); }
        public GButton getButton(string path)
        {
            return this.fairyAdapter.GetButton(path);
        }
        // public void getWindow(path, closeListener, parent)
        // {
        //     return this.fairyAdapter.GetWindow(path, closeListener, parent);
        // }
        public string mediatorName
        {
            get
            {
                return this.meditor.mediatorName;
            }
        }
        public Window viewComponent
        {
            get
            {
                return this;
            }
        }
        public void onRegister() { }
        public virtual void onEvent(string eventName, Object param) { }
        public void onRemove() { }
    }
}