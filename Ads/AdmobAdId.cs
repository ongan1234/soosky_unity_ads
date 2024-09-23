using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdmobAdId
{
#if UNITY_EDITOR
    public static List<string> opendAdsId = new List<string>()
    {
        "ca-app-pub-3940256099942544/9257395921",
        "ca-app-pub-3940256099942544/9257395921"
    };
    public static List<string> interId = new List<string>()
    {
        "ca-app-pub-3940256099942544/1033173712"
    };
    public static List<string> rewardedId = new List<string>()
    {
        "ca-app-pub-3940256099942544/5224354917",
        "ca-app-pub-3940256099942544/5224354917",
        "ca-app-pub-3940256099942544/5224354917"
    };
    //string bannerId = "ca-app-pub-3940256099942544/6300978111";
    public static string bannerId = "ca-app-pub-3940256099942544/2014213617";
    public static string nativeId = "ca-app-pub-3940256099942544/2247696110";
    public static string adsAppID = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_ANDROID
    //*** Ads Test ***
    public static List<string> opendAdsId = new List<string>()
    {
        "ca-app-pub-3940256099942544/9257395921",
        "ca-app-pub-3940256099942544/9257395921"
    };
    public static List<string> interId = new List<string>()
    {
        "ca-app-pub-3940256099942544/1033173712"
    };
    public static List<string> rewardedId = new List<string>()
    {
        "ca-app-pub-3940256099942544/5224354917",
        "ca-app-pub-3940256099942544/5224354917",
        "ca-app-pub-3940256099942544/5224354917"
    };
    public static string bannerId = "ca-app-pub-3940256099942544/9214589741";
    public static string nativeId = "ca-app-pub-3940256099942544/2247696110";
    public static string adsAppID = "ca-app-pub-3940256099942544~3347511713";

    //*** Ads Real *** 
    //public static List<string> opendAdsId = new List<string>()
    //{
    //    ""
    //};
    //public static List<string> interId = new List<string>()
    //{
    //    ""
    //};
    //public static List<string> rewardedId = new List<string>()
    //{
    //    ""
    //};
    //public static string bannerId = "";
    //public static string collapBannerId = "";
    //public static string nativeId = "";
    //public static string adsAppID = "";
#elif UNITY_IPHONE || UNITY_IOS
    //*** Ads Test ***
    public static List<string> opendAdsId = new List<string>()
    {
        "ca-app-pub-3940256099942544/5575463023",
        "ca-app-pub-3940256099942544/5575463023"
    };
    public static List<string> interId = new List<string>()
    {
        "ca-app-pub-3940256099942544/4411468910"
    };
    public static List<string> rewardedId = new List<string>()
    {
        "ca-app-pub-3940256099942544/1712485313",
        "ca-app-pub-3940256099942544/1712485313",
        "ca-app-pub-3940256099942544/1712485313"
    };
    public static string bannerId = "ca-app-pub-3940256099942544/2934735716";
    public static string nativeId = "ca-app-pub-3940256099942544/3986624511";
    public static string adsAppID = "ca-app-pub-3940256099942544~1458002511";

    //*** Ads Real ***
    //public static List<string> opendAdsId = new List<string>()
    //{
    //    ""
    //};
    //public static List<string> interId = new List<string>()
    //{
    //    ""
    //};
    //public static List<string> rewardedId = new List<string>()
    //{
    //    ""
    //};
    //public static string bannerId = "";
    //public static string nativeId = "";
    //public static string adsAppID = "";
#endif
}
