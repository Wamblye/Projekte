using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Networking;

public class AdManager : MonoBehaviour 
{
    private BannerView bannerAd;
    // Aufruf beliebiger zeitpunkt
    private InterstitialAd interstitial;
    // Werbung für eine Belohnung
    private RewardedAd rewardedAd;

    public static AdManager instance;

    public static bool OfflineWarnungAktiv;
    private bool bannerLoaded;
    public GameObject prefabOffline;
    private GameObject instPrefab;

    private IEnumerator couroutine;
    public IEnumerator couroutineCheckInternet;

    // Admon IDs
    string adUnitID_Banner = "---";
    string adUnitID_Interstitial = "---";
    string adUnitID_Reward = "---";


    void Awake()
    {
        if (instance != null)
		{
			Destroy(gameObject);
		} 
        else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
    }

    void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });

        bannerLoaded = false;
        OfflineWarnungAktiv = false;

		// Regelmäßig checken ob Ads geladen wurden. Wenn nicht -> laden
        couroutine = OnCoroutineCheckAdIsLoaded();
        StartCoroutine(couroutine);
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private void RequestBanner()
    {
        this.bannerAd = new BannerView(adUnitID_Banner, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();

        this.bannerAd.OnAdLoaded += HandleBannerAdLoaded;
        this.bannerAd.OnAdFailedToLoad += HandleBannerAdFailedToLoad;

        this.bannerAd.LoadAd(request);
    }

    public void RequestInterstitial()
    {
        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        this.interstitial = new InterstitialAd(adUnitID_Interstitial);

        this.interstitial.OnAdLoaded += HandleInterstitialAdLoaded;
        this.interstitial.OnAdFailedToLoad += HandleInterstitialAdFailedToLoad;
        this.interstitial.OnAdOpening += HandleInterstitialAdOpening;
        this.interstitial.OnAdFailedToShow += HandleInterstitialAdFailedToShow;
        this.interstitial.OnAdClosed += HandleInterstitialAdClosed;

        this.interstitial.LoadAd(this.CreateAdRequest());
    }

    public void RequestRewardedAd()
    {
        this.rewardedAd = new RewardedAd(adUnitID_Reward);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        this.rewardedAd.LoadAd(this.CreateAdRequest());
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            Debug.Log("Interstitial is loaded");
            interstitial.Show();
        }
        else
        {
            Debug.Log("Interstitial IsNotLoaded");
        }
    }

    public void ShowRewardedAd()
    {
        if (this.rewardedAd.IsLoaded()) 
        {
            Debug.Log("rewardedAd is loaded");
            this.rewardedAd.Show();
        }
        else 
        {
            Debug.Log("IsNotLoaded");
        }
    }

    // Banner Ad
    public void HandleBannerAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleBannerAdLoaded");
        bannerLoaded = true;
    }

    public void HandleBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleBannerAdFailedToLoad");
        bannerLoaded = false;
    }


    // Interstitial Ad
    public void HandleInterstitialAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialAdLoaded");
    }

    public void HandleInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleInterstitialAdFailedToLoad");
    }

    public void HandleInterstitialAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialAdOpening");
    }

    public void HandleInterstitialAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleInterstitialAdFailedToShow");
    }

    public void HandleInterstitialAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleInterstitialAdClosed");

		// Je nach Spielmodus zum richtigen Level Übersichtsberiech zurückkehren
        if (LevelManager.gameData.spielmodus == 1)
        {
            LevelLoader.instance.LoadLevel(SelectLevel.currentLevel + LevelLoader.buildNumber); 
        }
        else if (LevelManager.gameData.spielmodus == 2)
        {
            LevelLoader.instance.LoadLevel(SelectLevel.currentLevel + LevelLoader.buildNumberPerfekteKlicks); 
        }
        
    }


    // Rewarded Ad
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToLoad");
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToShow");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdClosed");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        Debug.Log("HandleUserEarnedReward");
        string type = args.Type;
        double amount = args.Amount;
        
        if (Buy.buyLoesung == true)
        {
            Buy.buyLoesung = false;
            Buy.earnedLoesung = true;
        }

        this.RequestRewardedAd();
    }

    private void SpawnPrefabOffline()
    {
        instPrefab = Instantiate(prefabOffline);

        instPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("GameCanvas").transform, false);
    }

    IEnumerator OnCoroutineCheckAdIsLoaded()
    {
        while(true) 
        { 
            // Ad Banner
            if (bannerLoaded == false)
            {
                this.RequestBanner();
            }

            // Ad Interstitial
            if (this.interstitial != null)
            {
                if (this.interstitial.IsLoaded())
                {
                    Debug.Log("Interstitial is loaded");
                }
                else
                {
                    Debug.Log("Interstitial IsNotLoaded");
                    this.RequestInterstitial();
                }
            }
            else
            {
                this.RequestInterstitial();
            }

            // Ad Rewarded Interstitial
            if (this.rewardedAd != null)
            {
                if (this.rewardedAd.IsLoaded()) 
                {
                    Debug.Log("rewardedAd is loaded");
                }
                else 
                {
                    Debug.Log("RewaredAd IsNotLoaded");
                    this.RequestRewardedAd();
                }
            }
            else
            {
                this.RequestRewardedAd();
            }
            
            
            yield return new WaitForSeconds(6f);
        }
    }

    IEnumerator CheckRequest()
    {
        // Prüfen ob eine Verbindung zum Internet besteht
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();

        if (request.error != null) 
        {
            Debug.Log("Error. Check internet connection!");
            if (OfflineWarnungAktiv == false)
            {
                Debug.Log("SpawnPrefabOffline");
                OfflineWarnungAktiv = true;
                SpawnPrefabOffline();
            }

        }
        else
        {
            Debug.Log("Internet is On");
            if (instance != null)
            {
                OfflineWarnungAktiv = false;
            }
        }
    }

    public void CheckInternet()
    {
        couroutineCheckInternet = CheckRequest();
        StartCoroutine(couroutineCheckInternet);
    }

    


}
