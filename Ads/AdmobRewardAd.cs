using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobRewardAd
{
    public static AdmobRewardAd Instance = new AdmobRewardAd();

    public AdManager adManager;
    private RewardedAd mRewardedAd;
    private bool isReward;
    private List<String> list_reward_ad_unit_id;
    private int countTier = 0;
    private string keyPos;

    public AdmobRewardAd()
    {
        list_reward_ad_unit_id = AdmobAdId.rewardedId;
        adManager = AdManager.Instance;
    }

    public void loadAds()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) { return; }
        if (adManager == null) { return; }
        //        if(PreferencesManager.checkSUB() != null)return;
        if (mRewardedAd != null) { return; }
        isReward = false;
        AdRequest adRequest = new AdRequest();

        RewardedAd.Load(list_reward_ad_unit_id[countTier], adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  mRewardedAd = null;
                  return;
              }

              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              mRewardedAd = ad;
              RegisterEventHandlers(ad);
          });

        if (countTier >= list_reward_ad_unit_id.Count - 1)
        {
            countTier = 0;
        }
        else
        {
            countTier++;
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            destroyAd();
            closeAds();
            loadAds();

        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void closeAds()
    {
        if (adManager != null)
        {
            Debug.Log("xxx mRewardedAd closeAds " + isReward);
            if (isReward)
            {
                adManager.haveReward();
            }
            else
            {
                adManager.notHaveReward();
            }
            adManager.closeAds(TYPE_ADS.RewardAd, keyPos);
        }
    }

    public void destroyAd()
    {
        // Clean up the old ad before loading a new one.
        if (mRewardedAd != null)
        {
            mRewardedAd.Destroy();
            mRewardedAd = null;
        }
    }


    public void ShowAds(string key_pos)
    {
        if (adManager == null) return;
        keyPos = key_pos;
        //        if(PreferencesManager.checkSUB() != null )return;
        if (canShowAds())
        {
            mRewardedAd.Show((Reward reward) =>
            {
                Debug.Log("xxx mRewardedAd have reward " + reward.Amount);
                isReward = true;
            });
        }
        else
        {
            Debug.Log("xxx The reward ad wasn't ready yet.");
            closeAds();
            //            loadAds();
        }
    }

    public bool canShowAds()
    {
        return (mRewardedAd != null && mRewardedAd.CanShowAd());
    }
}
