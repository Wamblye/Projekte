using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance { get { return instance; } }

    public GameObject prefab;
    public int amount;

    private List<GameObject> listGameobj;

    void Awake()
    {
        instance = this;

        listGameobj = new List<GameObject>(amount);

        for (int i = 0; i < amount; i++)
        {
            GameObject prefabInstance = Instantiate(prefab);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);

            listGameobj.Add(prefabInstance);
        }
    }

    public GameObject GetPrefab ()
    {
        foreach (GameObject gameObj in listGameobj) 
        {
            if (!gameObj.activeInHierarchy) 
            {
                gameObj.SetActive (true);        
                return gameObj;
            }
        }

        // Wenn alle Objekte aus der Liste (oben) schon in Benutzung, neue
        // Instance erzeugen und der Liste hinzufügen
        GameObject prefabInstance = Instantiate (prefab);
        prefabInstance.transform.SetParent (transform);    
        listGameobj.Add (prefabInstance);
        return prefabInstance;
    }


}
