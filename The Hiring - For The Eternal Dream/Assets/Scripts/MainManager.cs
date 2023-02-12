using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class MainManager : MonoBehaviour
{
    // Private

    // Public
    [Header("Object")]
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public CinemachineConfiner2D cinemachineConfiner2D;
    public PlayerController playerController;
    public GameObject gameplayUI;
    public GameObject interactUI;
    public GameObject pausedUI;
    public GameObject instructionOnPause;
    public GameObject inventory;
    public GameObject instruction;

    [Space(10)]
    [Header("Boolean")]
    public bool camAnimFinished = false;

    [Space(10)]
    [Header("Number")]
    public float transitionSpeed = 0.0f;
    public float defaultOrthoSizeCam = 0.0f;
    public float onCutSceneOrthoSizeCam = 0.0f;

    [Space(10)]
    [Header("Array")]
    public GameObject[] maps;
    public GameObject[] interactButtonIcons;


    private void Awake() {
        if (DataCenter.onCutScene) {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = onCutSceneOrthoSizeCam;
        } else {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = defaultOrthoSizeCam;
        }

        interactUI.SetActive(false);
        cinemachineConfiner2D.InvalidateCache();

        for (int i = 0; i < maps.Length; i++) {
            if (i == DataCenter.stage) {
                maps[i].SetActive(true);
            } else {
                maps[i].SetActive(false);
            }
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseOrContinue();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            Inventory();
        }

        if (DataCenter.onCutScene) {
            gameplayUI.SetActive(false);
            pausedUI.SetActive(false);

            if (cinemachineVirtualCamera.m_Lens.OrthographicSize >= defaultOrthoSizeCam) {
                camAnimFinished = true;
            } else {
                cinemachineVirtualCamera.m_Lens.OrthographicSize += Time.deltaTime * TransitionSpeed();
            }
        }

        if (pausedUI.activeSelf) {
            switch (SystemInfo.deviceType) {
                case DeviceType.Handheld or DeviceType.Unknown:
                    instructionOnPause.SetActive(false);
                break;
                case DeviceType.Desktop:
                    instructionOnPause.SetActive(true);
                break;
            }
        }
    }

    private float TransitionSpeed() {
        if (transitionSpeed >= 0.1f) {
            return transitionSpeed;
        } else {
            return 0.1f;
        }
    }

    public void SetGameplayUI(bool dynamic) {
        gameplayUI.SetActive(dynamic);

        switch (SystemInfo.deviceType) {
            case DeviceType.Unknown or DeviceType.Handheld:
                gameplayUI.SetActive(dynamic);
            break;
            case DeviceType.Desktop:
                gameplayUI.SetActive(false);
            break;
        }
    }

    public void PauseOrContinue() {
        if (pausedUI.activeSelf) {
            SetGameplayUI(true);
            Time.timeScale = 1.0f;
            pausedUI.SetActive(false);
        } else {
            SetGameplayUI(false);
            Time.timeScale = 0.0f;
            pausedUI.SetActive(true);
        }
    }

    public void MainMenu() {
        LoadingManager.sceneName = "MainMenu";
        SceneManager.LoadScene("Loading");
        Time.timeScale = 1.0f;
    }

    public void Inventory() {
        if (inventory.activeSelf) {
            inventory.SetActive(false);
        } else {
            inventory.SetActive(true);
        }
    }

    public void CloseButton(GameObject attachedGameObject) {
        attachedGameObject.SetActive(false);
    }
}
