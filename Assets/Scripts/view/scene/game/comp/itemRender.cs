using mm;
using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道具组件
/// </summary>
public class ItemRender : AppComp
{
    GTextField _title;
    GTextField _cdTextFiled;
    GImage _cdImage;
    Controller _cdCtrl;
    int cdMax;
    public ItemRender(GComponent viewComponent) : base(viewComponent)
    {
        // this._sameBallLabel = this.GetLabel("sameBall");
        // this._lightningLabel = this.GetLabel("lightning");
        // this._hammerLabel = this.GetLabel("hammer");
        // this._sameBallLabel.onClick.Add(this.useSameBall);
        // this._lightningLabel.onClick.Add(this.useLightning);
        // this._hammerLabel.onClick.Add(this.useHammer);
        this._title = this.GetTextField("title");
        this._cdTextFiled = this.GetTextField("cdTextFiled");
        this._cdImage = this.GetImage("imgCd");
        this._cdCtrl = this.GetController("cd");
    }

    public void onClick(EventCallback0 action)
    {
        this.viewComponent.onClick.Add(action);
    }
    public void onClick(EventCallback1 action)
    {
        this.viewComponent.onClick.Add(action);
    }

    public string title
    {
        get { return this._title.text; }
        set
        {
            this._title.text = value;
        }
    }

    public int cdValue
    {
        set
        {

            if (value < 0)
            {
                this._cdCtrl.selectedIndex = 0;
                this._cdImage.fillAmount = 0;
                this._cdTextFiled.text = "";
            }
            else
            {
                this._cdTextFiled.text = value + "";
                this._cdCtrl.selectedIndex = 1;
                this._cdImage.fillAmount = (float)value / this.cdMax;
            }
        }
    }

    private TimerCallback _cdTimeId;
    /** 更新某个玩家的CD */
    public void updateCd(int max = 30)
    {
        this.cdMax = max;
        var startTime = DateTime.Now;
        this._cdTimeId = (object obj) =>
           {
               var cd = this.cdMax + Util.difSeconds(DateTime.Now, startTime);
               //闹钟抖动
               if (cd <= 0)
               {
                   this.clearCd();
               }

               this.cdValue = ((short)cd);
           };
        this._cdTimeId?.Invoke(null);
        Timers.inst.Add(0.3f, 0, this._cdTimeId);
    }

    public bool existCd()
    {
        if (this._cdTimeId == null) return false;
        return Timers.inst.Exists(this._cdTimeId);
    }
    /** 清除cd */
    public void clearCd()
    {
        if (this._cdTimeId != null)
        {
            Timers.inst.Remove(this._cdTimeId);
        }
        this.cdValue = -1;
    }
}