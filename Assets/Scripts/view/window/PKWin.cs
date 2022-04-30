using mm;
using FairyGUI;
using UnityEngine;
using Spine.Unity;
using System;
public class PKWin : AppScene
{
    public static string NAME = "PKWin";

    GLoader3D pkLoader3D;
    public PKWin() : base("PKWin", "pk")
    {
    }
    override public void BindChild()
    {
        this.pkLoader3D = this.GetLoader3D("pkLoader3D");
        this.pkLoader3D.loop = false;
        this.pkLoader3D.spineAnimation.state.Complete += this.onPlayComplete;
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
        this.pkLoader3D.animationName = "animation";
    }

    void onPlayComplete(Spine.TrackEntry trackEntry)
    {
        mm.AppScene.show(GameScene.NAME,GameMode.FIGHT);
        // Timers.inst.Add(3, 1, (object obj) =>
        // {
        //     mm.AppScene.show(GameScene.NAME);
        // });
    }

    void updateMoney()
    {
        // int money = GameDataManager.GetInt(DataDef.MONEY);
        // this._moneyTxt.text = money + "";
    }
    public override void OnClickButton(GButton button)
    {
        // if (button == this._startBtn)
        // {
        //     mm.AppScene.show(GameScene.NAME);
        //     // this.Hide();
        // }
    }
}