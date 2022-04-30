using mm;
using FairyGUI;
using UnityEngine;

public class StartScene : AppScene
{
    public static string NAME = "StartScene";

    GTextField _moneyTxt;
    GButton _startBtn;
    GButton _fightBtn;
    GButton _personBtn;
    GButton _adBtn;
    GButton _skinBtn;
    GButton _setBtn;
    GButton _videoBtn;
    GLoader3D _bgLoader3D;
    GGroup _videoGroup;
    public StartScene() : base("StartScene", "start")
    {
    }
    override public void BindChild()
    {
        this._moneyTxt = this.GetTextField("moneyComp.money");
        this._startBtn = this.getButton("startBtn");
        this._fightBtn = this.getButton("fightBtn");
        this._personBtn = this.getButton("personBtn");
        this._adBtn = this.getButton("adBtn");
        this._skinBtn = this.getButton("skinBtn");
        this._setBtn = this.getButton("setBtn");
        this._videoBtn = this.getButton("videoBtn");
        this._bgLoader3D = this.GetLoader3D("bg");
        this._bgLoader3D.animationName = "animation";

        // this._videoGroup = this.getGroup("videoGroup");
        // this._videoGroup.onClick.Add(this.clickVideo);
    }
    override public string[] registerEvent()
    {
        return new string[]{
            Notification.UpdateMoney
        };
    }
    override public void onEvent(string eventName, object param)
    {
        if (Notification.UpdateMoney == eventName)
        {
            this.updateMoney();
        }
    }
    public override void RefreshUi()
    {
        this.updateMoney();
    }

    void updateMoney()
    {
        int money = DataManager.inst.getMoney();
        this._moneyTxt.text = money + "";
    }
    public override void OnClickButton(GButton button)
    {
        if (button == this._startBtn || button == this._fightBtn)
        {
            mm.AppScene.show(PKWin.NAME);
        }
        else if (button == this._personBtn)
        {
            mm.AppScene.show(GameScene.NAME);
        }
        else if (button == this._adBtn)
        {
            this.ClickAd();
        }
        else if (button == this._skinBtn)
        {
            this.ClickSkip();
        }
        else if (button == this._setBtn)
        {
            this.ClickSet();
        }
        else if (button == this._videoBtn)
        {
            this.clickVideo();
        }
    }
    void clickVideo()
    {
        mm.AppWindow.show(BuffWin.NAME);
    }

    void ClickSkip()
    {
        mm.AppScene.show(SkinWin.NAME);
    }

    void ClickAd()
    {
        // PlayerPrefs.DeleteAll();
        DataManager.inst.addMoney(100);
    }
    void ClickSet()
    {
        mm.AppWindow.show(SetWin.NAME);
    }
}