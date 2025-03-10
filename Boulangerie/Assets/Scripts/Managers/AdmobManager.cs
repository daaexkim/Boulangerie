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

        LoadBannerAd(); // ���� �� ��ʱ��� �ε�

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

    #region ��� ����
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
    #region ���� ����
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    const string frontID = "ca-app-pub-9546715742178824/2541019960";
    [HideInInspector] public InterstitialAd frontAd;


    void LoadFrontAd()
    {
        InterstitialAd.Load(isTestMode ? frontTestID : frontID, GetAdRequest(),
            (InterstitialAd ad, LoadAdError error) => {
                if (error != null || ad == null)
                {
                    Debug.LogWarning("���� �ε� ����: " + error);
                    frontAd = null; // ���� �ε� ���� �� null�� ����
                }
                else
                {
                    frontAd = ad;
                    frontAd.OnAdFullScreenContentClosed += () => DestroyFrontAd(); // ���� ���� �� ����
                    Debug.Log("���� �ε� ����");
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
            Debug.Log("������ �ε���� �ʾ����Ƿ� ��� �� �̵�");
            SceneManager.LoadScene(sceneID); // ���� ���� �ٷ� �� �̵�
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
