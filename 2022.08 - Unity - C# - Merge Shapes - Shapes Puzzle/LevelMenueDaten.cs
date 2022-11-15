using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelMenueDaten
{
    public int level;
    public int erze;
    public int diamanten;
    public int artefakte;

    public LevelMenueDaten(int newLevel, int newErze, int newDiamanten, int newArtefakte)
    {
        level = newLevel;
        erze = newErze;
        diamanten = newDiamanten;
        artefakte = newArtefakte;
    }
}
