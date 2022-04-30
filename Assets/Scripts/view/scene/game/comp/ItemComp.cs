using mm;
using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具组件
/// </summary>
public class ItemComp : AppComp
{
    private GTextField _gemLabel;
    private ItemRender _sameBallLabel;
    private ItemRender _lightningLabel;
    private ItemRender _hammerLabel;
    private GButton _btnVideo;

    public ItemComp(GComponent viewComponent) : base(viewComponent)
    {
        this._sameBallLabel = new ItemRender(this.GetComp("sameBall"));
        this._lightningLabel = new ItemRender(this.GetComp("lightning"));
        this._hammerLabel = new ItemRender(this.GetComp("hammer"));
        this._gemLabel = this.GetTextField("gemTxt");
        this._btnVideo = this.GetButton("v0");
        this._btnVideo.onClick.Add(this.clickVideo);
        // this.GetLabel("s").onClick
        this._sameBallLabel.onClick(this.useSameBall);
        this._lightningLabel.onClick(this.useLightning);
        this._hammerLabel.onClick(this.onDragStart);
    }

    public void updateCount()
    {
        // this._sameBallLabel.title = DataManager.inst.getItem(ItemDef.SAME_BALL) + "";
        // this._lightningLabel.title = DataManager.inst.getItem(ItemDef.LIGHTNING) + "";
        // this._hammerLabel.title = DataManager.inst.getItem(ItemDef.HAMMER) + "";
        CShop shop = ConfigManager.inst.getShopConfig();
        this._sameBallLabel.title = shop.item[0] + "";
        this._lightningLabel.title = shop.item[1] + "";
        this._hammerLabel.title = shop.item[2] + "";
        this._gemLabel.text = DataManager.inst.getGem() + "";
    }

    private void clickVideo()
    {
        AdManager.inst.CreateVideoAd((isEnd) =>
        {
            DataManager.inst.addGem();
        });
    }

    void useSameBall()
    {
        if (GameCommond.inst.curPlayer == -1) return;
        if (this._sameBallLabel.existCd())
        {
            Util.AppTip("CD, please use it later");
            return;
        }
        if (DataManager.inst.getMoney() < int.Parse(this._sameBallLabel.title))
        {
            Util.AppTip("Money is not enough");
            return;
        }
        this.setCd();
        // if (DataManager.inst.useItem(ItemDef.SAME_BALL))
        // GameCommond.inst.elimRandomColorGrid();
        DataManager.inst.addMoney(-int.Parse(this._sameBallLabel.title));
        mvc.send(Notification.UseSameBall, ItemDef.SAME_BALL);
    }
    void useLightning()
    {
        if (GameCommond.inst.curPlayer == -1) return;
        if (this._sameBallLabel.existCd())
        {
            Util.AppTip("CD, please use it later");
            return;
        }
        if (DataManager.inst.getMoney() < int.Parse(this._lightningLabel.title))
        {
            Util.AppTip("Money is not enough");
            return;
        }
        this.setCd();
        // if (DataManager.inst.useItem(ItemDef.LIGHTNING))
        // GameCommond.inst.elimRandomCount();
        DataManager.inst.addMoney(-int.Parse(this._lightningLabel.title));
        mvc.send(Notification.UseLightning, ItemDef.LIGHTNING);
    }
    public void useHammer()
    {
        if (GameCommond.inst.curPlayer == -1) return;
        if (this._sameBallLabel.existCd())
        {
            Util.AppTip("CD, please use it later");
            return;
        }
        this.setCd();
        // if (DataManager.inst.useItem(ItemDef.HAMMER))
        // GameCommond.inst.hitGrid(0);
        // mvc.send(Notification.ActiveHammer, ItemDef.HAMMER);
        // this._hammerLabel.viewComponent.draggable = true;
        // this._hammerLabel.viewComponent.onDragStart.Add(onDragStart);

    }
    public void enableHammer()
    {
        // this._canClick = false;
        // if (this._hammerComp == null)
        // {
        //     this._hammerComp = new GLoader();
        //     this._hammerComp.url = "ui://common/daoju3";
        // }
        // this._itemLayer.AddChild(this._hammerComp);
    }
    void onDragStart(EventContext context)
    {
        if (GameCommond.inst.curPlayer == -1) return;
        if (this._sameBallLabel.existCd())
        {
            Util.AppTip("CD, please use it later");
            return;
        }
        if (DataManager.inst.getMoney() < int.Parse(this._hammerLabel.title))
        {
            Util.AppTip("Money is not enough");
            return;
        }
        //取消掉源拖动
        context.PreventDefault();
        InputEvent evt = context.inputEvent;
        //icon是这个对象的替身图片，userData可以是任意数据，底层不作解析。context.data是手指的id。
        DragDropManager.inst.StartDrag(null, "ui://common/daoju3", 1, (int)evt.touchId);

        DragDropManager.inst.dragAgent.onDragEnd.Add(onDragEnd);
        mvc.send(Notification.EnableHammer, ItemDef.HAMMER);
    }
    void onDragEnd(EventContext context)
    {
        // var obj = GRoot.inst.touchTarget;
        Debug.Log("拖动结束" + context);
        mvc.send(Notification.UnableHammer);
    }
    void setCd()
    {
        // return;
        this._sameBallLabel.updateCd();
        this._lightningLabel.updateCd();
        this._hammerLabel.updateCd();
    }
}