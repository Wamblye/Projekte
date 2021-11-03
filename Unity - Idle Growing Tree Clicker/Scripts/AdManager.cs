using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour 
{
    private BannerView bannerAd;
    // Aufruf beliebiger zeitpunkt
    private InterstitialAd interstitial;
    // Werbung für eine Belohnung
    private RewardedAd rewardedAd;

    public static AdManager instance;

    public HealthBar healthBar;
    public SpawnPrefab spawnBoost;

    void Awake()
    {
        Debug.Log("awake");
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Debug.Log("start");
        MobileAds.Initialize(InitializationStatus => { });
        this.RequestBanner();

        bannerAd.Show();


        this.RequestRewardedAd();

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void RequestBanner()
    {
        string adUnitID = "ca-app-pub-xxxxxxxxxxxxxxx";
        this.bannerAd = new BannerView(adUnitID, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        this.bannerAd.LoadAd(request);
    }

    public void RequestInterstitial()
    {
        string adUnitID = "ca-app-pub-xxxxxxxxxxxxxxx";

        // Clean up interstitial ad before creating a new one
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        this.interstitial = new InterstitialAd(adUnitID);

        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void RequestRewardedAd()
    {
        string adUnitID = "ca-app-pub-xxxxxxxxxxxxxxxxxx";
        this.rewardedAd = new RewardedAd(adUnitID);

        this.rewardedAd.LoadAd(this.CreateAdRequest());
    }

    public void ShowInterstitial()
    {
        this.RequestInterstitial();
        if (this.interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            Debug.Log("Interstitial noch nicht bereit");
        }
    }

    public void ShowRewardedAd()
    {
        //Debug.Log("ShowRewardedAd");
        this.RequestRewardedAd();
        if (this.rewardedAd.IsLoaded()) 
        {
            this.rewardedAd.Show();
        }
        else 
        {
            Debug.Log("IsNotLoaded");
        }
    }


    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdFailedToLoad event received with message: " + args.Message);
        Debug.Log("FailedTooLoad");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdFailedToShow event received with message: " + args.Message);
        Debug.Log("FailedTooShow");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        //MonoBehaviour.print("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);
        if (Buy.buyRewardedBoost == true)
        {
            BuyBoost();
            Buy.buyRewardedBoost = false;
        }
        else if (Buy.buyRewardedFullHeal == true)
        {
            BuyFullHeal();
            Buy.buyRewardedFullHeal = false;
        }
        else if (Buy.buyRewardedGold == true)
        {
            BuyGold();
            Buy.buyRewardedGold = false;
        }
    }

    public void BuyBoost()
    {
        GameManager.boost = true;
        GameManager.sec = 1800;

        spawnBoost.Spawn("BoostImage", 70, 1500);
    }

    public void BuyFullHeal()
    {
        healthBar.SetHealth(1000);
        PlayerPrefs.SetFloat("health", healthBar.GetValue());
    }

    public void BuyGold()
    {
        GameManager.gold += 100;
        PlayerPrefs.SetInt("gold", GameManager.gold);
    }


}
