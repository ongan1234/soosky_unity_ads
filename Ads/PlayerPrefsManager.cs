using System;
using UnityEngine;

public class PlayerPrefsManager
{
    public static bool hasLifetime()
    {
        return PlayerPrefs.HasKey("APP_IAP_LIFETIME");
    }

    public static bool hasSUB()
    {
        return PlayerPrefs.HasKey("APP_SUB_PRO");
    }

    public static void purchaseLifetime()
    {
        PlayerPrefs.SetString("APP_IAP_LIFETIME", "IAP_SUCCESS");
    }

    public static void purchaseAndRestoreSuccess()
    {
        PlayerPrefs.SetString("APP_SUB_PRO", "SUB_SUCCESS");
    }

    public static void purchaseFailed()
    { 
        PlayerPrefs.DeleteKey("APP_SUB_PRO");
        PlayerPrefs.DeleteKey("APP_IAP_LIFETIME");
    }

    public static void saveCounterAds(String key, int count)
    {
        PlayerPrefs.SetInt(key, count);
    }
    public static int getCounterAds(String key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }
}
