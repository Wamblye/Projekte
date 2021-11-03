using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SpawnPrefab : MonoBehaviour
{

    public GameObject gameObj;

    public void Spawn(int x, int y)
    {
        Vector2 pos = new Vector2(x, y);
        GameObject instance = Instantiate(gameObj);

        instance.transform.SetParent(GameObject.FindGameObjectWithTag("GameSpaceTag").transform, false);
        instance.transform.position = pos;
    }

    public void Spawn(string tag, int x, int y)
    {
        GameObject ob = GameObject.FindGameObjectWithTag(tag);
        if (ob == null) 
        {
            Vector2 pos = new Vector2(x, y);
            GameObject instance = Instantiate(gameObj);

            instance.transform.SetParent(GameObject.FindGameObjectWithTag("GameSpaceTag").transform, false);
            instance.tag = tag;
            instance.transform.position = pos;
        }
    }

    public void SpawnOfflinePopup(int x, int y, string text)
    {
        Vector2 pos = new Vector2(x, y);
        GameObject instance = Instantiate(gameObj);

        instance.transform.SetParent(GameObject.FindGameObjectWithTag("GameSpaceTag").transform, false);
        instance.transform.position = pos;
        instance.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(text);
    }

    public void SetTextPrefabChild(int leafs)
    {
        gameObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(leafs.ToString());
    }

    public void DestroyPrefab()
    {
        Destroy(gameObj);
    }

}
