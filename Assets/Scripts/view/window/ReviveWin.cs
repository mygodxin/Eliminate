using mm;
using FairyGUI;
using UnityEngine;
using Spine.Unity;
using System;
/// <summary>
/// 复活界面
/// </summary>
public class ReviveWin : AppScene
{
    public static string NAME = "ReviveWin";

    GButton _reviveBtn;
    GButton _closeBtn;
    public ReviveWin() : base("ReviveWin", "end")
    {
    }
    override public void BindChild()
    {
        this._reviveBtn = this.getButton("reviveBtn");
        this._closeBtn = this.getButton("closeBtn");
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
        if (button == this._reviveBtn)
        {
            int count = ConfigManager.inst.getGameConfig().reviveElimCount;
            GameCommond.inst.elimCount(count);
            this.Hide();
        }
        else if (button == this._closeBtn)
        {
            this.Hide();
            mm.AppWindow.show(GameEndWin.NAME);
        }
    }
}