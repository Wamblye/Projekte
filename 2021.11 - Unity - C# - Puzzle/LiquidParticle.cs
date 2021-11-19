using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidParticle : MonoBehaviour
{
    public GameObject gameObj;
    float lifeTime;
    float lifeTimeDuration = 15f;

    void Awake()
    {
        Debug.Log("Awake");
        //Destroy(gameObj, lifetime);
    }

    void OnEnable()
    {
        lifeTime = lifeTimeDuration;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f) 
        {
            gameObject.SetActive (false);
        }
    }

    public void SpawnLiquids(GameObject spawner)
    {
        float randomX = Random.Range(0f, 30f);
        Vector2 pos = new Vector2(spawner.transform.position.x + randomX, spawner.transform.position.y);

        GameObject instance = ObjectPoolingManager.Instance.GetPrefab();
        instance.transform.position = pos;
    }
}
