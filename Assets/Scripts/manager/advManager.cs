// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using UnityEngine;

// public class advManager
// {
//     public static advManager Instance = new advManager();
// #if DEBUG
//     /// <summary>
//     /// 横幅测试广告
//     /// </summary>
//     public static readonly string BannerAdvId = "b195f8dd8ded45fe847ad89ed1d016da";
//     /// <summary>
//     ///插屏测试广告
//     /// </summary>
//     public static readonly string InstersititalAdvId = "24534e1901884e398f1253216226017e";
//     /// <summary>
//     /// 视频测试广告
//     /// </summary>
//     public static readonly string RewardedAdvId = "920b6145fb1546cf8b5cf2ac34638bb7";
// #else
//      /// <summary>
//         /// 横幅测试广告
//         /// </summary>
//         public static readonly string BannerAdvId = "b195f8dd8ded45fe847ad89ed1d016da";
//         /// <summary>
//         ///插屏测试广告
//         /// </summary>
//         public static readonly string InstersititalAdvId = "24534e1901884e398f1253216226017e";
//         /// <summary>
//         /// 视频测试广告
//         /// </summary>
//         public static readonly string RewardedAdvId = "920b6145fb1546cf8b5cf2ac34638bb7";
// #endif
//     /// <summary>
//     /// 广告状态
//     /// </summary>
//     private Dictionary<string, EAdvState> advsState;//= new Dictionary<string, EAdvState>();
//     public enum EAdvType
//     {
//         Test,
//     }
//     /// <summary>
//     /// 广告类型
//     /// </summary>
//     private EAdvType eAdvType;

//     enum EAdvState
//     {
//         UnLoad,
//         Loading,
//         LoadFailed,
//         LoadSuccess,
//     }

//     public void init()
//     {
//         advsState = new Dictionary<string, EAdvState>();

//         //视频加载相关
//         MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
//         MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
//         MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
//         MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
//         MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
//         MoPubManager.Instance.Initialized = new MoPubManager.InitializedEvent();
//         MoPubManager.Instance.Initialized.AddListener(Initialized);
//         MoPub.LoadRewardedVideoPluginsForAdUnits(new string[] { RewardedAdvId });
//     }

//     public void desPosed()
//     {
//         MoPubManager.OnRewardedVideoLoadedEvent -= OnRewardedVideoLoadedEvent;
//         MoPubManager.OnRewardedVideoFailedEvent -= OnRewardedVideoFailedEvent;
//         MoPubManager.OnRewardedVideoFailedToPlayEvent -= OnRewardedVideoFailedToPlayEvent;
//         MoPubManager.OnRewardedVideoClosedEvent -= OnRewardedVideoClosedEvent;
//         MoPubManager.OnRewardedVideoReceivedRewardEvent -= OnRewardedVideoReceivedRewardEvent;
//         advsState = null;
//     }

//     private void Initialized(string id)
//     {
//         loadRewardeId();
//     }
//     public void loadRewardeId()
//     {
//         EAdvState eAdvState;
//         if (!advsState.TryGetValue(RewardedAdvId, out eAdvState) || eAdvState == EAdvState.LoadFailed || eAdvState == EAdvState.UnLoad)
//         {
//             advsState[RewardedAdvId] = EAdvState.Loading;
//             MoPub.RequestRewardedVideo(RewardedAdvId);
//         }
//     }

//     /// <summary>
//     /// 播放成功
//     /// </summary>
//     /// <param name="type"></param>
//     public void playVideo(EAdvType type)
//     {
//         if (!MoPub.HasRewardedVideo(RewardedAdvId))
//         {
//             Debug.Log("advManager :" + RewardedAdvId + ",Incomplete load");
//             loadRewardeId();
//             return;
//         }
//         eAdvType = type;
//         MoPub.ShowRewardedVideo(RewardedAdvId);
//     }

//     /// <summary>
//     /// 播放成功
//     /// </summary>
//     private void playSUccess()
//     {
//         if (eAdvType == EAdvType.Test)
//         {

//         }
//     }
//     /// <summary>
//     /// 视频加载成功
//     /// </summary>
//     /// <param name="obj"></param>
//     private void OnRewardedVideoLoadedEvent(string advId)
//     {
//         Debug.Log("advManager OnRewardedVideoLoadedEvent :" + advId);
//         advsState[advId] = EAdvState.LoadSuccess;
//     }
//     private void OnRewardedVideoFailedEvent(string advId, string error)
//     {
//         Debug.Log("advManager OnRewardedVideoFailedEvent :" + advId + ",error :" + error);
//         advsState[advId] = EAdvState.LoadFailed;
//     }

//     private void OnRewardedVideoFailedToPlayEvent(string advId, string error)
//     {
//         Debug.Log("advManager OnRewardedVideoFailedToPlayEvent :" + advId + ",error :" + error);
//     }

//     private void OnRewardedVideoClosedEvent(string advId)
//     {
//         Debug.Log("advManager OnRewardedVideoClosedEvent :" + advId);
//         advsState[advId] = EAdvState.UnLoad;
//     }

//     private void OnRewardedVideoReceivedRewardEvent(string arg1, string arg2, float arg3)
//     {
//         Debug.Log("advManager OnRewardedVideoReceivedRewardEvent:" + arg1 + "," + arg2 + "," + arg3);
//         playSUccess();
//     }
// }
