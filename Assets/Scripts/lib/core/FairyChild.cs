using FairyGUI;
using System;

namespace mm
{
    public class FairyChild
    {
        public GComponent viewComponent;
        public AppWindow appWindow;
        public FairyChild(GComponent viewComponent, AppWindow appWindow = null)
        {
            this.appWindow = appWindow;
            this.viewComponent = viewComponent;
        }
        private Object GetObj(string path, GComponent view = null, string type = null)
        {
            if (type == null) { type = "component"; }
            string[] pathStr = path.Split('.');
            var len = pathStr.Length;
            if (view == null)
                view = this.viewComponent;
            var i = 0;
            for (; i < len - 1; ++i)
            {
                view = view.GetChild(pathStr[i]).asCom;
                if (view == null)
                    return null;
            }
            switch (type)
            {
                case "controller": return view != null ? view.GetController(pathStr[i]) : null;
                case "transition": return view != null ? view.GetController(pathStr[i]) : null;
            }
            return view != null ? view.GetChild(pathStr[i]) : null;
        }
        public GComponent GetComp(string path)
        {
            var obj = this.GetObj(path) as GObject;
            return obj == null ? null : obj.asCom;
        }
        public GButton GetButton(string path)
        {
            var obj = this.GetObj(path) as GButton;
            if (obj != null)
            {
                obj.onClick.Add(() =>
                {
                    this.OnClickButton(obj);
                });
            }
            return obj;
        }
        public void OnClickButton(GButton button)
        {
            if (this.appWindow != null)
            {
                this.appWindow.OnClickButton(button);
            }
        }
        public GLabel GetLabel(string path)
        {
            var obj = this.GetObj(path) as GLabel;
            return obj == null ? null : obj.asLabel;
        }
        public GProgressBar GetProgressBar(string path)
        {
            var obj = this.GetObj(path) as GProgressBar;
            return obj;
        }
        public GTextField GetTextField(string path)
        {
            var obj = this.GetObj(path) as GTextField;
            return obj;
        }
        public GRichTextField GetRichTextField(string path)
        {
            var obj = this.GetObj(path) as GRichTextField;
            return obj;
        }
        public GTextInput GetTextInput(string path)
        {
            var obj = this.GetObj(path) as GTextInput;
            return obj;
        }
        public GLoader GetLoader(string path)
        {
            var obj = this.GetObj(path) as GLoader;
            return obj;
        }
        public GLoader3D GetLoader3D(string path)
        {
            var obj = this.GetObj(path) as GLoader3D;
            return obj;
        }
        public GList GetList(string path)
        {
            var obj = this.GetObj(path) as GList;
            return obj;
        }
        public GGraph GetGraph(string path)
        {
            var obj = this.GetObj(path) as GGraph;
            return obj;
        }
        public GGroup GetGroup(string path)
        {
            var obj = this.GetObj(path) as GGroup;
            return obj;
        }
        public GSlider GetSlider(string path)
        {
            var obj = this.GetObj(path) as GSlider;
            return obj;
        }
        public GComboBox GetComboBox(string path)
        {
            var obj = this.GetObj(path) as GComboBox;
            return obj;
        }
        public GImage GetImage(string path)
        {
            var obj = this.GetObj(path) as GImage;
            return obj == null ? null : obj.asImage;
        }
        public GMovieClip GetMovieClip(string path)
        {
            var obj = this.GetObj(path) as GMovieClip;
            return obj;
        }
        public Controller GetController(string path)
        {
            return this.GetObj(path, null, "controller") as Controller;
        }
        public Transition GetTransition(string path)
        {
            return this.GetObj(path, null, "transition") as Transition;
        }
    }
}