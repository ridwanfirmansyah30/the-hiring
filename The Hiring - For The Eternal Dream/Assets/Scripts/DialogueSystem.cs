using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    // Private

    // Public
    [Header("Options")]
    public Scenario[] scenarios = new Scenario[] {};
    [System.Serializable]
    public class Scenario {
        public enum Cast {
            Player, Opposites
        }
        public Cast cast = new Cast {};

        [TextArea] public string script = string.Empty;
    }

    [Space(10)]
    [Header("Object")]
    public GameObject skipButtonObject;
    public GameObject continueButtonObject;
    public TextMeshProUGUI textContent;

    [Space(10)]
    [Header("Boolean")]
    public bool skipped = false;

    [Space(10)]
    [Header("Number")]
    public int currentScenario = 0;
    public float typingScale = 0.0f;

    [Space(10)]
    [Header("Array")]
    public GameObject[] cast;


    private void Awake() {
        currentScenario = 0;
        textContent.text = string.Empty;
        continueButtonObject.SetActive(false);
        skipped = false;
    }

    private void Start() {
        Dialogue();
    }

    private void Update() {
        skipButtonObject.SetActive(!continueButtonObject.activeSelf);
    }

    public void Skip() {
        skipped = true;
        textContent.text = scenarios[currentScenario].script;
    }

    public void Continue() {
        if (currentScenario < scenarios.Length) {
            Dialogue();
            textContent.text = string.Empty;
            continueButtonObject.SetActive(false);
            skipped = false;
        } else {
            LoadingManager.sceneName = "Game";
            SceneManager.LoadScene("Loading");
        }
    }

    private void Dialogue() {
        for (int i = 0; i < cast.Length; i++) {
            if ((Scenario.Cast)i == scenarios[currentScenario].cast) {
                cast[i].SetActive(true);
            } else {
                cast[i].SetActive(false);
            }
        }

        StartCoroutine(Typing());
    }

    private IEnumerator Typing() {
        yield return new WaitForEndOfFrame();

        foreach (char letter in scenarios[currentScenario].script.ToCharArray()) {
            if (!skipped) {
                textContent.text += letter;
                yield return new WaitForSeconds(typingScale);
            }
        }

        continueButtonObject.SetActive(true);
        currentScenario++;

        yield break;
    }

}
