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
    void Start()
    {
        interstitialAdExample = GetComponent<InterstitialAdExample>();
        bannerAdExample = GetComponent<BannerAdExample>();

        interstitialAdExample.LoadAd();
        bannerAdExample.LoadBanner();
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
