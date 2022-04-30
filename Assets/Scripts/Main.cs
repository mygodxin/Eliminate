using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using mm;
using System;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Storage.SetInt(DataDef.SKIN, 0);
        mvc.init(new UnityEventDispatcher());

        UIConfig.modalLayerColor = new Color(0f, 0f, 0f, 0.7f);

        // MoPubManager.Instance.
        // mvc.
        // advManager.Instance.init();
        // Global.fairyPath = "Assets/Resources/fui/";
        Global.fairyPath = "fui/";
        mvc.on(ASourceLoader.EVT_SourceLoader_CompleteEvent, this.onItemLoaded);
        ResManager.inst.Preload();
        // mvc.on(ASourceLoader.EVT_SourceLoader_CompleteEvent, (System.Object tar) =>
        // {
        //     Debug.Log("下载完成===========");
        //     var foo = tar as ASourceLoader;
        //     Debug.Log("包价值=" + foo.fileName);
        //     if (foo.fileName == "common")
        //     {
        //         mm.AppWindow.show(StartScene.ClassName);
        //     }
        // });
        // UIPackage.AddPackage(Global.fairyPath + "common");
        // UIPackage.AddPackage(Global.fairyPath + "game");
        // GComponent view = UIPackage.CreateObject("game", "GameScene").asCom;
        // var aWindow = new Window();
        // aWindow.contentPane = view;
        // aWindow.Show();
        // GButton btn = aWindow.contentPane.GetChild("backBtn").asButton;
        // btn.onClick.Add(() =>
        // {
        //     Debug.Log("关闭界面");
        //     aWindow.Hide();
        // });
    }
    public void onItemLoaded(object obj)
    {
        var foo = obj as ASourceLoader;
        if (foo.fileName == "start")
        {
            mm.AppWindow.show(LoadingScene.NAME);
            // mm.AppWindow.show(SkinRewardWin.NAME);
            // mm.AppWindow.show(GameScene.NAME);
            // mm.AppWindow.show(GameEndWin.NAME);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
