using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
        gameData = SaveSystem.Load();
        explosion = GameObject.FindWithTag("Explosion");
        mergeWolke = explosion.GetComponent<ParticleSystem>();
    }

    void Start()
    {
        ResumeGame();
        level = gameData.level;
        currentLevel = gameData.currentLevel;
        collisionMerge = false;
        abgabeKorrekt1 = false;
        abgabeKorrekt2 = false;
        abgabeKorrekt3 = false;
        abgabeKorrekt4 = false;
        abgabeKorrekt5 = false;
        abgabeSlotsFalse = 0;
        geschafftFensterOffen = false;
        CheckObVorgabeLeer();
    }

    void Update()
    {
        EarnedRewardedAd();
        textCoins.SetText(gameData.coins.ToString());
        CheckLevelGeschafft();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        CancelInvoke();
    }

    void ResumeGame ()
    {
        Time.timeScale = 1;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public List<Sprite> GetSprites()
    {
        return listSpritesSlimes;
    }


    public void RemoveColorsBlack(int form, int farbe)
    {
		// Alle schwarzen Formen entfernen
        for (int i = 0; i < listSpielfeld.Count; i++)
        {  
            if (listSpielfeld[i] != null)
            {
                string spriteNameGesamt = listSpielfeld[i].transform.GetComponent<Image>().sprite.name;
                int found = spriteNameGesamt.IndexOf("-");
                string textReplace = "-" + spriteNameGesamt.Substring(found + 1);

                string formArt = spriteNameGesamt.Replace(textReplace, "").ToString();
                string farbeM = spriteNameGesamt.Substring(found + 1);

                if (int.Parse(formArt) == form && int.Parse(farbeM) == farbe)
                {
                    Destroy(listSpielfeld[i]);
                }
            }
        }
    }

    public void AddListSpielfeld(GameObject obj)
    {
        listSpielfeld.Add(obj);
    }

    public List<GameObject> GetVorgabeGameObjekts()
    {
        return listVorgabeGameObjekts;
    }

    private void CheckLevelGeschafft()
    {
        if (abgabeKorrekt1 == true && abgabeKorrekt2 == true && abgabeKorrekt3 == true &&
            abgabeKorrekt4 == true && abgabeKorrekt5 == true)
        {
            if (geschafftFensterOffen == false)
            {
                AudioManager.instance.Play("Success");
                spawnPrefab.SpawnGeschafft();
                geschafftFensterOffen = true;
            }   
        }
    }


    public void SkipLevel()
    {
        if (gameData.currentLevel == gameData.level)
        {
            gameData.level += 1;
        }
        Debug.Log("NextLevel: CurrentLevel - " + gameData.currentLevel + " Level - " + gameData.level);

        SaveSystem.Save(gameData);

        if (gameData.currentLevel >= 85)
        {
            levelLoader.LoadLevel(levelScreenIndex + 2);
        }
        else if (gameData.currentLevel >= 44)
        {
            levelLoader.LoadLevel(levelScreenIndex + 1);
        }
        else 
        {
            levelLoader.LoadLevel(levelScreenIndex);
        }
    }

  
    public void CloseTutorial()
    {
        tutorial.SetActive(false);
    }

    public void EarnedRewardedAd()
    {
        if (Buy.earnedLoesung == true)
        {
            SkipLevel();
            Buy.earnedLoesung = false;
        }
    }



    



}

