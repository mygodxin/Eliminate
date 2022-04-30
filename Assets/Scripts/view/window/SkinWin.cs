using mm;
using FairyGUI;
using UnityEngine;
using Spine.Unity;
using System;
using System.Collections.Generic;
public class SkinWin : AppScene
{
    public static string NAME = "SkinWin";

    GTextField _moneyTxt;
    GButton _closeBtn;
    GButton _moneyBtn;
    GButton _videoBtn;
    GButton _useBtn;
    Controller _progressCtrl;
    Controller _stateCtrl;
    GList _list;
    CSkin[] _skinArr;
    CBg[] _bgArr;
    GLoader3D _skinLoad3D;
    GLoader3D _bgLoad3D;
    int _selIndex = -1;

    public SkinWin() : base("SkinWin", "skin")
    {
    }
    override public void BindChild()
    {
        this._moneyTxt = this.GetTextField("moneyComp.money");
        this._closeBtn = this.getButton("closeButton");
        this._moneyBtn = this.getButton("moneyBtn");
        this._videoBtn = this.getButton("videoBtn");
        this._useBtn = this.getButton("useBtn");

        this._skinLoad3D = this.GetLoader3D("skinLoad3D");
        this._bgLoad3D = this.GetLoader3D("bgLoad3D");
        this._bgLoad3D.fill = FillType.Scale;

        this._progressCtrl = this.getController("progress");
        this._stateCtrl = this.getController("c1");

        this._list = this.getList("list");
        this._list.itemRenderer = this.itemRenderer;
        this._list.onClickItem.Add(this.onClickItem);

        this._skinArr = ConfigManager.inst.getShopConfig().skin;
        this._bgArr = ConfigManager.inst.getShopConfig().bg;
        // this.onRollOver.Add(this.onItemRollOver);
        // this.onRollOut.Add(this.onItemRollOut);
        var test = this.getButton("test");
        test.onClick.Add(() =>
        {
            DataManager.inst.addMoney(50);
        });
    }
    override public string[] registerEvent()
    {
        return new string[]{
            Notification.UpdateMoney,
            Notification.UpdateSkinTimes,
            Notification.UpdateSkin,
            Notification.UpdateBgTimes,
            Notification.UpdateBg,
            Notification.AddBg,
            Notification.AddSkin
        };
    }
    override public void onEvent(string eventName, object param)
    {
        if (eventName == Notification.UpdateMoney)
        {
            this.updateMoney();
        }
        else if (eventName == Notification.UpdateSkinTimes)
        {
            this.updateSkin((int)param);
            // this.updateBtn((int)param);
        }
        else if (eventName == Notification.UpdateSkin)
        {
            this.updateList(false);
            this.updateBtn((int)param);
        }
        else if (eventName == Notification.UpdateBgTimes)
        {
            this.updateSkin((int)param);
            // this.updateBtn((int)param);
        }
        else if (eventName == Notification.UpdateBg)
        {
            this.updateList(false);
            this.updateBtn((int)param);
        }
        else if (eventName == Notification.AddBg)
        {
            int idx = (int)param;
            this.updateList(false);
            this.updateBtn(idx);
            mm.AppWindow.show(SkinGetWin.NAME,new int[]{this._stateCtrl.selectedIndex,idx});
        }
        else if (eventName == Notification.AddSkin)
        {
            int idx = (int)param;
            this.updateList(false);
            this.updateBtn(idx);
 
            mm.AppWindow.show(SkinGetWin.NAME,new int[]{this._stateCtrl.selectedIndex,idx});
        }
    }
    public override void RefreshUi()
    {
        this._selIndex = 0;
        this.updateMoney();
        this._stateCtrl.onChanged.Add((EventContext ctx) =>
        {
            this.updateView(this._stateCtrl.selectedIndex);
        });
        this.updateView(this._stateCtrl.selectedIndex);
    }
    public override void OnClickButton(GButton button)
    {
        if (button == this._closeBtn)
        {
            mm.AppScene.show(StartScene.NAME);
            // this.randomAni();
            // this.Hide();
        }
        else if (button == this._moneyBtn)
        {
            this.clickMoney();
        }
        else if (button == this._videoBtn)
        {
            this.clickVideo();
        }
        else if (button == this._useBtn)
        {
            this.clickUse();
        }
    }

    void clickUse()
    {
        var stateIndex = this._stateCtrl.selectedIndex;
        if (stateIndex == 0)
            SkinManager.inst.setCurSkin(this._selIndex);
        else
            SkinManager.inst.setCurBg(this._selIndex);
    }

    void clickMoney()
    {
        int curMoney = DataManager.inst.getMoney();
        int needMoney = SkinManager.inst.getBuyPrice();
        if (curMoney < needMoney)
        {
            Util.AppTip("Money is not Enough");
            return;
        }
        int stateIndex = this._stateCtrl.selectedIndex;
        if (stateIndex == 0)
        {
            // int curTimes = SkinManager.inst.getBuyTimes();
            // // int needMoney = this._skinArr[this._selIndex].price[curTimes];
            // if (curMoney < needMoney)
            // {
            //     Util.AppTip("Money is not Enough");
            //     return;
            // }
            // SkinManager.inst.addSkinTimes(this._selIndex, 1);
            this.randomAni();
            DataManager.inst.addMoney(-needMoney);
        }
        else
        {
            // int curTimes = SkinManager.inst.getBgTimes(this._selIndex);
            // int needMoney = this._skinArr[this._selIndex].price[curTimes];
            // if (curMoney < needMoney)
            // {
            //     Util.AppTip("Money is not Enough");
            //     return;
            // }
            // SkinManager.inst.addSkinTimes(this._selIndex, 1);
            this.randomAni();
            DataManager.inst.addMoney(-needMoney);
        }
    }

    void clickVideo()
    {
        int curMoney = DataManager.inst.getMoney();
        int needMoney = SkinManager.inst.getBuyPrice() - SkinManager.inst.getBuyVideo();
        if (curMoney < needMoney)
        {
            Util.AppTip("Money is not Enough");
            return;
        }
        AdManager.inst.CreateVideoAd((isEnd) =>
        {
            if (isEnd)
            {
                DataManager.inst.addMoney(-needMoney);
                // SkinManager.inst.addSkinTimes(this._selIndex, 1);
                int stateIndex = this._stateCtrl.selectedIndex;
                if (stateIndex == 0)
                {
                    this.randomAni();
                    // DataManager.inst.addMoney(-needMoney);
                }
                else
                {
                    this.randomAni();
                    // DataManager.inst.addMoney(-needMoney);
                }
                // DataManager.inst.addMoney(int.Parse(this._videoBtn.data as string));
            }
        });
    }
    void updateView(int index)
    {
        this.updateList();
        // var stateIndex = index == 0 ? SkinManager.inst.getCurSkinIndex() : SkinManager.inst.getCurBgIndex();
        // this.updateBtn(this._selIndex);
        // var stateIndex = index == 0 ?
        string skinName;
        if (index == 0)
        {
            skinName = SkinManager.skinNameArr[this._selIndex];
            // string skinName = index == 0 ?
            this._skinLoad3D.skinName = skinName;
            this._skinLoad3D.animationName = "animation2";
        }
        else
        {
            skinName = SkinManager.skinNameArr[this._selIndex];
            // string skinName = index == 0 ?
            this._bgLoad3D.skinName = skinName;
            this._bgLoad3D.animationName = "animation2";
        }
    }
    void updateMoney()
    {
        this._moneyTxt.text = DataManager.inst.getMoney() + "";
    }
    void updateSkin(int idx)
    {
        // if (idx != this._selIndex) return;
        this.itemRenderer(idx, this._list.GetChildAt(idx));
        // this._list.numItems = 
        // this.updateList();
        if (idx == this._selIndex)
            this.updateBtn(idx);
    }
    void updateList(bool isReset = true)
    {
        var index = this._stateCtrl.selectedIndex;
        object data;
        if (index == 0)
        {
            data = this._skinArr;
        }
        else
        {
            data = this._bgArr;
        }
        // object data = index == 0 ? this._skinArr : this._bgArr;
        int len = (data as Array).Length;
        this._list.data = data;
        this._list.numItems = len;
        if (isReset)
        {
            this._list.selectedIndex = 0;
            EventContext ctx = new EventContext();
            ctx.data = this._list.GetChildAt(0);
            this.onClickItem(ctx);
        }
    }

    GComponent _toolTipComp;
    void onItemRollOver(EventContext eventContext)
    {
        // GRoot.inst.ShowPopup(SkinPreview);
        eventContext.StopPropagation();
        GLoader loader = eventContext.sender as GLoader;
        if (loader == null) return;
        int index = this._list.GetChildIndex(loader.parent);
        if (index >= 0)
        {
            CSkin skin = this._skinArr[index];
            string icon = skin.icon;
            string name = skin.name;
            // mm.AppWindow.show(SkinPreview.NAME,name);
            if (this._toolTipComp == null)
            {
                this._toolTipComp = /* new SkinPreview(); */UIPackage.CreateObject("skin", "SkinPreview").asCom;
                // this._toolTipComp.scaleX = this._toolTipComp.scaleY = 0.5f;
                //    list = this._toolTipComp.getList("list");
                this._toolTipComp.GetChild("list").asList.itemRenderer = this.tipItemRenderer;
                this._toolTipComp.touchable = false;
            }
            this._toolTipComp.data = SkinManager.skinNameArr[index];
            this._toolTipComp.GetChild("list").asList.numItems = 6;
            GRoot.inst.ShowPopup(this._toolTipComp);
            // Debug.Log("移入" + index);
        }
    }

    void tipItemRenderer(int index, GObject item)
    {
        GButton label = item.asButton;
        string icon = "ui://common/" + this._toolTipComp.data + "_" + index;
        label.icon = icon;
        label.title = "";
    }

    void onItemRollOut(EventContext eventContext)
    {
        eventContext.StopPropagation();
        // mm.AppWindow.hide(SkinPreview.NAME);
        GLoader loader = eventContext.sender as GLoader;
        if (loader == null) return;
        int index = this._list.GetChildIndex(loader.parent);
        if (index >= 0)
        {
            GRoot.inst.HidePopup();
            // Debug.Log("移除" + index);
        }
    }

    void itemRenderer(int index, GObject item)
    {
        var stateIndex = this._stateCtrl.selectedIndex;
        GButton label = item.asButton;
        if (stateIndex == 0)
        {

            CSkin skin = this._skinArr[index] as CSkin;
            string icon = skin.icon;
            string name = skin.name;
            int[] price = skin.price;
            int[] skinHaveList = SkinManager.inst.getSkinHaveList();
            label.icon = icon;
            if (skinHaveList[index] == 1)
            {
                label.GetController("time").selectedIndex = 0;
            }
            else
            {
                label.GetController("time").selectedIndex = 1;

                int totalTimes = skin.times;
                int curTimes = SkinManager.inst.getSkinTimes(index);
                label.title = curTimes + "/" + totalTimes;
            }
            int useIndex = SkinManager.inst.getCurSkinIndex();
            label.GetController("use").selectedIndex = index == useIndex ? 1 : 0;
            // comp.GetChild()
            label.data = name;
            GLoader loader = label.GetChild("icon").asLoader;
            loader.onRollOver.Remove(this.onItemRollOver);
            loader.onRollOut.Remove(this.onItemRollOut);
            loader.onRollOver.Add(this.onItemRollOver);
            loader.onRollOut.Add(this.onItemRollOut);
        }
        else
        {
            CBg bg = this._bgArr[index];
            string icon = bg.icon;
            string name = bg.name;
            int[] price = bg.price;
            int[] bgHaveList = SkinManager.inst.getBgHaveList();
            label.icon = icon;
            if (bgHaveList[index] == 1)
            {
                label.GetController("time").selectedIndex = 0;
            }
            else
            {
                label.GetController("time").selectedIndex = 1;

                int totalTimes = bg.times;
                int curTimes = SkinManager.inst.getBgTimes(index);
                label.title = curTimes + "/" + totalTimes;
            }
            // comp.GetChild()
            int useIndex = SkinManager.inst.getCurBgIndex();
            label.GetController("use").selectedIndex = index == useIndex ? 1 : 0;
            label.data = name;
            GLoader loader = label.GetChild("icon").asLoader;
            loader.onRollOver.Remove(this.onItemRollOver);
            loader.onRollOut.Remove(this.onItemRollOut);
        }
    }
    void onClickItem(EventContext eventContext)
    {
        int index = this._list.GetChildIndex(eventContext.data as GButton);
        this._selIndex = index;
        this.updateBtn(index);
    }

    void updateBtn(int index = -1)
    {
        // var price = SkinManager.inst.getBuyPrice();
        // var video = SkinManager.inst.getBuyVideo();
        // this._moneyBtn.title = price + "";
        // this._videoBtn.title = "+" + video;

        int stateIndex = this._stateCtrl.selectedIndex;
        if (stateIndex == 0)
        {
            int[] skinHaveList = SkinManager.inst.getSkinHaveList();
            if (skinHaveList[index] == 0)
            {
                var price = SkinManager.inst.getBuyPrice();
                var video = SkinManager.inst.getBuyVideo();
                this._moneyBtn.title = price + "";
                this._videoBtn.title = "+" + video;

                this._progressCtrl.selectedIndex = 1;
            }
            else
            {
                this._progressCtrl.selectedIndex = 2;
                int curSkin = SkinManager.inst.getCurSkinIndex();
                this._useBtn.enabled = curSkin != index;
            }
            this._skinLoad3D.skinName = SkinManager.skinNameArr[this._selIndex];
            this._skinLoad3D.animationName = "animation2";
        }
        else
        {
            int[] bgHaveList = SkinManager.inst.getBgHaveList();
            if (bgHaveList[index] == 0)
            {
                var price = SkinManager.inst.getBuyPrice();
                var video = SkinManager.inst.getBuyVideo();
                this._moneyBtn.title = price + "";
                this._videoBtn.title = "+" + video;

                this._progressCtrl.selectedIndex = 1;
            }
            else
            {
                this._progressCtrl.selectedIndex = 2;
                int curSkin = /* this._selIndex;// */SkinManager.inst.getCurBgIndex();
                this._useBtn.enabled = curSkin != index;
            }
            this._bgLoad3D.skinName = SkinManager.skinNameArr[this._selIndex];
            this._bgLoad3D.animationName = "animation2";
        }
    }

    int lotteryTimes = 3;
    void randomAni()
    {
        int stateIndex = this._stateCtrl.selectedIndex;
        if (stateIndex == 0)
        {
            var noHaveSkin = SkinManager.inst.getNoHaveSkinArr();
            if (noHaveSkin.Length > 0)
            {
                int len = noHaveSkin.Length;
                int idx = Util.getRandom(0, len);
                Debug.Log("抽中=" + idx);
                // Storage.SetInt(DataDef.RESULT_SKIN_INDEX, noHaveSkin[idx]);
                int tar = noHaveSkin[idx];
                // SkinManager.inst.addSkinTimes(tar, 1);
                int start = 0;
                int end = 0;
                Timers.inst.Add(0.1f, lotteryTimes * len + idx + 1, (object obj) =>
               {
                   int cur = Util.getRandom(0,len);
                   this.setSel(noHaveSkin[cur/* start */]);
                   start++;
                   end++;
                   if (start == noHaveSkin.Length)
                   {
                       start = 0;
                   }
                   if (end == lotteryTimes * len + idx + 1)
                   {
                       int playTimes = 0;
                       Timers.inst.Add(0.3f, 3, (object obj) =>
                       {
                           this.setSel(tar);
                           Timers.inst.Add(0.1f, 1, (object obj) =>
                           {
                               this.setSel(-1);
                               playTimes++;
                               if (playTimes == 3)
                               {
                                   this.setSel(-1);
                                   SkinManager.inst.addBuyTimes();
                                   SkinManager.inst.addSkinTimes(tar, 1);
                                   this.updateBtn(this._selIndex);
                               }
                           });
                       });
                       //    Timers.inst.Add(1, 1, (object obj) =>
                       //     {
                       //         this.setSel(-1);
                       //         SkinManager.inst.addBuyTimes();
                       //         SkinManager.inst.addSkinTimes(tar, 1);
                       //         this.updateBtn(this._selIndex);
                       //     });
                   }
               });
            }
        }
        else
        {
            var noHaveBg = SkinManager.inst.getNoHaveBgArr();
            if (noHaveBg.Length > 0)
            {
                int len = noHaveBg.Length;
                int idx = Util.getRandom(0, len);
                Debug.Log("抽中=" + idx);
                // Storage.SetInt(DataDef.RESULT_SKIN_INDEX, noHaveSkin[idx]);
                int tar = noHaveBg[idx];
                // SkinManager.inst.addBgTimes(tar, 1);
                int start = 0;
                int end = 0;
                Timers.inst.Add(0.1f, lotteryTimes * len + idx + 1, (object obj) =>
               {
                   int cur = Util.getRandom(0,len);
                   this.setSel(noHaveBg[cur/* start */]);
                   start++;
                   end++;
                   if (start == noHaveBg.Length)
                   {
                       start = 0;
                   }
                   if (end == lotteryTimes * len + idx + 1)
                   {
                       int playTimes = 0;
                       Timers.inst.Add(0.3f, 3, (object obj) =>
                       {
                           this.setSel(tar);
                           Timers.inst.Add(0.1f, 1, (object obj) =>
                           {
                               this.setSel(-1);
                               playTimes++;
                               if (playTimes == 3)
                               {
                                   this.setSel(-1);
                                   SkinManager.inst.addBuyTimes();
                                   SkinManager.inst.addBgTimes(tar, 1);
                                   this.updateBtn(this._selIndex);
                               }
                           });
                       });
                       //    Timers.inst.Add(1, 1, (object obj) =>
                       //    {
                       //        this.setSel(-1);
                       //        SkinManager.inst.addBuyTimes();
                       //        SkinManager.inst.addBgTimes(tar, 1);
                       //        this.updateBtn(this._selIndex);
                       //    });
                   }
               });
            }
        }
    }

    void setSel(int index)
    {
        for (int i = 0; i < this._list.numChildren; i++)
        {
            GButton label = this._list.GetChildAt(i).asButton;
            label.GetController("sel").selectedIndex = i == index ? 1 : 0;
        }
    }
}