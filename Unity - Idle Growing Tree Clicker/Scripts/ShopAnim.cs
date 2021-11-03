using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopAnim : MonoBehaviour
{
    void Start()
    {
        shop = new List<Shop>();
        InvokeRepeating("CheckBuy", 1, 1);
    }

    public void CheckBuy()
    {
        foreach(Shop s in shop) 
        {
            SetButtonsActive(s.button, s.cost);
        }
    }

    public void ShowHideShop(int num) 
    {
        if (shopMenu != null) 
        {
            Animator animator = shopMenu.GetComponent<Animator>();
            if (animator != null) 
            {
                bool isOpen = animator.GetBool("show");
                if (isOpen == false)
                {
                    if (num == 1)
                    {
                        LoadShopDaten();
                    }
                    else if (num == 2)
                    {
                        LoadBoostDaten();
                    }
                    else if (num == 3)
                    {
                        LoadBaumDaten();
                    }
                    else if (num == 4)
                    {
                        LoadGoldDaten();
                    }
                }
                
                animator.SetBool("show", !isOpen);
            }
        }
    }

    public void LoadShopDaten()
    {
        Debug.Log("Daten wurden geladen");
        shop = new List<Shop>();

        for(int i = 1; i <= 16; i++) 
        {
            button = GameObject.FindGameObjectWithTag("Button" + i.ToString());
            
            // Default Werte holen
            string rewardDefault = button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
            //string costDefault = button.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
            string costString = button.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
            string costDefault = costString.Replace(".", "");
            string levelDefault = button.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text;
            // Speicher Daten laden
            float reward = PlayerPrefs.GetFloat("reward" + i.ToString(), float.Parse(rewardDefault));
            long cost = long.Parse(PlayerPrefs.GetString("cost" + i.ToString(), costDefault));
            int level = PlayerPrefs.GetInt("level" + i.ToString(), int.Parse(levelDefault));

            // TEST TEST TEST
            SetButtonsActive(button, cost);
            shop.Add(new Shop(reward, cost, level, button));

            // Reward Text
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(reward.ToString());
            // Cost Text
            button.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(cost.ToString("n0"));
            // Level Text
            button.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(level.ToString());
        }
    }


    public void SetButtonsActive(GameObject button, long cost) 
    {
        //GameObject obj = GameObject.FindGameObjectWithTag("Button1");
        if (GameManager.punkte < cost)
        {
            button.transform.GetComponent<Image>().sprite = spriteDeactivated;
        } else 
        {
            button.transform.GetComponent<Image>().sprite = spriteActivated;
        }
    }



}
