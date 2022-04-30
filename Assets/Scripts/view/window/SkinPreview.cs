using mm;
using FairyGUI;
using UnityEngine;
using Spine.Unity;
using System;
public class SkinPreview : AppWindow
{
    public static string NAME = "SkinPreview";

    GList _list;
    public SkinPreview() : base("SkinPreview", "skin")
    {
        this.isPopup = true;
        this.touchable = false;
        this.modal = false;
    }
    override public void BindChild()
    {
        this._list = this.getList("list");
        this._list.itemRenderer = this.itemRenderer;
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
        CSkin[] skin = this.data as CSkin[];
        this._list.data = SkinManager.skinNameArr[(int)this.data];
        this._list.numItems = 6;
    }

    void itemRenderer(int index, GObject item)
    {
        GButton label = item.asButton;
        label.GetController("price").selectedIndex = 0;
        string icon = "ui://common/" + this._list.data + "_" + index;
        label.icon = icon;
        label.title = "";
    }
}