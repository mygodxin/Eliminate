using FairyGUI;
using System;
namespace mm
{
    public enum EAlertType
    {
        /** 显示两个按钮 */
        DOUBLE = 0,
        /** 只显示左边的 */
        LEFT = 1,
        /** 只显示右边的 */
        RIGHT = 2,
        /** 左右按钮颜色交换[TODO] */
        SWAP = 3,
        /** 什么按钮都没有 */
        NONE = 4
    }

    public class AlertWindow : AppWindow
    {
        public static AlertWindow inst;
        Controller stateCtrl;
        Controller topCtrl;
        GButton leftButton;
        GButton rightButton;
        GTextField contentTextFiled;
        EAlertType alertType;
        string contentString;
        Action _leftCallBack;
        Action _rightCallBack;
        Action closeCallBack;
        public AlertWindow(string name, string pack) : base(name, pack)
        {
        }
        override public void BindChild()
        {
            this.stateCtrl = this.getController("state");
            this.topCtrl = this.getController("frame.top");
            this.leftButton = this.getButton("leftButton");
            this.rightButton = this.getButton("rightButton");
            this.contentTextFiled = this.GetTextField("contentTextFiled");
        }
        public void SetAndShow(string content, EAlertType alertType = EAlertType.DOUBLE, Action leftCallBack = null, Action rightCallBack = null)
        {
            this._leftCallBack = leftCallBack;
            this._rightCallBack = rightCallBack;
            this.alertType = alertType;
            this.contentString = content;
            base._show();
        }
        public override void RefreshUi()
        {
            // if (this.topCtrl != null)
            //     this.topCtrl.selectedIndex = this.param.noClose ? 1 : 0;
            // if (this.param.type !== 1 && this.param.type !== 2 && this.param.type !== 3)
            //     this.param.type = 0;
            this.stateCtrl.selectedIndex = (int)this.alertType;
            this.contentTextFiled.text = this.contentString;
            // if (this.param.title)
            //     this.frame.icon = this.param.title;
            // if (this.param.textL)
            //     this.leftButton.title = this.param.textL;
            // if (this.param.textR)
            //     this.rightButton.title = this.param.textR;
        }
        public override void OnClickButton(GButton button)
        {
            // switch (button)
            // {
            //     case this.leftButton:
            //         this.param.type == EAlertType.SWAP ? this.onClickRight() : this.onClickLeft();
            //         break;
            //     case this.rightButton:
            //         this.param.type == EAlertType.SWAP ? this.onClickLeft() : this.onClickRight();
            //         break;
            // }
            if (button == this.leftButton)
            {
                this.onClickLeft();
                this._hide();
            }
            else if (button == this.rightButton)
            {
                this.onClickRight();
                this._hide();
            }
        }
        void onClickLeft()
        {
            // if (!this.param.stayL)
            //     this.hide();
            // if (typeof this.param.subL == "function") {
            //     this.param.subL.call(this.param.objL || this.param.thisObj || this);
            // }
            this._leftCallBack?.Invoke();
        }
        void onClickRight()
        {
            // if (!this.param.stayL)
            //     this.hide();
            // if (typeof this.param.subR == "function") {
            //     this.param.subR.call(this.param.objR || this.param.thisObj || this);
            // }
            this._rightCallBack?.Invoke();
        }

        override protected void _hide()
        {
            base._hide();
        }
    }
}