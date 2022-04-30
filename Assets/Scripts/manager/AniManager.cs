using FairyGUI;
using System;

public class EEmType
{
    public static string SLEEP = "A";
    public static string WAKE = "B";
}
public class EFingerType
{
    public static string UP = "UP";
    public static string DOWN = "D";
    public static string LEFT = "L";
    public static string RIGHT = "R";
}
// A是出场动画
// B是选中动画
// C-D是向下移动时的动画，移动时间0.5秒，
// C-up是向上移动
// C-L是向左移动
// C-R是向右移动
// D是消除前
public enum EAniType
{
    SHOW,
    SEL,
    MOVE_LEFT,
    MOVE_RIGHT,
    MOVE_UP,
    MOVE_DOWN,
    MOVE_LEFT2,
    MOVE_RIGHT2,
    MOVE_UP2,
    MOVE_DOWN2,
    REMOVE_BEFORE,
    YE_TI,
    YE_TI2,
    YE_TI3,
    YE_TI4,
    YE_TI5,
    BING_KUAI,
    BING_KUAI2,
    BING_KUAI3,
    BING_KUAI4,
    BING_KUAI5,
    Default,
    BaoShi,
    BaoShi2,
    BaoShi3
}
/// <summary>
/// 动画管理类
/// </summary>
public class AniManager
{
    public static AniManager inst = new AniManager();

    public string getAniName(EAniType aniType, int color)
    {
        // A是出场动画
        // B是选中动画
        // C-D是向下移动时的动画，移动时间0.5秒，
        // C-up是向上移动
        // C-L是向左移动
        // C-R是向右移动
        // D是消除前
        int index = color + 1;
        switch (aniType)
        {
            case EAniType.Default:
                return "E" + index;
            case EAniType.SHOW:
                return "A" + index;
            case EAniType.SEL:
                return "B" + index;
            case EAniType.MOVE_UP:
                return "C" + index + "-UP";
            case EAniType.MOVE_DOWN:
                return "C" + index + "-D";
            case EAniType.MOVE_LEFT:
                return "C" + index + "-L";
            case EAniType.MOVE_RIGHT:
                return "C" + index + "-R";
            case EAniType.MOVE_UP2:
                return "C" + index + "-UP2";
            case EAniType.MOVE_DOWN2:
                return "C" + index + "-D2";
            case EAniType.MOVE_LEFT2:
                return "C" + index + "-L2";
            case EAniType.MOVE_RIGHT2:
                return "C" + index + "-R2";
            case EAniType.YE_TI:
                return "YeTi";
            case EAniType.YE_TI2:
                return "YeTi2";
            case EAniType.YE_TI3:
                return "YeTi3";
            case EAniType.YE_TI4:
                return "YeTi4";
            case EAniType.YE_TI5:
                return "YeTi5";
            case EAniType.BING_KUAI:
                return "bingkuai";
            case EAniType.BING_KUAI2:
                return "bingkuai2";
            case EAniType.BING_KUAI3:
                return "bingkuai3";
            case EAniType.BING_KUAI4:
                return "bingkuai4";
            case EAniType.BING_KUAI5:
                return "bingkuai5";
            case EAniType.REMOVE_BEFORE:
                return "D" + index;
            case EAniType.BaoShi:
                return "BaoShi";
            case EAniType.BaoShi2:
                return "BaoShi2";
            case EAniType.BaoShi3:
                return "BaoShi3";
        }
        return null;
    }

    public void playAni(GComponent component, EAniType aniType, Action complete, bool loop = true)
    {
        // var component = this._comp;
        var icon3D = component.GetChild("icon3D").asLoader3D;
        if (component.data == null) return;
        int color = (int)component.data;//GameCommond.inst.getGridByIndex(index);

        var animationName = AniManager.inst.getAniName(aniType, color);

        // icon3D.spineAnimation.state.ClearTrack(0);
        if (icon3D.animationName != animationName)
        {
            icon3D.spineAnimation.skeleton.SetToSetupPose();
            icon3D.spineAnimation.ClearState();
            icon3D.animationName = animationName;
            // icon3D.spineAnimation.skeleton.Update(0);
            icon3D.spineAnimation.Update(0);
        }
        if (icon3D.loop != loop)
            icon3D.loop = loop;

        if (complete != null)
        {
            Spine.AnimationState.TrackEntryDelegate boomComplete = null;
            boomComplete = delegate (Spine.TrackEntry trackEntry)
                    {
                        complete?.Invoke();
                        icon3D.spineAnimation.state.Complete -= boomComplete;
                    };
            icon3D.spineAnimation.state.Complete += boomComplete;
        }
    }

    public EAniType getYetiEAniType(Grid grid)
    {
        if (grid.elimTimes == 2)
        {
            return EAniType.YE_TI2;
        }
        else if (grid.elimTimes == 1)
        {
            return EAniType.YE_TI4;
        }
        else
        {
            return EAniType.YE_TI4;
        }
    }
    public EAniType getIcecubeEAniType(Grid grid)
    {
        if (grid.elimTimes == 2)
        {
            return EAniType.BING_KUAI2;
        }
        else if (grid.elimTimes == 1)
        {
            return EAniType.BING_KUAI4;
        }
        else
        {
            return EAniType.BING_KUAI4;
        }
    }
}