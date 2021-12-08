using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpinWheel : MonoBehaviour
{
    private int randomValue;
    private float timeInterval;
    private bool coroutineAllowed;
    private int finalAngle;

    public GameObject textWon;

    
    void Start()
    {
        coroutineAllowed = true;
    }

    public void StartSpin()
    {
        if (coroutineAllowed == true && Timer.timedSpin <= 0)
        {
            Timer.timedSpin = 3600f;
            Timer.instance.StartCoroutine(Timer.instance.TimerTake());
            StartCoroutine(Spin());
        }
    }

    

    private IEnumerator Spin()
    {
        coroutineAllowed = false;
        randomValue = Random.Range(20, 30);
        timeInterval = 0.1f;

        for (int i = 0; i < randomValue; i++)
        {
            transform.Rotate(0, 0, 22.5f);
            yield return new WaitForSeconds(timeInterval);
        }

        if (Mathf.RoundToInt(transform.eulerAngles.z) % 45 != 0)
        {
            transform.Rotate(0, 0, 22.5f);
        }

        finalAngle = Mathf.RoundToInt(transform.eulerAngles.z);

        switch (finalAngle)
        {
            case 0:
                Debug.Log("You Win 1 Diamond");
                textWon.transform.GetComponent<TextMeshProUGUI>().enabled = true;
                textWon.transform.GetComponent<TextMeshProUGUI>().SetText("You Won 1 Diamond");
                SpinningWheelManager.gameData.diamonds += 1;
                break;
            case 45:
                Debug.Log("You Win 10 Gold");
                textWon.transform.GetComponent<TextMeshProUGUI>().enabled = true;
                textWon.transform.GetComponent<TextMeshProUGUI>().SetText("You Won 10 Coins");
                SpinningWheelManager.gameData.coins += 10;
                break;
            case 90:
                Debug.Log("You Win 1 Chest");
                textWon.transform.GetComponent<TextMeshProUGUI>().enabled = true;
                textWon.transform.GetComponent<TextMeshProUGUI>().SetText("You Won 1 Chest");
                ChestManager.instance.ShowChestOpening();
                break;
            case 135:
                Debug.Log("You Win 5 Diamonds");
                textWon.transform.GetComponent<TextMeshProUGUI>().enabled = true;
                textWon.transform.GetComponent<TextMeshProUGUI>().SetText("You Won 5 Diamonds");
                SpinningWheelManager.gameData.diamonds += 5;
                break;
            case 180:
                Debug.Log("You Win 50 Gold");
                textWon.transform.GetComponent<TextMeshProUGUI>().enabled = true;
                textWon.transform.GetComponent<TextMeshProUGUI>().SetText("You Won 50 Coins");
                SpinningWheelManager.gameData.coins += 50;
                break;
            case 225:
                Debug.Log("You Win 100 Gold");
                textWon.transform.GetComponent<TextMeshProUGUI>().enabled = true;
                textWon.transform.GetComponent<TextMeshProUGUI>().SetText("You Won 100 Coins");
                SpinningWheelManager.gameData.coins += 100;
                break;
            case 270:
                Debug.Log("You Win 1 Chest");
                textWon.transform.GetComponent<TextMeshProUGUI>().enabled = true;
                textWon.transform.GetComponent<TextMeshProUGUI>().SetText("You Won 1 Chest");
                ChestManager.instance.ShowChestOpening();
                break;
            case 315:
                Debug.Log("You Win 30 Gold");
                textWon.transform.GetComponent<TextMeshProUGUI>().enabled = true;
                textWon.transform.GetComponent<TextMeshProUGUI>().SetText("You Won 30 Coins");
                SpinningWheelManager.gameData.coins += 30;
                break;
        }
        coroutineAllowed = true;
        SaveSystem.Save(SpinningWheelManager.gameData);
    }


}
