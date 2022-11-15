using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static GameData gameData;

    public TextMeshProUGUI textCoins;

    public Sprite spriteLevelAktiviert;
    public Sprite spriteLevelDeaktiviert;

    public List<GameObject> listLevelButtons;


   void Awake()
   {
       gameData = SaveSystem.Load();
   }

   void Start()
   {
       textCoins.SetText(gameData.coins.ToString());
       
       if (gameData.spielmodus == 1) //Block Zerstörung
       {
		   // Alle Level Icons je nach Level-Spielstand aktivieren bzw. deaktivieren
           foreach(GameObject go in listLevelButtons)
            {
                foreach(LevelMenueDaten ld in gameData.levelMenueDaten)
                {
                    int level = int.Parse(go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
                    if (level == ld.level)
                    {
                        if (level <= gameData.level)
                        {
                            go.transform.GetComponent<Image>().sprite = spriteLevelAktiviert;
                        }
                        else 
                        {
                            go.transform.GetComponent<Image>().sprite = spriteLevelDeaktiviert;
                        }
                    }
                }
            }
       }
   }

   

    public void NextArea(int num)
    {
        LevelLoader.instance.LoadLevel(num);
    }
}
