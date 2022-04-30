using UnityEngine;
using System;
using mm;
using System.Collections.Generic;

public class SkinNameDef
{
    public const string Default = "A";
    public const int Fruit = 1;
}

public class SkinManager
{
    public static SkinManager inst = new SkinManager();

    public const int MAX_SKIN_COUNT = 9;
    public const int MAX_BG_COUNT = 9;
    public const int COLOR_COUNT = 6;
    public static readonly string[] skinNameArr = new string[MAX_SKIN_COUNT] {
        //  "default", "fruit","animal","cake","gem","monster","sweet","qiu","xc"
        // "A","E","I","G","B","H","F","C","D"
        "A","B","C","D","E","F","G","H","I"
    };
    // public static readonly string[] skinNameShopArr = new string[MAX_SKIN_COUNT] {
    //      "default", "fruit","animal","cake","gem","monster","sweet","qiu","xc"
    //     // "A","E","I","G","B","H","F","C","D"
    // };
    public int getBuyPrice()
    {
        // int price = 0;
        int buyTimes = this.getBuyTimes();
        int[] priceArr = ConfigManager.inst.getShopConfig().price;
        if (buyTimes < (priceArr.Length - 1))
        {
            return priceArr[buyTimes];
        }
        else
        {
            return priceArr[priceArr.Length - 1];
        }
        // return price;
    }
    public int getBuyVideo()
    {
        // int price = 0;
        int buyTimes = this.getBuyTimes();
        int[] videoArr = ConfigManager.inst.getShopConfig().video;
        if (buyTimes < (videoArr.Length - 1))
        {
            return videoArr[buyTimes];
        }
        else
        {
            return videoArr[videoArr.Length - 1];
        }
        // return price;
    }
    public int getBuyTimes()
    {
        return Storage.GetInt(DataDef.BUY_TIMES);
    }
    public void addBuyTimes()
    {
        int buyTimes = this.getBuyTimes();
        buyTimes++;
        Storage.SetInt(DataDef.BUY_TIMES, buyTimes);
    }
    public string getCurSkinName()
    {
        int idx = this.getCurSkinIndex();
        return skinNameArr[idx];
    }
    public string getCurBgName()
    {
        int idx = this.getCurBgIndex();
        return "bj" + skinNameArr[idx];
    }

    public int getCurSkinIndex()
    {
        return Storage.GetInt(DataDef.SKIN);
    }

    public void setCurSkin(int idx)
    {
        Storage.SetInt(DataDef.SKIN, idx);
        mvc.send(Notification.UpdateSkin, idx);
    }

    public int[] getSkinArr()
    {
        int[] skinList = Storage.GetIntArray(DataDef.SKIN_TIMES_LIST);
        if (skinList.Length == 0)
        {
            skinList = new int[MAX_SKIN_COUNT];
        }
        return skinList;
    }

    public void addSkinTimes(int idx, int time)
    {
        int[] skinList = this.getSkinArr();
        skinList[idx] += time;
        Storage.SetIntArray(DataDef.SKIN_TIMES_LIST, skinList);
        int totalTimes = ConfigManager.inst.getShopConfig().skin[idx].times;
        if (skinList[idx] >= totalTimes)
        {
            this.addSkinHave(idx);
        }
        mvc.send(Notification.UpdateSkinTimes, idx);
    }

    public int[] getSkinHaveList()
    {
        int[] skinHaveList = Storage.GetIntArray(DataDef.SKIN_HAVE_LIST);
        if (skinHaveList.Length == 0)
        {
            skinHaveList = new int[MAX_SKIN_COUNT] { 1, 0, 0, 0, 0, 0, 0, 0, 0 };
        }
        return skinHaveList;
    }
    public void addSkinHave(int val)
    {
        int[] skinHaveList = this.getSkinHaveList();
        skinHaveList[val] = 1;
        Storage.SetIntArray(DataDef.SKIN_HAVE_LIST, skinHaveList);
        mvc.send(Notification.AddSkin, val);
    }

    public int getSkinTimes(int index)
    {
        int[] skinTimesArr = Storage.GetIntArray(DataDef.SKIN_TIMES_LIST);
        if (skinTimesArr.Length == 0)
        {
            skinTimesArr = new int[MAX_SKIN_COUNT];
        }
        return skinTimesArr[index];
    }
    public int getResultSkinIndex()
    {
        int index = Storage.GetInt(DataDef.RESULT_SKIN_INDEX);
        if (index == 0)
        {
            index = this.resetResultSkinIndex();
        }
        return index;
    }
    public int resetResultSkinIndex()
    {
        this.resetResultSkinProgress();
        int[] noHaveArr = this.getNoHaveSkinArr();
        if (noHaveArr.Length > 0)
        {
            int idx = Util.getRandom(0, noHaveArr.Length);
            Storage.SetInt(DataDef.RESULT_SKIN_INDEX, noHaveArr[idx]);
            return noHaveArr[idx];
        }
        return -1;
    }

    public int[] getNoHaveSkinArr()
    {
        List<int> list = new List<int>();
        // CSkin[] skinArr = ConfigManager.inst.getShopConfig().skin;
        int[] haveArr = SkinManager.inst.getSkinHaveList();
        for (int i = 0; i < MAX_SKIN_COUNT; i++)
        {
            if (haveArr[i] == 0)
                list.Add(i);
        }
        return list.ToArray();
    }
    public int getResultSkinProgress()
    {
        return Storage.GetInt(DataDef.RESULT_SKIN_PROGRESS);
    }
    public void addResultSkinProgress(int val)
    {
        int progress = this.getResultSkinProgress();
        progress += val;
        Storage.SetInt(DataDef.RESULT_SKIN_PROGRESS, progress);
    }
    public void resetResultSkinProgress()
    {
        Storage.SetInt(DataDef.RESULT_SKIN_PROGRESS, 0);
    }
    public int getResultSkinProgressFree()
    {
        int times = Storage.GetInt(DataDef.RESULT_SKIN_PROGRESS_FREE);
        if (times == 0) times = 3;
        return times;
    }
    public void addResultSkinProgressFree(int val)
    {
        int times = this.getResultSkinProgressFree();
        times += val;
        Debug.Log("结算50进度次数=" + times);
        Storage.SetInt(DataDef.RESULT_SKIN_PROGRESS_FREE, times);
    }
    ////////////////////////////////背景
    public int getCurBgIndex()
    {
        return Storage.GetInt(DataDef.BG);
    }

    public void setCurBg(int idx)
    {
        Storage.SetInt(DataDef.BG, idx);
        mvc.send(Notification.UpdateBg, idx);
    }

    public int[] getBgArr()
    {
        int[] bgList = Storage.GetIntArray(DataDef.BG_TIMES_LIST);
        if (bgList.Length == 0)
        {
            bgList = new int[MAX_BG_COUNT];
        }
        return bgList;
    }

    public void addBgTimes(int idx, int time)
    {
        int[] bgList = this.getBgArr();
        bgList[idx] += time;
        Storage.SetIntArray(DataDef.BG_TIMES_LIST, bgList);
        int totalTimes = ConfigManager.inst.getShopConfig().bg[idx].times;
        if (bgList[idx] >= totalTimes)
        {
            this.addBgHave(idx);
        }
        mvc.send(Notification.UpdateBgTimes, idx);
    }

    public int[] getBgHaveList()
    {
        int[] bgHaveList = Storage.GetIntArray(DataDef.BG_HAVE_LIST);
        if (bgHaveList.Length == 0)
        {
            bgHaveList = new int[MAX_BG_COUNT] { 1, 0, 0, 0, 0, 0, 0, 0, 0 };
        }
        return bgHaveList;
    }
    public void addBgHave(int val)
    {
        int[] bgHaveList = this.getBgHaveList();
        bgHaveList[val] = 1;
        Storage.SetIntArray(DataDef.BG_HAVE_LIST, bgHaveList);
        mvc.send(Notification.AddBg, val);
    }

    public int getBgTimes(int index)
    {
        int[] bgTimesArr = Storage.GetIntArray(DataDef.BG_TIMES_LIST);
        if (bgTimesArr.Length == 0)
        {
            bgTimesArr = new int[MAX_BG_COUNT];
        }
        return bgTimesArr[index];
    }
    public int getResultBgIndex()
    {
        int index = Storage.GetInt(DataDef.RESULT_BG_INDEX);
        if (index == 0)
        {
            index = this.resetResultBgIndex();
        }
        return index;
    }
    public int resetResultBgIndex()
    {
        this.resetResultBgProgress();
        CBg[] bgArr = ConfigManager.inst.getShopConfig().bg;
        int[] noHaveArr = this.getNoHaveBgArr();
        if (noHaveArr.Length > 0)
        {
            int idx = Util.getRandom(0, noHaveArr.Length);
            // Debug.Log("随机皮肤=idx="+idx+"t="+bgArr.Length);
            Storage.SetInt(DataDef.RESULT_BG_INDEX, noHaveArr[idx]);
            return noHaveArr[idx];
        }
        return -1;
    }
    public int[] getNoHaveBgArr()
    {
        List<int> list = new List<int>();
        // CBg[] bgArr = ConfigManager.inst.getShopConfig().bg;
        int[] haveArr = SkinManager.inst.getBgHaveList();
        for (int i = 0; i < MAX_BG_COUNT; i++)
        {
            if (haveArr[i] == 0)
                list.Add(i);
        }
        return list.ToArray();
    }
    public int getResultBgProgress()
    {
        return Storage.GetInt(DataDef.RESULT_BG_PROGRESS);
    }
    public void addResultBgProgress(int val)
    {
        int progress = this.getResultBgProgress();
        progress += val;
        Storage.SetInt(DataDef.RESULT_BG_PROGRESS, progress);
    }
    public void resetResultBgProgress()
    {
        Storage.SetInt(DataDef.RESULT_BG_PROGRESS, 0);
    }
    public int getResultBgProgressFree()
    {
        int times = Storage.GetInt(DataDef.RESULT_BG_PROGRESS_FREE);
        if (times == 0) times = 3;
        return times;
    }
    public void addResultBgProgressFree(int val)
    {
        int times = this.getResultBgProgressFree();
        times += val;
        Storage.SetInt(DataDef.RESULT_BG_PROGRESS_FREE, times);
    }

    public int getResultItemType()
    {
        var index = Storage.GetInt(DataDef.RESULT_SKIN_OR_BG);
        return index;
    }
    public void resetResultItemType()
    {
        this.resetResultBgProgress();
        var type = this.getResultItemType();
        if(type == -1) return;
        type = type == 0 ? 1 : 0;
        int index;
        if (type == ItemType.SKIN)
        {
            index = this.resetResultSkinIndex();
        }
        else
            index = this.resetResultBgIndex();
        if (index == -1)
        {
            type = type == 0 ? 1 : 0;
            if (type == ItemType.SKIN)
            {
                index = this.resetResultSkinIndex();
            }
            else
                index = this.resetResultBgIndex();
            if (index == -1)
            {
                type = -1;
                Storage.SetInt(DataDef.RESULT_SKIN_OR_BG, type);
                return;
            }
        }
        Debug.Log("重置进度=====" + index);
        Storage.SetInt(DataDef.RESULT_SKIN_OR_BG, type);
    }
}
