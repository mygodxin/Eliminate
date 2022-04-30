using mm;
using FairyGUI;
using UnityEngine;

public class GameEndWin : AppWindow
{
    public static string NAME = "GameEndWin";

    GTextField _moneyTxt;
    GTextField _scoreTxt;
    GTextField _skinTxt;
    GButton _videoBtn;
    GButton _closeBtn;
    GLoader _skinLoader;
    GLoader3D _skinLoad3D;
    Controller _winCtrl;
    Controller _skinCtrl;
    Controller _typeCtrl;
    public GameEndWin() : base("GameEndWin", "end")
    {
    }
    override public void BindChild()
    {
        this._winCtrl = this.getController("winCtrl");
        this._moneyTxt = this.GetTextField("moneyComp.moneyTxt");
        this._scoreTxt = this.GetTextField("scoreTxt");
        this._skinTxt = this.GetTextField("skinTxt");
        this._videoBtn = this.getButton("videoBtn");
        this._closeBtn = this.getButton("noBtn");
        this._skinLoader = this.GetLoader("skinLoader");
        this._skinLoad3D = this.GetLoader3D("skinLoader3D");
        this._skinCtrl = this.getController("skin");
        this._typeCtrl = this.getController("type");
        this._skinLoader.onClick.Add(this.clickSkin);
        this._skinLoad3D.onClick.Add(this.clickSkin);
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
        this._videoBtn.enabled = true;
        this._closeBtn.visible = false;
        Timers.inst.Add(2, 1, (object obj) =>
        {
            Debug.Log("结算界面按钮=====");
            this._closeBtn.visible = true;
        });
        this.setCloseBtn();
        this.setScore();
        this.updateSkin();
    }
    public override void OnClickButton(GButton button)
    {
        if (button == this._closeBtn)
        {
            this.ClickNo();
        }
        else if (button == this._videoBtn)
        {
            this.ClickVideo();
        }
    }
    void setCloseBtn()
    {
        // this._videoBtn.enabled = true;
        // this._closeBtn.visible = false;
        // Timers.inst.Add(1, 1, (object obj) =>
        // {
        //     this._closeBtn.visible = true;
        // });
    }
    void setScore()
    {
        int score = GameCommond.inst.myScore;
        // int doubleTimes = DataManager.inst.getDoubleScoreTimes();
        // if (doubleTimes > 0)
        // {
        //     score *= 2;
        //     DataManager.inst.addDoubleScoreTimes(-1);
        // }
        this._scoreTxt.text = score + "";

        int money = score * GameDef.ScoreToMoney;
        this._moneyTxt.text = money + "";

        int otherScore = GameCommond.inst.otherScore;
        this._winCtrl.selectedIndex = score > otherScore ? 0 : 1;
    }
    int _skinIndex;
    void updateSkin()
    {
        this._skinCtrl.selectedIndex = 1;
        int type = SkinManager.inst.getResultItemType();
        if (type < 0)
        {
            this.resetSkin();
            return;
        }
        this._typeCtrl.selectedIndex = type;
        int skinIndex;
        if (type == ItemType.SKIN)
            skinIndex = SkinManager.inst.getResultSkinIndex();
        else
            skinIndex = SkinManager.inst.getResultBgIndex();

        if (skinIndex <= 0)
        {
            this.resetSkin();
            return;
        }
        int times = SkinManager.inst.getResultSkinProgressFree();
        if (times > 1)
        {
            SkinManager.inst.addResultSkinProgressFree(-1);
            SkinManager.inst.addResultSkinProgress(50);
        }
        else
        {
            int addProgress = Util.getRandom(5, 10);
            SkinManager.inst.addResultSkinProgress(addProgress);
        }
        this._skinLoad3D.url = type == ItemType.SKIN ? "ui://common/HuanZhuan" : "ui://common/bj_huanzhuang";
        this._skinLoad3D.skinName = SkinManager.skinNameArr[skinIndex];
        this._skinLoad3D.animationName = "animation2";

        // string skinName = SkinManager.skinNameArr[skinIndex];
        // this._skinLoader.url = "";//"ui://common/" + skinName + "_0";
        int progress = SkinManager.inst.getResultSkinProgress();
        if (progress > 100) progress = 100;
        this._skinTxt.text = progress + "/100";
        this._skinIndex = skinIndex;
    }
    void resetSkin()
    {
        this._skinTxt.text = "";
        this._skinLoader.url = "";
        this._skinCtrl.selectedIndex = 0;
    }
    void clickSkin()
    {
        if (this._skinTxt.text == "100/100")
        {
            this._skinLoader.url = "";
            this._skinTxt.text = "";
            // this._skinLoad3D.visible = false;
            this._skinCtrl.selectedIndex = 0;
            // SkinManager.inst.resetResultSkinProgress();
            // SkinManager.inst.resetResultSkinIndex();
            mm.AppWindow.show(SkinRewardWin.NAME, this._skinIndex);
            // SkinManager.inst.resetResultItemType();
        }
        else
        {
            Util.AppTip("进度不足，清继续努力！");
        }
    }
    void ClickNo()
    {
        // if (this._skinTxt.text == "100/100" || this._skinTxt.text == "")
        // {
        //     // SkinManager.inst.resetResultSkinIndex();
        //     SkinManager.inst.resetResultItemType();
        // }
        // SkinManager.inst.resetResultSkinProgress();
        // DataManager.inst.addMoney(GameCommond.inst.myScore);
        this.Hide();
        mm.AppScene.show(StartScene.NAME);
        mvc.send(Notification.CloseGameEnd);
    }
    void ClickVideo()
    {
        AdManager.inst.CreateVideoAd((isEnd) =>
        {
            int money = int.Parse(this._moneyTxt.text) * 2;
            this._moneyTxt.text = money + "";
            this._videoBtn.enabled = false;
        });
    }
    // void updateMoney()
    // {
    //     int money = GameCommond.inst.myScore;
    //     this._moneyTxt.text = money + "";
    // }
    protected override void OnHide()
    {
        base.OnHide();

        int progress = SkinManager.inst.getResultSkinProgress();
        if (progress > 100) progress = 100;
        if (progress == 100)
            SkinManager.inst.resetResultItemType();

        int money = int.Parse(this._moneyTxt.text);
        DataManager.inst.addMoney(money);
        Debug.Log("金币增加=" + money);
    }
}