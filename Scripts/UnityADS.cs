using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class UnityADS : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public string AndroidID;
    public string IosID;
    public bool testMode;
    public int GecisReklamiBolum;

    string InterstitialID = "Interstitial_Android";
    string RewardedID = "Rewarded_Android";
    string platformID;

    [Header("Rewarded Wait Time")]
    public TextMeshProUGUI AdTimeText;
    public int reklamBekelmeDK;
    float zaman, zaman2;
    int forAdTime;
    bool canWacthAd;


    void Awake()
    {
#if UNITY_ANDROID
        platformID = AndroidID;
#else
        platformID = IosID;
#endif

        AdTimeText.DOFade(0, 0f);
        //PlayerPrefs.SetInt("hour", 0);
        //PlayerPrefs.SetInt("minute", 0);
        //PlayerPrefs.SetInt("second", 0);
        forAdTime = PlayerPrefs.GetInt("hour") * 3600 + PlayerPrefs.GetInt("minute") * 60 + PlayerPrefs.GetInt("second");
        canWacthAd = false;
    }
    private void Update()
    {
        if (Time.time > zaman && !Advertisement.isInitialized && IntConnection.isConnected)
        {
            zaman = Time.time + 1f;
            Advertisement.Initialize(platformID, testMode, this);
        }

        if (Time.time > zaman2)
        {
            zaman2 = Time.time + 1;
            int geriSayim = 0;
            int saniyeCinsindenFark;
            bool geriSayimVarmi = false;
            saniyeCinsindenFark = (DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second) - forAdTime;

            if (saniyeCinsindenFark > reklamBekelmeDK * 60)
            {
                //saniye cinsinden aralarýnda fark bekleme süresinden fazlamý
                AdTimeText.text = "00:00";
                canWacthAd = true;
            }
            else
            {
                geriSayimVarmi = true;
                geriSayim = reklamBekelmeDK * 60 - saniyeCinsindenFark;
            }

            if (geriSayimVarmi && geriSayim > 0)
            {
                canWacthAd = false;
                if (geriSayim > 60)
                {
                    int a = geriSayim / 60;
                    AdTimeText.text = a + ":" + (geriSayim - a * 60);
                }
                else
                {
                    if (geriSayim < 10)
                        AdTimeText.text = "00:0" + geriSayim;
                    else
                        AdTimeText.text = "00:" + geriSayim;
                }
            }
            else
            {
                canWacthAd = true;
            }
        }
    }

    public void ShowRewardedAD()
    {
        if ((PlayerPrefs.GetInt("day") != DateTime.Now.Day | canWacthAd) && Advertisement.isInitialized)
        {
            Advertisement.Show(RewardedID, this);

            canWacthAd = false;
            PlayerPrefs.SetInt("hour", DateTime.Now.Hour);
            PlayerPrefs.SetInt("minute", DateTime.Now.Minute);
            PlayerPrefs.SetInt("second", DateTime.Now.Second);
            PlayerPrefs.SetInt("day", DateTime.Now.Day);
            forAdTime = PlayerPrefs.GetInt("hour") * 3600 + PlayerPrefs.GetInt("minute") * 60 + PlayerPrefs.GetInt("second");
        }
        else
        {
            AdTimeText.DOKill();
            AdTimeText.DOFade(1, 0.6f);
            AdTimeText.DOFade(0, 1f).SetDelay(3f);
        }
    }
    public void ShowInterstitial()
    {
        Advertisement.Show(InterstitialID, this);
    }


    public void OnInitializationComplete()
    {
        print("Reklam sistemi baþlatýldý.");

        Advertisement.Load(InterstitialID, this);
        Advertisement.Load(RewardedID, this);
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        print($"Reklam sistemi baþlatýlamadý: {error} - {message}");
    }


    public void OnUnityAdsAdLoaded(string placementId)
    {
        print("Reklam yüklendi.");
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        print("Reklam yüklenirken hata oluþtu.");

        if (placementId == InterstitialID)
            SceneManager.LoadScene((int.Parse(SceneManager.GetActiveScene().name) + 1).ToString());
    }


    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        print("Reklam gösterilirken hata oluþtu.");
        Advertisement.Initialize(platformID, testMode, this);
    }
    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        print("Reklam baþarýyla gösterildi.");      //reklamlar gösterildikten sonra tekrar çaðýrýyoruz yoksa bir sonraki reklam show fonksiyonu çalýþmýyor

        if (placementId == InterstitialID && int.Parse(SceneManager.GetActiveScene().name) % GecisReklamiBolum == 0)  
        {
            SceneManager.LoadScene((int.Parse(SceneManager.GetActiveScene().name) + 1).ToString());
            Advertisement.Load(InterstitialID, this);
        }
        else if (placementId == RewardedID)
        {
            GetComponent<InLevelBtn>().GiveRewardForADS(1);
            Advertisement.Load(RewardedID, this);
        }
    }
}