using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    void Awake() => Instance = this;

    InterstitialAdExample interstitialAdExample;
    BannerAdExample bannerAdExample;
    IEnumerator Start()
    {
        yield return null; // 광고.cs에서 start() 실행할 수 있도록 1프레임 return

        interstitialAdExample = GetComponent<InterstitialAdExample>();
        bannerAdExample = GetComponent<BannerAdExample>();

        while (!Advertisement.isInitialized)
            yield return null;

        interstitialAdExample.LoadAd();
        bannerAdExample.LoadBanner();

        //while (!Advertisement.Banner.isLoaded)
        //    yield return null;
        bannerAdExample.ShowBannerAd();

        interstitialAdExample.onUnityAdsShowComplete = (unityAdsShowCompletionState) =>
        {
            GameOverUI.Instance.ReloadScene();
        };
    }

    public void ShowAds()
    {
        bannerAdExample.HideBannerAd();
        interstitialAdExample.ShowAd();
    }
}
