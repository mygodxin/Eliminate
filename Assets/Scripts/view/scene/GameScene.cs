using mm;
using FairyGUI;
using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// 游戏主场景
/// </summary>
public class GameScene : AppScene
{
    public static string NAME = "GameScene";

    GButton _testBtn;
    GButton _backBtn;
    GTextField _otherScoreTxt;
    GTextField _myScoreTxt;
    GTextField _myScoreTxt1;
    GTextField _myMoneyTxt;
    GLoader3D _star;
    GLoader3D _qp;
    GLoader _bg;
    // GLoader _qipan;
    GGraph _myGraph;
    GGraph _otherGraph;
    GridComp _gridComp;
    ItemComp _itemComp;
    Controller _modeCtrl;
    Controller _turnTipCtrl;
    GLoader3D _turnTipLoader3D;
    GameMode _gameMode;
    int _revireTimes;
    GLabel _countdownLabel;
    GLoader3D _itemLoader3D;
    GLoader3D _emLoader3D;
    Controller _newRecordCtrl;

    public GameScene() : base("GameScene", "game")
    {
    }
    override public void BindChild()
    {
        this._backBtn = this.getButton("backBtn");

        this._otherScoreTxt = this.GetTextField("otherScore");
        this._myScoreTxt = this.GetTextField("myScore");
        this._myScoreTxt1 = this.GetTextField("myScore1");
        this._myMoneyTxt = this.GetTextField("moneyComp.money");
        this._myGraph = this.getGraph("myGraph");
        this._otherGraph = this.getGraph("otherGraph");
        this._newRecordCtrl = this.getController("newRecord");

        // this._star = this.GetLoader3D("bg");
        // this._star.animationName = "animation";

        var empty = this.getGraph("emptyComp");
        this._testBtn = this.getButton("testBtn");
        this._testBtn.visible = false;

        this._modeCtrl = this.getController("mode");
        GameObject defen = Resources.Load<GameObject>("map/1/Grid");//"EF/baozha/FeiXing_dx");
        GameObject gameObject = UnityEngine.Object.Instantiate(defen);
        GoWrapper go = new GoWrapper(gameObject);
        empty.SetNativeObject(go);
        this._bg = this.GetLoader("bg");
        // this._qipan = this.GetLoader("qipan");
        this._qp = this.GetLoader3D("qpLoader3D");
        this._itemLoader3D = this.GetLoader3D("itemLoader3D");
        this._emLoader3D = this.GetLoader3D("emLoader3D");

        this._turnTipCtrl = this.getController("turnTip");
        this._turnTipLoader3D = this.GetLoader3D("turnTip");
        this._countdownLabel = this.GetLabel("countdown");

        this._gridComp = new GridComp(this.GetComp("gridComp"));
        this._itemComp = new ItemComp(this.GetComp("itemComp"));
    }
    override public string[] registerEvent()
    {
        return new string[]{
            Notification.ScoreUpdate,
            Notification.GameEnd,
            Notification.OtherSelect,
            Notification.MoveEnd,
            Notification.Eliminate,
            Notification.CheckAndMoveEnd,
            Notification.CloseGameEnd,
            Notification.CreateNewGrid,
            Notification.ElimnateGrid,
            Notification.ElimIceCube,
            Notification.ElimPollution,
            Notification.ElimBaoShi,
            Notification.ShowTurnTip,
            Notification.ShowCountdown,
            Notification.UseItem,
            Notification.UseLightning,
            Notification.UseSameBall,
            Notification.UseHammer,
            Notification.EnableHammer,
            Notification.UnableHammer,
            Notification.ActiveEM,
            Notification.NewRecord,
            Notification.UpdateGem,
            Notification.UpdateMoney,
            Notification.InitMap
        };
    }
    override public void onEvent(string eventName, object param)
    {
        if (eventName == Notification.ScoreUpdate)
        {
            this.onScoreUpdate(param);
        }
        else if (eventName == Notification.GameEnd)
        {
            this.OnGameEnd();
        }
        else if (eventName == Notification.OtherSelect)
        {
            this._gridComp.otherMove(param as Point[]);
        }
        else if (eventName == Notification.MoveEnd)
        {
            this._gridComp.onMoveEnd(param);
        }
        else if (eventName == Notification.Eliminate)
        {
            // this.onEliminate(param as List<Point>);
        }
        else if (eventName == Notification.CheckAndMoveEnd)
        {
            this._gridComp.onCheckAndMoveEnd(param as List<Point>, this._myGraph, this._otherGraph);
        }
        else if (eventName == Notification.CreateNewGrid)
        {
            this._gridComp.onCreateNewGrid(param as Point);
        }
        else if (eventName == Notification.ElimnateGrid)
        {
            this._gridComp.onElimnateGrid(param as Point);
        }
        else if (eventName == Notification.ElimIceCube)
        {
            this._gridComp.onElimIceCube(param as Point);
        }
        else if (eventName == Notification.ElimPollution)
        {
            this._gridComp.onElimPollution(param as Point);
        }
        else if (eventName == Notification.ElimBaoShi)
            this._gridComp.onElimBaoShi(param as Point);
        else if (eventName == Notification.ShowTurnTip)
            this.showTurnTip();
        else if (eventName == Notification.ShowCountdown)
            this.showCountdown();
        else if (eventName == Notification.UseItem)
        {
            // ItemDef type;
            // if (param != null)
            //     type = ()param;
            this.useItem((ItemDef)param);
        }
        else if (eventName == Notification.UseHammer)
        {
            // this.useItem(ItemDef.HAMMER, (Point)param);
            this._itemComp.useHammer();
        }
        else if (eventName == Notification.UseLightning)
        {
            this.useItem(ItemDef.LIGHTNING);
        }
        else if (eventName == Notification.UseSameBall)
        {
            this.useItem(ItemDef.SAME_BALL);
        }
        else if (eventName == Notification.EnableHammer)
        {
            this._gridComp.enableHammer();
        }
        else if (eventName == Notification.UnableHammer)
            this._gridComp.unableHammer();
        else if (eventName == Notification.ActiveEM)
        {
            if (this._emLoader3D.animationName != EEmType.WAKE)
            {
                this._emLoader3D.animationName = EEmType.WAKE;
            }
        }
        else if (eventName == Notification.NewRecord)
        {
            this._newRecordCtrl.selectedIndex = 1;
        }
        else if (eventName == Notification.UpdateGem)
        {
            Util.AppTip("You get 10 gem！");
            this._itemComp.updateCount();
        }else if(eventName == Notification.UpdateMoney){
            this.updateMoney();
        }else if(eventName == Notification.InitMap){
            this._gridComp.initMap();
        }
    }
    public override void RefreshUi()
    {
        // if (this.openData != null)
        // {
        //     this._gameMode = GameMode.FIGHT;
        // }
        // else
        //     this._gameMode = GameMode.PERSON;
        this._newRecordCtrl.selectedIndex = 0;
        this._modeCtrl.selectedIndex = this.openData != null ? 0 : 1;
        //复活次数
        this._revireTimes = this._modeCtrl.selectedIndex == 1 ? 1 : 0;

        this._itemComp.updateCount();
        this.updateBg();
        this.updateScore();
        this.updateMoney();
        this.hideCountdown();
        // this.showEm();
        this._emLoader3D.animationName = EEmType.SLEEP;
        this._gridComp.init(this.openData);

        // if (GameCommond.inst.isFinished())
        // {
        //     mvc.send(Notification.GameEnd);
        // }
        // this.showCountdown();
    }

    void updateBg()
    {
        this._bg.url = "ui://game/" + SkinManager.inst.getCurBgName();
        // this._qipan.url = "ui://game/qipan" + SkinManager.skinNameArr[SkinManager.inst.getCurBgIndex()];
        this._qp.skinName = SkinManager.skinNameArr[SkinManager.inst.getCurBgIndex()] + "";
        this._qp.animationName = "2";
    }

    void updateScore()
    {
        // int curPlayer = (int)obj;
        // if (curPlayer == GameDef.SELF)
        // {
        this._myScoreTxt1.text = this._myScoreTxt.text = GameCommond.inst.myScore + "";
        this._otherScoreTxt.text = GameCommond.inst.otherScore + "";
        // }
    }

    void updateMoney()
    {
        int money = DataManager.inst.getMoney();
        this._myMoneyTxt.text = money + "";
    }

    public override void OnClickButton(GButton button)
    {
        if (button == this._backBtn)
            this.clickBack();
        else if (button == this._testBtn)
        {
            this.clickTest();
        }
    }
    void clickTest()
    {
        this.RefreshUi();
    }
    void clickBack()
    {
        // mm.AppScene.show(StartScene.NAME);
        // Util.appTip("你好啊====");
        Util.AppAlert("ARE YOU SURE YOU WANT TO QUIT THE GAME?", mm.EAlertType.DOUBLE, () =>
        {
            mm.AppScene.show(StartScene.NAME);
        });
    }

    void onScoreUpdate(object obj)
    {
        this.updateScore();
    }

    void OnGameEnd()
    {
        // Util.AppAlert("游戏结束");
        if (this._revireTimes > 0)
        {
            mm.AppWindow.show(ReviveWin.NAME);
        }
        else
            mm.AppWindow.show(GameEndWin.NAME);
    }

    void showTurnTip()
    {
        this._turnTipCtrl.selectedIndex = 1;
        this._turnTipLoader3D.animationName = "animation";
        Timers.inst.Add(1, 1, (object obj) =>
        {
            this.hideTurnTip();
        });
    }
    void hideTurnTip()
    {
        this._turnTipCtrl.selectedIndex = 0;
        this._turnTipLoader3D.animationName = null;
    }

    int _countdownTime;
    void showCountdown()
    {
        this._countdownTime = ConfigManager.inst.getGameConfig().countdownTime;
        this._countdownLabel.visible = true;
        this.updateCountdown(null);
        Timers.inst.Add(1, 0, this.updateCountdown);
    }
    void updateCountdown(object obj)
    {
        if (this._countdownTime > 0)
        {
            this._countdownLabel.title = this._countdownTime + "s";
            this._countdownTime--;
        }
        else
        {
            this.hideCountdown();
            mm.AppWindow.show(GameEndWin.NAME);
        }
    }
    void hideCountdown()
    {
        if (Timers.inst.Exists(this.updateCountdown))
            Timers.inst.Remove(this.updateCountdown);
        this._countdownLabel.visible = false;
    }

    protected override void OnHide()
    {
        base.OnHide();

        this.hideCountdown();
    }

    void useItem(ItemDef type, Point p = null)
    {
        var aniName = "";
        switch (type)
        {
            case ItemDef.SAME_BALL:
                aniName = "caideng";
                this._itemLoader3D.x = this.width / 2 - 50;
                this._itemLoader3D.y = this.height / 2 - 50;
                break;
            case ItemDef.LIGHTNING:
                aniName = "shandian";
                this._itemLoader3D.x = this.width / 2 - 303;
                this._itemLoader3D.y = this.height / 2 - 1110;
                break;
            case ItemDef.HAMMER:
                aniName = "chuizi";
                // this._itemLoader3D.x = p.x;
                // this._itemLoader3D.y = p.y;
                break;
        }
        this._itemLoader3D.url = "ui://game/" + aniName;
        this._itemLoader3D.skinName = "default";
        this._itemLoader3D.animationName = aniName;
        this._itemLoader3D.loop = false;
        Spine.AnimationState.TrackEntryDelegate boomComplete = null;
        boomComplete = delegate (Spine.TrackEntry trackEntry)
                {
                    switch (type)
                    {
                        case ItemDef.SAME_BALL:
                            GameCommond.inst.elimRandomColorGrid();
                            break;
                        case ItemDef.LIGHTNING:
                            GameCommond.inst.elimRandomCount();
                            break;
                        case ItemDef.HAMMER:
                            GameCommond.inst.hitGrid(0);
                            break;
                    }
                    this._itemLoader3D.spineAnimation.state.Complete -= boomComplete;
                };
        this._itemLoader3D.spineAnimation.state.Complete += boomComplete;
    }

    // void setEm(EEmType type)
    // {
    //     this._emLoader3D.animationName = EEmType.WAKE;
    // }
}