using mm;
using FairyGUI;
using UnityEngine;

public class LoadingScene : AppScene
{
    public static string NAME = "LoadingScene";

    GLoader3D _logo;
    GImage _imgBall;
    GProgressBar _bar;
    public LoadingScene() : base("LoadingScene", "loading")
    {
    }
    override public void BindChild()
    {
        this._imgBall = this.getImage("bar.qiu");
        this._bar = this.GetProgressBar("bar");
        this._logo = this.GetLoader3D("logo");
        this._logo.animationName = "LOGO";
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

        ConfigManager.inst.loadConfig(this.onConfigLoadComplete);
        this._bar.value = 0;
        this._bar.TweenValue(100, 1)
        .OnUpdate((tween) =>
        {
            // Debug.Log("进度=" + tween.value.x);
            this._imgBall.rotation = (float)tween.value.x * 7;
        })
        .OnComplete((tween) =>
        {
            if (tween.value.x == 100)
            {
                mm.AppScene.show(StartScene.NAME);
            }
        });
        // GTween.To(0, 720, 5)
        // .SetEase(EaseType.Linear)
        // .SetTarget(this._imgBall,TweenPropType.Rotation)
        // .OnComplete();
    }

    void onConfigLoadComplete() {

    }

    void updateBar()
    {
    }
    public override void OnClickButton(GButton button)
    {
        // if (button == this._startBtn)
        // {
        //     mm.AppScene.show(PKWin.NAME);
        // }
    }

    void ClickSkip()
    {
        
    }

    void ClickAd()
    {

    }
}