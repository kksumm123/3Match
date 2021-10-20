using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BannerAdExample : MonoBehaviour
{
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOsAdUnitId = "Banner_iOS";
    string _adUnitId;

    void Start()
    {
        _androidAdUnitId = "Banner_Android";
        _iOsAdUnitId = "Banner_iOS";
        //_adUnitId 선언만 하고 할당해주는 코드가 없었다 !
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        Debug.Log("배너 광고 옵션 설정");
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Debug.Log($"배너 광고 로드 _adUnitId : {_adUnitId}\noptions : {options}");
        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitId, options);
        Debug.Log("배너 광고 로드 명령어 실행");
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
    }

    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }

    // Implement a method to call when the Show Banner button is clicked:
    public void ShowBannerAd()
    {
        Debug.Log("ShowBannerAd() 실행");
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_adUnitId, options);
        Debug.Log("Advertisement.Banner.Show 명령어 실행");
    }

    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() => Debug.Log("배너 클릭함");
    void OnBannerShown() => Debug.Log("배너 보여줬음");
    void OnBannerHidden() => Debug.Log("배너 숨김");

    void OnDestroy()
    {
        // Clean up the listeners:
    }
}