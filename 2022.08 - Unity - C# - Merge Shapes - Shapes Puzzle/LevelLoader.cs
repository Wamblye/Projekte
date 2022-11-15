using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    // buildNumber muss jedes mal um 1 erhöht werden, wenn eine neue Scene
    // im Buildindex hinzugefügt worden ist und diese vor den Leveln sich befindet
    public static int buildNumber = 7;
    public static int buildNumberPerfekteKlicks = 111;
    public Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
            return;
        }
    }


    public void LoadLevel(int sceneIndex)
    {
        if (animator != null) 
        {
            animator.Play("Load");
        }
        Scene currentLevelScene = SceneManager.GetActiveScene();
        int index = currentLevelScene.buildIndex;

        if (index != sceneIndex)
        {
            StartCoroutine(LoadDelay(sceneIndex));
        }
    }


    IEnumerator LoadDelay(int sceneIndex)
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!operation.isDone)
        {
            yield return null;
        }
    }



}
