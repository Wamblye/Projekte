using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static float punkte;
    public static bool boostApple = false;
    public static int incrementAmount;
    public GameObject clickerBaum;
    public TextPopup textPopup;
    public static float exp;

    // Unkraut Spwawner
    public UnkrautSpawner unkrautSpawner;

    public static float spawnFruitChance;
    public static float sec;
    public SpawnPrefab totPrefab;
    // Bäume
    public static int baum;
    public Sprite spriteBaum1Lev1;
    public Sprite spriteBaum1Lev2;
    public Sprite spriteBaum1Lev3
    public static int goldShop1;
    public static int goldShop2;



    void Start() {
        // Gesamt Punkte
        punkte = PlayerPrefs.GetFloat("punkte", 0);
        gold = PlayerPrefs.GetInt("gold", 0);
        // aktueller Baum
        goldShop1 = PlayerPrefs.GetInt("goldShop1", 0);
        goldShop2 = PlayerPrefs.GetInt("goldShop2", 0);

        spawnUnkrautChance = 5f;

        changeBaum();

        healthBar.SetMaxHealth(1000, health);
        levelBar.SetMaxHealth(maxExp, expValue);
        SetTextToTextMeshProUI("LevelZahl", level);

        InvokeRepeating("IncrementIdle", 1, 1);

        if (Random.Range(0, 5) == 0)
        {
            AdManager.instance.ShowInterstitial();
        }
        
    }

    void Update() {
        if(boost)
        {
            sec -= Time.smoothDeltaTime;
            if(sec >= 0)
            {
                 int ganzeSekunden = (int) sec;
                 SetTextToTextMeshProUI("BoostImageSec", ganzeSekunden, " sec");
            }
            else
            {
                 //Debug.Log("Done");
                 boost = false;
                 spawnPrefab = GameObject.FindGameObjectWithTag("BoostImage").GetComponent<SpawnPrefab>();
                 spawnPrefab.DestroyPrefab();
                 sec = 1800;
            }
        }

        SetTextToTextMeshProUI("Punkte", GameManager.punkte.ToString("n0"));

        // Leben auf Null gesunken, Punkte und Gold zurücksetzen
        if (healthBar.GetValue() <= 0)
        {
            ResetPunkteGold();
        }
    }

    public void IncrementIdle() 
    {
        punkte += idleIncrement * boostBasisApple;
    }

    public void Increment() {
        AudioManager.instance.Play("Click");

        exp += incrementAmount;
        expValue += incrementAmount;

        punkte += incrementAmount;

        levelBar.SetHealth(expValue);

        textPopup.showTextPopup(incrementAmount.ToString());

        // Unkraut Spawner
        if (randomProzentChance() < spawnUnkrautChance)
        {
            unkrautSpawner.spawnUnkraut();
        }

        if (expValue > maxExp) 
        {
            LevelUp();
            changeBaum();
        }
        
    }

    public void LevelUp()
    {
        AudioManager.instance.Play("LevelUp");

        level++;
        expValue = 0;
        maxExp += 4000;
        levelUp = true;

        PlayerPrefs.SetInt("level", GameManager.level);

        levelBar.SetMaxHealth(maxExp, expValue);
        SetTextToTextMeshProUI("LevelZahl", level);
    }
   
    
    public void SetTextToTextMeshProUI(string tag, float wert)
    {
            var obj = GameObject.FindGameObjectWithTag(tag);
            obj.GetComponent<TextMeshProUGUI>().SetText(wert.ToString());
    }
    public void SetTextToTextMeshProUI(string tag, string wert)
    {
            var obj = GameObject.FindGameObjectWithTag(tag);
            obj.GetComponent<TextMeshProUGUI>().SetText(wert.ToString());
    }
    public void SetTextToTextMeshProUI(string tag, float wert, string text)
    {
            var obj = GameObject.FindGameObjectWithTag(tag);
            obj.GetComponent<TextMeshProUGUI>().SetText(wert.ToString() + text);
    }
    public void SetTextToTextMeshProUI(string tag, string wert, string text)
    {
            var obj = GameObject.FindGameObjectWithTag(tag);
            obj.GetComponent<TextMeshProUGUI>().SetText(wert.ToString() + text);
    }

     private float randomProzentChance() {
        float random = Random.Range(0.0f, 100.0f);
        //Debug.Log("random: " + random);
        return random;
    }


}
