using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Buy : MonoBehaviour
{
    public void buyBoostShop(int num)
    {
        if (num == 1)
        {
            buyRewardedBoost = true;
            AdManager.instance.ShowRewardedAd();
        } 
        else if (num == 2) 
        {
            buyRewardedFullHeal = true;
            AdManager.instance.ShowRewardedAd();
        } 
        else if (num == 3) 
        {
            cost = int.Parse(button.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text);
            level = int.Parse(button.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text);

            if (GameManager.punkte >= cost)
            {
                AudioManager.instance.Play("Selling");
                long basisCost = 100;
                BuyHeal(basisCost);
            }
        }
        else if (num == 4) 
        {
            Debug.Log(GameManager.spawnFruitChance);

            cost = int.Parse(button.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text);
            level = int.Parse(button.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text);

            if (GameManager.punkte >= cost)
            {
                AudioManager.instance.Play("Selling");
                long basisCost = 10000;
                BuyAppleChance(basisCost);
            }
        }
        else if (num == 5) 
        {
            buyRewardedGold = true;
            AdManager.instance.ShowRewardedAd();
        }
    }
   


    public void BuyHeal(long basisCost)
    {
        GameManager.punkte -= cost;
        cost = incrementalNextCost(basisCost);
        level++;

        // Nicht über das Maximum hinaus heilen
        if (healthBar.GetValue() <= 980)
        {
            healthBar.PlusHealth(heilung);
        }
        else 
        {
            healthBar.SetHealth(1000);
        }

        PlayerPrefs.SetFloat("health", healthBar.GetValue());
        PlayerPrefs.SetFloat("punkte", GameManager.punkte);
        PlayerPrefs.SetString("healCost", cost.ToString());
        PlayerPrefs.SetInt("healLevel", level);

        // Cost Text
        button.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(cost.ToString());
        button.transform.GetChild(4).GetComponent<TextMeshProUGUI>().SetText(level.ToString());

        // Nach Abzug der KOsten von den Punkten prüfen, ob der Button deaktiviert werden
        // muss oder er nochmal kaufen kann
        if (GameManager.punkte < cost)
        {
            button.transform.GetComponent<Image>().sprite = spriteDeactivated;
        }
    }


    public void UpdateText(float reward, long cost, int level)
    {
        // Reward Text
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(reward.ToString());
        // Cost Text
        button.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(cost.ToString("n0"));
        // Level Text
        button.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(level.ToString());
                
    }

    public void ResetText()
    {
        fehlerText.enabled = false;
    }



}
