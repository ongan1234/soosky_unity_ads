using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance = null;

    public Action OnHaveReward;
    public Action OnNotHaveReward;
    public Action<TYPE_ADS, string> OnCloseAds;
    public Action<TYPE_ADS, STATUS_ADS, string> OnStatusLoadAds;

    public void statusLoadAds(TYPE_ADS type_ads, STATUS_ADS status_ads, string key_pos) { if(OnStatusLoadAds != null) OnStatusLoadAds.Invoke(type_ads, status_ads, key_pos); }
    public void haveReward() { if(OnHaveReward != null) OnHaveReward.Invoke(); }
    public void notHaveReward() { if(OnNotHaveReward != null) OnNotHaveReward.Invoke(); }
    public void closeAds(TYPE_ADS type_ads, string key_pos)
    {
        if(_LoadingAds != null) Destroy(_LoadingAds);
        if(OnCloseAds!= null) OnCloseAds.Invoke(type_ads, key_pos);
    }

    public GameObject LoadingAds;
    private GameObject _LoadingAds;

    public int countTimeSecond = 3;
    public int rewardCoin = 0;

    private bool isInitialized = false;
    public bool IsInitialized(){
        return isInitialized;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            isInitialized = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyBanner()
    {
        AdmobBannerAd.Instance?.DestroyAd();
    }

    public void ShowBanner()
    {
        AdmobBannerAd.Instance?.ShowBanner();
    }

    public void LoadRewardedAd()
    {
        AdmobRewardAd.Instance.loadAds();
    }

    public void ShowRewardedAd(string key_pos, int bonus_coin)
    {
        this.rewardCoin = bonus_coin;
        ShowPopupLoadAds(TYPE_ADS.RewardAd, key_pos);
    }

    public void CountInterstitialAd(int startAds, int loopAds, String keyCount)
    {
        AdmobInterstitialAd.Instance.countToShowAds(startAds, loopAds, keyCount);
    }

    public void LoadInterstitialAd()
    {
        AdmobInterstitialAd.Instance.loadAds();
    }

    public void ShowInterstitialAd(string key_pos)
    {
        ShowPopupLoadAds(TYPE_ADS.InterstitialAd, key_pos);
    }

    public void LoadAppOpenAd()
    {
        AdmobOpenAd.Instance.loadAds();
    }

    public void ShowAppOpenAd(string key_pos)
    {
        AdmobOpenAd.Instance.ShowAds(key_pos);
    }

    public void ShowPopupLoadAds(TYPE_ADS type_ads, string key_pos)
    {
        _LoadingAds = Instantiate(LoadingAds, Vector3.zero, Quaternion.identity);
        if (type_ads == TYPE_ADS.InterstitialAd)
        {
            LoadInterstitialAd();
        }
        else if (type_ads == TYPE_ADS.RewardAd)
        {
            LoadRewardedAd();
        }
        else
        {
            LoadAppOpenAd();
        }

        StartCoroutine(CountdownToShowAds(type_ads, key_pos));
    }

    void ShowAds(TYPE_ADS type_ads, string key_pos)
    {
        if (type_ads == TYPE_ADS.InterstitialAd)
        {
            AdmobInterstitialAd.Instance.ShowAds(key_pos);
        }
        else if (type_ads == TYPE_ADS.RewardAd)
        {
            AdmobRewardAd.Instance.ShowAds(key_pos);
        }
        else
        {
            AdmobOpenAd.Instance.ShowAds(key_pos);
        }
    }

    IEnumerator CountdownToShowAds(TYPE_ADS type_ads, string key_pos)
    {
        int counter = countTimeSecond;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        ShowAds(type_ads, key_pos);
    }
}
