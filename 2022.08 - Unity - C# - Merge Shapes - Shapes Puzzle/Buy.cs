using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Buy : MonoBehaviour
{
    public static bool buyLoesung = false;
    public static bool earnedLoesung = false;

    public TextMeshProUGUI fehlerText;

    public GameObject buttonBereich;
    public GameObject loesungBereich;

    private GameManager manager;

    public void BuyLoesung(int num) 
    {
        manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        if (num == 1) // Gold
        {
            int cost = 50;
            if (GameManager.gameData.coins >= cost)
            {
                AudioManager.instance.Play("Sell");
                GameManager.gameData.coins -= cost;
                SaveSystem.Save(GameManager.gameData);

                manager.SkipLevel();
            }
            else
            {
                // Nicht genug Geld
                fehlerText.enabled = true;
                Invoke("ResetText", 3);
            }
        }
        else if (num == 2) // Ad
        {	
			// Check ob Internet Verbindung vorhanden ist
            AdManager.instance.CheckInternet();

			// Iinternet ist da -> Werbung zeigen
            if (AdManager.OfflineWarnungAktiv == false)
            {
                buyLoesung = true;
                AdManager.instance.ShowRewardedAd();
            }
        }
    }

    public void ResetText()
    {
        fehlerText.enabled = false;
    }




}
