using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdmobManager : Singleton<AdmobManager>
{
    public bool isTestMode;

    void Start()
    {
        var requestConfiguration = new RequestConfiguration
           .Builder()
           .SetTestDeviceIds(new List<string>() { "DA55FBA1AEDF47FA" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        LoadBannerAd(); // 시작 시 배너광고 로드

        SceneManager.sceneLoaded += ADLoaded;
    }

    void ADLoaded(Scene scene, LoadSceneMode mode)
    {
        DestroyBannerAd();
        DestroyFrontAd();

        LoadBannerAd();
        LoadFrontAd();
    }
    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    #region 배너 광고
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    const string bannerID = "ca-app-pub-9546715742178824/4057281619";
    BannerView bannerAd;

    void LoadBannerAd()
    {
        AdSize adaptiveSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID,
            adaptiveSize, AdPosition.Top);
        bannerAd.LoadAd(GetAdRequest());

        if (SceneManager.GetActiveScene().buildIndex == 1)
            ToggleBannerAd(false);
    }

    public void ToggleBannerAd(bool b)
    {
        if (b) bannerAd.Show();
        else
        {
            if (bannerAd != null)
            {
                bannerAd.Hide();
            }
        }
    }

    void DestroyBannerAd()
    {
        if (bannerAd != null)
        {
            bannerAd.Destroy();
            bannerAd = null;
        }
    }
    #endregion
    #region 전면 광고
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    const string frontID = "ca-app-pub-9546715742178824/2541019960";
    [HideInInspector] public InterstitialAd frontAd;


    void LoadFrontAd()
    {
        InterstitialAd.Load(isTestMode ? frontTestID : frontID, GetAdRequest(),
            (InterstitialAd ad, LoadAdError error) => {
                if (error != null || ad == null)
                {
                    Debug.LogWarning("광고 로드 실패: " + error);
                    frontAd = null; // 광고 로드 실패 시 null로 설정
                }
                else
                {
                    frontAd = ad;
                    frontAd.OnAdFullScreenContentClosed += () => DestroyFrontAd(); // 광고 닫힌 후 정리
                    Debug.Log("광고 로드 성공");
                }
            });
    }

    public void ShowFrontAd(int sceneID)
    {
        if (frontAd != null)
        {
            frontAd.OnAdFullScreenContentClosed += () => SceneManager.LoadScene(sceneID);
            frontAd.Show();
        }
        else
        {
            Debug.Log("광고가 로드되지 않았으므로 즉시 씬 이동");
            SceneManager.LoadScene(sceneID); // 광고 없이 바로 씬 이동
        }
    }

    void DestroyFrontAd()
    {
        if (frontAd != null)
        {
            frontAd.Destroy();
            frontAd = null;
        }
    }
    #endregion
}
