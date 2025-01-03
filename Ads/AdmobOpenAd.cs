using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobOpenAd
{
    public static AdmobOpenAd Instance = new AdmobOpenAd();

    private AppOpenAd mAppOpenAd;
    private List<string> list_open_ad_unit_id;
    private int countTier = 0;
    private AdManager adManager;
    private string keyPos;
    private DateTime _expireTime;
    private bool hasUsing4Hours = true;
    private bool isLoadedAds = false;

    public AdmobOpenAd()
    {
        list_open_ad_unit_id = AdmobAdId.opendAdsId;
        adManager = AdManager.Instance;
    }

    /// <summary>
    /// Loads the AppOpenAd ad.
    /// </summary>
    public void loadAds()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) { return; }
        if (adManager == null) { return; }
        if (PlayerPrefsManager.hasSUB()) { return; }
        if (mAppOpenAd != null) { return; }
        if (isLoadedAds) return;
       isLoadedAds = true;

        Debug.Log("xxx Loading the AppOpenAd ad.");
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        AppOpenAd.Load(list_open_ad_unit_id[countTier], adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                isLoadedAds = false;
                if (error != null || ad == null)
                {
                    Debug.LogError("xxx AppOpenAd ad failed to load an ad " + "with error : " + error);
                    if (!hasUsing4Hours)
                    {
                        destroyAd();
                    }
                    adManager.statusLoadAds(TYPE_ADS.OpenAd, STATUS_ADS.FAIL, keyPos);
                    return;
                }

                Debug.Log("xxx AppOpenAd ad loaded with response : " + ad.GetResponseInfo());

                if (hasUsing4Hours)
                {
                    _expireTime = DateTime.Now + TimeSpan.FromHours(4);
                }

                adManager.statusLoadAds(TYPE_ADS.OpenAd, STATUS_ADS.SUCCESS, keyPos);
                mAppOpenAd = ad;
                RegisterEventHandlers(mAppOpenAd);
            });

        countTier = (countTier + 1) % list_open_ad_unit_id.Count; // Switch to next ad unit
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"AppOpenAd ad paid {adValue.Value} {adValue.CurrencyCode}.");
        };

        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("AppOpenAd ad recorded an impression.");
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log("AppOpenAd ad was clicked.");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            AdManager.Instance.isShowingAd = true; // Set state to showing
            Debug.Log("AppOpenAd ad full screen content opened.");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            AdManager.Instance.isShowingAd = false; // Reset state
            Debug.Log("xxx AppOpenAd ad full screen content closed.");

            // Don't call loadAds here; only close the ad
            if (!hasUsing4Hours)
            {
                destroyAd();
                loadAds();
            }
            closeAds();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("xxx AppOpenAd ad failed to open full screen content " + "with error : " + error);
            destroyAd();
        };
    }

    private void closeAds()
    {
        if (adManager != null)
        {
            adManager.closeAds(TYPE_ADS.OpenAd, keyPos);
        }
    }

    public void destroyAd()
    {
        if (mAppOpenAd != null)
        {
            mAppOpenAd.Destroy();
            mAppOpenAd = null; // Reset the ad
        }
    }

    public void countToShowAds(int startAds, int loopAds, string keyCount)
    {
        if (adManager == null) return;

        if (PlayerPrefsManager.hasSUB())
        {
            closeAds();
            return;
        }

        int countFullAds = PlayerPrefsManager.getCounterAds(keyCount);
        countFullAds += 1;
        keyPos = keyCount;
        PlayerPrefsManager.saveCounterAds(keyCount, countFullAds);

        bool isShowAds = countFullAds >= startAds && ((countFullAds - startAds) % loopAds == 0);

        if (isShowAds)
        {
            ShowAds(keyCount);
        }
        else
        {
            closeAds();
        }
    }

    public void ShowAds(string key_pos)
    {
        if (adManager == null) return;

        keyPos = key_pos;

        if (PlayerPrefsManager.hasSUB())
        {
            closeAds();
            return;
        }

        if (canShowAds())
        {
            mAppOpenAd.Show();
        }
        else
        {
            closeAds();
            loadAds();
        }
    }

    public bool canShowAds()
    {
        if (hasUsing4Hours)
        {
            return (mAppOpenAd != null && mAppOpenAd.CanShowAd() && DateTime.Now < _expireTime);
        }
        else
        {
            return (mAppOpenAd != null && mAppOpenAd.CanShowAd());
        }
    }
}
