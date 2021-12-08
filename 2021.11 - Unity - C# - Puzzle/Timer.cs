using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public static float timedSpin;
    
    void Awake ()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		} else
		{
			instance = this;
            DontDestroyOnLoad(gameObject);
		}
	}

    void Start()
    {
        if (PlayerPrefs.HasKey("LAST_LOGIN"))
        {
            float timerSpin = PlayerPrefs.GetFloat("TIMER");
            DateTime lastLogin = DateTime.Parse(PlayerPrefs.GetString("LAST_LOGIN"));
            TimeSpan ts = DateTime.Now - lastLogin;

            float rawSeconds = (float) ts.TotalSeconds;
            float restTimer = timerSpin - rawSeconds;

            if (restTimer <= 0)
            {
                Timer.timedSpin = 0;
            }
            else 
            {
                Timer.timedSpin = restTimer;
                Timer.instance.StartCoroutine(Timer.instance.TimerTake());
            }
            //Debug.Log("sec: " + rawSeconds + " offlineLeafs: " + offlineLeafs + " ProzentLeafs: " + leafsProzent+  " leafs: " + leafs);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LAST_LOGIN", DateTime.Now.ToString());
        PlayerPrefs.SetFloat("TIMER", timedSpin);
    }

    
    public IEnumerator TimerTake()
    {
        while (timedSpin > 0)
        {
            yield return new WaitForSeconds(1f);
            timedSpin -= 1f;
        }
    }

}
