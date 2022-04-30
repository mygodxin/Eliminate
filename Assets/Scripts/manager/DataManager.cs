
using UnityEngine;
using mm;
using System;
using System.Collections.Generic;

/// <summary>
/// 本地数据管理类 
/// </summary>
public class DataManager
{
    public static DataManager inst = new DataManager();

    int[] skinTimesArr;
    private int addGemCount = 10;
    public void init()
    {

    }

    public int getMoney()
    {
        return Storage.GetInt(DataDef.MONEY);
    }

    public void addMoney(int val)
    {
        int money = DataManager.inst.getMoney();
        money += val;
        Storage.SetInt(DataDef.MONEY, money);
        mvc.send(Notification.UpdateMoney, val);
    }

    public int getGem()
    {
        return Storage.GetInt(DataDef.GEM);
    }

    public void addGem(int val = 10)
    {
        int gem = DataManager.inst.getGem();
        gem += val;
        Storage.SetInt(DataDef.GEM, gem);
        mvc.send(Notification.UpdateGem, val);
    }

    public void addDoubleScoreTimes(int val)
    {
        int times = DataManager.inst.getDoubleScoreTimes();
        times += val;
        Storage.SetInt(DataDef.DOUBLE_SCORE_TIMES, times);
        // mvc.send(Notification.UpdateMoney, val);
    }
    public int getDoubleScoreTimes()
    {
        return Storage.GetInt(DataDef.DOUBLE_SCORE_TIMES);
    }
    public T PaseJson<T>(string jsonString)
    {
        return JsonUtility.FromJson<T>(jsonString);
    }

    private string ToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    private ItemWrap getItemWrap()
    {
        if (!PlayerPrefs.HasKey(DataDef.ITEM))
        {
            var itemList = new ItemWrap();
            itemList.init();
            string json = this.ToJson(itemList);
            PlayerPrefs.SetString(DataDef.ITEM, json);
        }
        var str = PlayerPrefs.GetString(DataDef.ITEM);
        return this.PaseJson<ItemWrap>(str);
    }

    public int getItem(ItemDef type)
    {
        var itemWrap = this.getItemWrap();
        return itemWrap.getItemCount(type);
    }

    public void addItem(ItemDef type, int count)
    {
        var itemWarp = this.getItemWrap();
        itemWarp.addItemCount(type, count);
    }

    public bool useItem(ItemDef type, int count = 1)
    {
        var total = DataManager.inst.getItem(ItemDef.SAME_BALL);
        if (total < count)
        {
            Util.AppTip("Item count is not enough");
            return false;
        }
        DataManager.inst.addItem(ItemDef.SAME_BALL, -1);
        return true;
    }
}