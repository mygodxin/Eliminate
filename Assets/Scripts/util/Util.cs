using UnityEngine;
using System;
using FairyGUI;
using System.Collections.Generic;
public class Util
{
    static Stack<GLabel> tipPool = new Stack<GLabel>();
    public static void AppTip(string content)
    {
        GLabel comp;
        if (Util.tipPool.Count > 0)
        {
            comp = tipPool.Pop();
        }
        else
        {
            comp = UIPackage.CreateObject("common", "Tip").asLabel;
        }
        comp.title = content;
        GRoot.inst.AddChild(comp);
        comp.Center();
        comp.GetTransition("show").Play();
        Timers.inst.Add(2, 1, (object obj) =>
         {
             GRoot.inst.RemoveChild(comp);
             tipPool.Push(comp);
         });
    }
    public static void AppAlert(string content, mm.EAlertType alertType = mm.EAlertType.DOUBLE, Action leftCallback = null, Action rightCallBack = null)
    {
        if (null == mm.AlertWindow.inst)
        {
            mm.AlertWindow.inst = new mm.AlertWindow("Alert", "common");
            // mm.AlertWindow.inst.showAnimation = appWindowEase.backShow;
            // k7.AlertWindow.inst.hideAnimation = appWindowEase.backHide;
        }
        // if (typeof param == 'number')
        // {
        //     switch (param)
        //     {
        //         case AlertType.KONW:
        //             param = { type: 1, noClose: true, textL: '知道了' };
        //             break;
        //         case AlertType.OK:
        //             param = { type: 1, noClose: true, textL: '好' };
        //             break;
        //         default: param = { };
        //     }
        // }
        // return 
        mm.AlertWindow.inst.SetAndShow(content, alertType, leftCallback, rightCallBack);
    }
    public static int getRandom(int start, int end)
    {
        return UnityEngine.Random.Range(start, end);
    }
    /// <summary>
    /// 生成不重复的随机数
    /// </summary>
    /// <returns></returns>
    public static List<int> getRandomWithoutRep(int start, int end, int count)
    {
        if (end - start < count) return null;

        List<int> list = new List<int>();
        while (list.Count < count)
        {
            int random = Util.getRandom(start, end);
            if (list.IndexOf(random) < 0)
            {
                list.Add(random);
            }
        }
        return list;
    }

    /// <summary>
    /// 在指定时间过后执行指定的表达式
    /// </summary>
    /// <param name="interval">事件之间经过的时间（以毫秒为单位）</param>
    /// <param name="action">要执行的表达式</param>
    public static void SetTimeout(float interval, Action action)
    {
        //     System.Timers.Timer timer = new System.Timers.Timer(interval);
        //     timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
        //    {
        //        timer.Enabled = false;
        //        action();
        //    };
        //     timer.Enabled = true;
        // TweenManager.CreateTween().SetDelay(interval / 1000).OnComplete(() =>
        // {
        //     action();
        // });
    }
    /// <summary>
    /// 在指定时间周期重复执行指定的表达式
    /// </summary>
    /// <param name="interval">事件之间经过的时间（以毫秒为单位）</param>
    /// <param name="action">要执行的表达式</param>
    public static void SetInterval(double interval, Action<System.Timers.ElapsedEventArgs> action)
    {
        System.Timers.Timer timer = new System.Timers.Timer(interval);
        timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
    {
        action(e);
    };
        timer.Enabled = true;
    }


    public static double difSeconds(DateTime startTime, DateTime endTime)
    {
        TimeSpan secondSpand = new TimeSpan(endTime.Ticks - startTime.Ticks);
        return secondSpand.TotalSeconds;
    }
}