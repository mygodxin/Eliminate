using UnityEngine;
public class AdManager
{
    private readonly string _bannerAdUnit = "0ac59b0996d947309c33f59d6676399f";
    private readonly string _interstitialAdUnit = "4f117153f5c24fa6a3a92b818a5eb630";
    private readonly string _rewardedAdUnit = "8f000bd5e00246de9c789eed39ff6096";
    public delegate void VideoCallback(bool isEnd);
    VideoCallback videoCallback;
    public static AdManager inst = new AdManager();

    public void Init()
    {
        this.addEvent();
        this.LoadAd();
    }

    void addEvent()
    {
        MoPubManager.OnAdLoadedEvent += OnAdLoadedEvent;
        MoPubManager.OnAdFailedEvent += OnAdFailedEvent;

        MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
        MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;

        MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
        MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
        MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
        MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;

#if mopub_native_beta
        MoPubManager.OnNativeLoadEvent += OnNativeLoadEvent;
        MoPubManager.OnNativeFailEvent += OnNativeFailEvent;
#endif

        MoPubManager.OnImpressionTrackedEvent += OnImpressionTrackedEvent;
        MoPubManager.OnImpressionTrackedEventBg += OnImpressionTrackedEventBg;
    }
    void LoadAd()
    {
        MoPub.LoadBannerPluginsForAdUnits(new string[] { _bannerAdUnit });
        MoPub.LoadInterstitialPluginsForAdUnits(new string[] { _interstitialAdUnit });
        MoPub.LoadRewardedVideoPluginsForAdUnits(new string[] { _rewardedAdUnit });
    }
    private void AdFailed(string adUnitId, string action, string error)
    {
        var errorMsg = "Failed to " + action + " ad unit " + adUnitId;
        if (!string.IsNullOrEmpty(error))
            errorMsg += ": " + error;
    }
    // Banner Events
    private void OnAdLoadedEvent(string adUnitId, float height)
    {
    }
    private void OnAdFailedEvent(string adUnitId, string error)
    {
    }
    // Interstitial Events
    private void OnInterstitialLoadedEvent(string adUnitId)
    {
    }
    private void OnInterstitialFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load interstitial", error);
    }
    private void OnInterstitialDismissedEvent(string adUnitId)
    {
    }
    // Rewarded Video Events
    private void OnRewardedVideoLoadedEvent(string adUnitId)
    {
        // var availableRewards = MoPub.GetAvailableRewards(adUnitId);
        // _demoGUI.AdLoaded(adUnitId);
        // _demoGUI.LoadAvailableRewards(adUnitId, availableRewards);
        MoPub.ShowRewardedVideo(this._rewardedAdUnit);
    }
    private void OnRewardedVideoFailedEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "load rewarded video", error);
    }
    private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
    {
        AdFailed(adUnitId, "play rewarded video", error);
    }
    private void OnRewardedVideoClosedEvent(string adUnitId)
    {
        // _demoGUI.AdDismissed(adUnitId);
        this.videoCallback?.Invoke(true);
    }
    private void OnImpressionTrackedEvent(string adUnitId, MoPub.ImpressionData impressionData)
    {
        // _demoGUI.ImpressionTracked(adUnitId, impressionData);
    }
    private void OnImpressionTrackedEventBg(string adUnitId, MoPub.ImpressionData impressionData)
    {
        // MoPubDemoGUI.ImpressionTrackedBg(adUnitId, impressionData);
    }
    public void CreateVideoAd(VideoCallback callback)
    {
        this.videoCallback = callback;
        this.videoCallback?.Invoke(true);
        // if (!MoPub.HasRewardedVideo(this._rewardedAdUnit))
        // {
        //     MoPub.RequestRewardedVideo(this._rewardedAdUnit);
        // }
        // else
        // {
        //     Debug.Log("播放视频广告");
        //     MoPub.ShowRewardedVideo(this._rewardedAdUnit);
        // }
    }
    public void CreateBannerAd()
    {
        MoPub.ShowBanner(_bannerAdUnit, true);
    }
    public void HideBannerAd()
    {
        MoPub.ShowBanner(_bannerAdUnit, false);
    }
    public void CreateInterstitialAd()
    {
        if (!MoPub.HasRewardedVideo(this._rewardedAdUnit))
        {
            MoPub.RequestRewardedVideo(this._rewardedAdUnit);
        }
        else
        {
            MoPub.ShowInterstitialAd(_bannerAdUnit);
        }
    }
    public void HideInterstitialAd()
    {
        MoPub.DestroyInterstitialAd(_bannerAdUnit);
    }
}