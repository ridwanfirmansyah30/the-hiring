using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Private
    private DataCenter dataCenter;

    // Public
    [Header("Object")]
    public GameObject outagePanel;
    public GameObject playButton;


    private void Awake() {
        dataCenter = GameObject.FindGameObjectWithTag("DataCenter").GetComponent<DataCenter>();

        // Prevent from accidently turned on/off gameObject
        outagePanel.SetActive(false);
        playButton.SetActive(true);
    }

    public void Play() {
        // Activating and also running animation
        outagePanel.SetActive(true);
        playButton.SetActive(false);
        DataCenter.onCutScene = true;
        DataCenter.stage = 0;
        DataCenter.activeSpawnPos = dataCenter.playerSpawnPos[0];
    }
}
