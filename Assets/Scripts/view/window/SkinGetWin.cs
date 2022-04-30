using mm;
using FairyGUI;
using UnityEngine;
using Spine.Unity;
using System;
public class SkinGetWin : AppWindow
{
    public static string NAME = "SkinGetWin";

    GButton _videoBtn;
    GButton _closeBtn;
    GLoader _skinLoader;
    GLoader3D _skinLoader3D;
    Controller _typeCtrl;
    int _type;
    int skinIndex;
    public SkinGetWin() : base("SkinGetWin", "skin")
    {
    }
    override public void BindChild()
    {
        this._videoBtn = this.getButton("videoBtn");
        this._closeBtn = this.getButton("noBtn");
        this._skinLoader = this.GetLoader("icon");
        this._skinLoader3D = this.GetLoader3D("icon3D");
        this._typeCtrl = this.getController("type");
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
        // this._closeBtn.visible = false;
        // Timers.inst.Add(2, 1, (object obj) =>
        // {
        //     this._closeBtn.visible = true;
        // });
        var data = this.openData as int[];
        
        this.skinIndex = data[1];
        string skinName = SkinManager.skinNameArr[skinIndex];
        // this._skinLoader.url = "ui://common/" + skinName + "_0";
        // GLoader3D gridSpine = gridComp.GetChild("icon3D").asLoader3D;
        // gridSpine.url = "XianChuQiu";
        int type = data[0];//SkinManager.inst.getResultItemType();
        this._typeCtrl.selectedIndex = type;
        this._skinLoader3D.url = type == ItemType.SKIN ? "ui://common/HuanZhuan" : "ui://common/bj_huanzhuang";
        this._skinLoader3D.skinName = skinName;
        this._skinLoader3D.animationName = type == ItemType.SKIN?"animation":"animation1";
        this._skinLoader3D.frame = 0;
        this._skinLoader3D.loop = false;
        this._type = type;
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
            // DataManager.inst.addDoubleScoreTimes(GameDef.VideoDoubleScoreTimes);
            if (this._type == ItemType.SKIN)
                SkinManager.inst.addSkinHave(skinIndex);
            else
                SkinManager.inst.addBgHave(skinIndex);
            this.Hide();
            Util.AppTip("Your get new skin!");
        });
    }
}