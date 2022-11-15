using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public static GameData gameData;

    public LevelLoader levelLoader;
    public Animator animator;

    public Sprite audioOn;
    public Sprite audioOff;
    public GameObject buttonAudio;
    private bool muted = false;

    private int levelScreenIndex = 1;

    void Awake()
    {
        //SaveSystem.Delete();
       gameData = SaveSystem.Load();
    }

    void Start()
    {
        muted = gameData.muted;
        SetStartAudio();
    }

    public void LoadLevelWithAnimation()
    {
        if (animator != null)
        {
            animator.Play("Load"); 
        }
    }

    public void Spielen()
    {
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

    public void SetAudio()
    {
        AudioManager.instance.Play("Click");
        if (muted == false)
        {
            AudioListener.volume = 0;
            buttonAudio.transform.GetComponent<Image>().sprite = audioOff;
            gameData.muted = true;
            muted = true;
        }
        else
        {   
            AudioListener.volume = 1;
            buttonAudio.transform.GetComponent<Image>().sprite = audioOn;
            gameData.muted = false;
            muted = false;
        }
        SaveSystem.Save(gameData);
    }

    public void SetStartAudio()
    {
        if (muted == true)
        {
            AudioListener.volume = 0;
            buttonAudio.transform.GetComponent<Image>().sprite = audioOff;
        }
        else
        {   
            AudioListener.volume = 1;
            buttonAudio.transform.GetComponent<Image>().sprite = audioOn;
        }
    }



}
