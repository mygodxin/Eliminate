using mm;
using FairyGUI;
using UnityEngine;
using Spine.Unity;
using System;
public class SetWin : AppWindow
{
    public static string NAME = "SetWin";

    GButton _effectBtn;
    GButton _musicBtn;
    GButton _ruleBtn;
    GButton _suoBtn;
    GButton _exitBtn;
    GButton _closeBtn;
    public SetWin() : base("SetWin", "set")
    {
    }
    override public void BindChild()
    {
        this._effectBtn = this.getButton("effectBtn");
        this._musicBtn = this.getButton("musicBtn");
        this._ruleBtn = this.getButton("ruleBtn");
        this._suoBtn = this.getButton("suoBtn");
        this._exitBtn = this.getButton("exitBtn");
        this._closeBtn = this.getButton("closeButton");
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
        // this.updateMoney();
        // this.onPlayComplete(null);
        // this.pkLoader3D.animationName = "animation";
    }
    public override void OnClickButton(GButton button)
    {
        if (button == this._effectBtn)
        {
            // mm.AppScene.show(GameScene.NAME);
            // this.Hide();
        }
        else if (button == this._musicBtn)
        {

        }
        else if (button == this._exitBtn)
        {

        }
        else if (button == this._ruleBtn)
        {

        }
        else if (button == this._suoBtn)
        {

        }
        else if (button == this._closeBtn)
        {
            this.Hide();
        }
    }
}