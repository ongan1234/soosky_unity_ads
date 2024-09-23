using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobInterstitialAd
{
    public static AdmobInterstitialAd Instance = new AdmobInterstitialAd();

    private InterstitialAd mInterstitialAd;
    private List<string> list_interstitial_ad_unit_id;
    private int countTier = 0;
    private AdManager adManager;
    private string keyPos;

    public AdmobInterstitialAd()
    {
        list_interstitial_ad_unit_id = AdmobAdId.interId;
        adManager = AdManager.Instance;
    }

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void loadAds()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable) { return; }
        if (adManager == null) { return; }
        if (PlayerPrefsManager.hasSUB()) { return; }
        if (mInterstitialAd != null) { return; }
        Debug.Log("Loading the interstitial ad.");
        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(list_interstitial_ad_unit_id[countTier], adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    mInterstitialAd = null;
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                mInterstitialAd = ad;
                RegisterEventHandlers(mInterstitialAd);
            });

        if (countTier >= list_interstitial_ad_unit_id.Count - 1)
        {
            countTier = 0;
        }
        else
        {
            countTier++;
        }
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");

            destroyAd();
            closeAds();
            loadAds();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            mInterstitialAd = null;
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void closeAds()
    {
        if(adManager != null)
        {
            adManager.closeAds(TYPE_ADS.InterstitialAd, keyPos);
        }
    }

    public void destroyAd()
    {
        // Clean up the old ad before loading a new one.
        if (mInterstitialAd != null)
        {
            mInterstitialAd.Destroy();
            mInterstitialAd = null;
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
            adManager.ShowPopupLoadAds(TYPE_ADS.InterstitialAd, keyPos);
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
            mInterstitialAd.Show();
        }
        else
        {
            Debug.Log("xxx The interstitial ad wasn't ready yet.");
            closeAds();
            //loadAds();
        }
    }

    public bool canShowAds()
    {
        return (mInterstitialAd != null && mInterstitialAd.CanShowAd());
    }
}
