using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour
{
    // Verweis auf das GameObject im Prefab Ordner "ClickPopup_Holder"
    public GameObject gameObj;

    public void showTextPopup(string text)
    {
        GameObject clickPopupInstance = Instantiate(gameObj);
        // Position innerhalb des Canvas setzen 
        clickPopupInstance.transform.SetParent(GameObject.FindGameObjectWithTag("GameSpaceTag").transform, false);
        // Text ändern
        clickPopupInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(text);

        // Position nach Touch Input
        Vector2 randomPos = randomPosition();
        clickPopupInstance.transform.position = randomPos;
    }
    public void showGoldPopup(Vector2 pos)
    {
        GameObject clickPopupInstance = Instantiate(gameObj);
        // Position innerhalb des Canvas setzen 
        clickPopupInstance.transform.SetParent(GameObject.FindGameObjectWithTag("GameSpaceTag").transform, false);
        clickPopupInstance.transform.position = pos;
    }

      private Vector2 randomPosition() {
        int randomX = Random.Range(250, 820);
        int randomY = Random.Range(700, 1300);

        Vector2 pos = new Vector2(randomX, randomY);
        return pos;
    }

}
