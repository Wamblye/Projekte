using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class OfflineProgress : MonoBehaviour
{
    public SpawnPrefab spawnPrefab;
    public GameObject offlinePopup;
    public static int leafs;
	int x = 540;
	int y = 1000;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("LAST_LOGIN"))
        {
            DateTime lastLogin = DateTime.Parse(PlayerPrefs.GetString("LAST_LOGIN"));
            TimeSpan ts = DateTime.Now - lastLogin;

            float rawSeconds = (float) ts.TotalSeconds;

			berechneOfflineLeafs();

            if (leafs > 0) 
            {
                spawnPrefab.SpawnOfflinePopup(x, y, leafs.ToString("n0"));
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LAST_LOGIN", DateTime.Now.ToString());
    }
}
