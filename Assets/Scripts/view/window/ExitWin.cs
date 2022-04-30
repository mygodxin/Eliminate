using mm;
using FairyGUI;
using UnityEngine;
using Spine.Unity;
using System;
public class ExitWin : AppScene
{
    public static string NAME = "ExitWin";

    GButton _rightBtn;
    GButton _leftBtn;
    public ExitWin() : base("Alert", "common")
    {
    }
    override public void BindChild()
    {
        this._rightBtn = this.getButton("rightButton");
        this._leftBtn = this.getButton("leftButton");
    }
    override public string[] registerEvent()
    {
        return new string[]{

        };
    }
    override public void onEvent(string eventName, object param)
    {

    }
    public override void RefreshUi()
    {
    }
    public override void OnClickButton(GButton button)
    {
        if (button == this._leftBtn)
        {
            mm.AppScene.show(StartScene.NAME);
            // this.Hide();
        }else if(button == this._rightBtn){
            this.Hide();
        }
    }
}