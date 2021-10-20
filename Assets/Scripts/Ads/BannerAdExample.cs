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
        //_adUnitId ���� �ϰ� �Ҵ����ִ� �ڵ尡 ������ !
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        Debug.Log("��� ���� �ɼ� ����");
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Debug.Log($"��� ���� �ε� _adUnitId : {_adUnitId}\noptions : {options}");
        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitId, options);
        Debug.Log("��� ���� �ε� ��ɾ� ����");
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
        Debug.Log("ShowBannerAd() ����");
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_adUnitId, options);
        Debug.Log("Advertisement.Banner.Show ��ɾ� ����");
    }

    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() => Debug.Log("��� Ŭ����");
    void OnBannerShown() => Debug.Log("��� ��������");
    void OnBannerHidden() => Debug.Log("��� ����");

    void OnDestroy()
    {
        // Clean up the listeners:
    }
}