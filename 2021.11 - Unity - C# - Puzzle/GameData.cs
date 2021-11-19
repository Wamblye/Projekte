using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int level;
    public int count;
	public int gold;
    public int diamanten;

    //[SerializeField] public List<Stones> stonesList;
    [SerializeField] public List<ShopDaten> shopDatenList;

    public GameData()
    {
        level = 1;
        count = 14;
		gold = 0;
		diamanten = 0;
		
		shopDatenList = new List<ShopDaten>();
    }
}
