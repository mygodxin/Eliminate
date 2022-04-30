using FairyGUI;

namespace mm
{
    public class AppScene : AppWindow
    {
        public static AppScene current
        {
            get
            {
                return AppScene._current;
            }
        }
        private static AppScene _current;
        public AppScene(string name, string pack) : base(name, pack)
        {

        }

        override public void initConfig()
        {
            this.modal = false;
            this.isCenter = false;
            this.isFullScreen = true;
            this.bringToFontOnClick = false;
        }

        override protected void _show()
        {

            if (AppScene._current != this)
            {
                // GRoot.inst.RemoveChildren();
                if (AppScene._current != null)
                    AppScene._current._hide();
                AppScene._current = this;
                base._show();
            }
            else
            {
                base._show();
            }
        }
    }
}