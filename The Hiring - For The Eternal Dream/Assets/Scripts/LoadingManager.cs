using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static string sceneName = string.Empty;
    //public Image background;


    void Start() {
        StartCoroutine(LoadAsynchronously());
    }

    private IEnumerator LoadAsynchronously() {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        /*while(!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            background.color = new Color(0, 0, 0, progress);
            yield return null;
        }*/

        yield return null;
    }
}
