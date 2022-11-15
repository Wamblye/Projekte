using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectLevel : MonoBehaviour
{
    public GameObject button;
    public static int currentLevel;

    public void StartLevel()
    {
        currentLevel = int.Parse(button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        LevelManager.gameData.currentLevel = currentLevel;
        SaveSystem.Save(LevelManager.gameData);
        int buildIndex = 2;

       if (currentLevel <= LevelManager.gameData.level)
       {
            if (currentLevel > 5)
            {
                if (currentLevel % 3 == 0)
                {
                    Debug.Log("ShowAd");
                    ShowAd();
                }
                else
                {
                    Debug.Log("Show Level ohne Ad");
                    LevelLoader.instance.LoadLevel(currentLevel + buildIndex);
                }
            }
            else
            {
                LevelLoader.instance.LoadLevel(currentLevel + buildIndex);
            }
       }
   }


    public void ShowAd()
    {
        AdManager.instance.CheckInternet();

        if (AdManager.OfflineWarnungAktiv == false)
        {
            AdManager.instance.ShowInterstitial();
        }
    }


}
