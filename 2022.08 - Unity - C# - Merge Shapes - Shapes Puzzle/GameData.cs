using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int level;
    public int currentLevel;
    public int currentLevelScreen;

    public int coins;
    public int diamonds;

    public bool muted;

    public int spielmodus;

    [SerializeField] public List<LevelMenueDaten> levelMenueDaten;

    public GameData()
    {
        level = 1;
        currentLevel = 1;
        currentLevelScreen = 1;

        coins = 0;
        diamonds = 0;

        muted = false;

        spielmodus = 1;

        levelMenueDaten = new List<LevelMenueDaten>();
    }
}
