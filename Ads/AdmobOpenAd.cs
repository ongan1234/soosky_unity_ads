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
        Debug.Log("xxx Loading the AppOpenAd ad.");
        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(list_open_ad_unit_id[countTier], adRequest,
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("xxx AppOpenAd ad failed to load an ad " +
                                   "with error : " + error);
                    mAppOpenAd = null;
                    adManager.statusLoadAds(TYPE_ADS.OpenAd, STATUS_ADS.FAIL, keyPos);
                    return;
                }

                Debug.Log("xxx AppOpenAd ad loaded with response : "
                          + ad.GetResponseInfo());
                adManager.statusLoadAds(TYPE_ADS.OpenAd, STATUS_ADS.SUCCESS, keyPos);
                mAppOpenAd = ad;
                RegisterEventHandlers(mAppOpenAd);
            });

        if (countTier >= list_open_ad_unit_id.Count - 1)
        {
            countTier = 0;
        }
        else
        {
            countTier++;
        }
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("AppOpenAd ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("AppOpenAd ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("AppOpenAd ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("AppOpenAd ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("xxx AppOpenAd ad full screen content closed.");

            destroyAd();
            closeAds();
            loadAds();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            mAppOpenAd = null;
            Debug.LogError("xxx AppOpenAd ad failed to open full screen content " +
                           "with error : " + error);
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
        // Clean up the old ad before loading a new one.
        if (mAppOpenAd != null)
        {
            mAppOpenAd.Destroy();
            mAppOpenAd = null;
        }
    }

    public void countToShowAds(int startAds, int loopAds, String keyCount)
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
        bool isShowAds = false;
        if (countFullAds < startAds)
        {
            isShowAds = false;
        }
        else if (countFullAds == startAds)
        {
            isShowAds = true;
        }
        else
        {
            if ((countFullAds - startAds) % loopAds == 0)
            {
                isShowAds = true;
            }
            else
            {
                isShowAds = false;
            }
        }

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
            Debug.Log("xxx The AppOpenAd ad wasn't ready yet.");
            //            activity.closeAds(TYPE_ADS.AppOpenAdAd);
            //            loadAds();
        }
    }

    public bool canShowAds()
    {
        return (mAppOpenAd != null && mAppOpenAd.CanShowAd());
    }
}
