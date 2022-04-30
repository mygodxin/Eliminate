using mm;
using FairyGUI;
using UnityEngine;
using Spine.Unity;
using System;
public class BuffWin : AppWindow
{
    public static string NAME = "BuffWin";

    GButton _videoBtn;
    GButton _closeBtn;
    public BuffWin() : base("BuffWin", "start")
    {
    }
    override public void BindChild()
    {
        this._videoBtn = this.getButton("videoBtn");
        this._closeBtn = this.getButton("noBtn");
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
        this._closeBtn.visible = false;
        Timers.inst.Add(2, 1, (object obj) =>
        {
            this._closeBtn.visible = true;
        });
    }

    void updateMoney()
    {
        // int money = GameDataManager.GetInt(DataDef.MONEY);
        // this._moneyTxt.text = money + "";
    }
    public override void OnClickButton(GButton button)
    {
        if (button == this._closeBtn)
        {
            // mm.AppScene.show(GameScene.NAME);
            this.Hide();
        }
        else if (button == this._videoBtn)
        {
            this.clickVide();
        }
    }
    void clickVide()
    {
        AdManager.inst.CreateVideoAd((isEnd) =>
        {
            DataManager.inst.addDoubleScoreTimes(GameDef.VideoDoubleScoreTimes);
            this.Hide();
            Util.AppTip("Your get score 10 times in the first 5 times");
        });
    }
}