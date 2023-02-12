using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutageManager : MonoBehaviour
{
    // Private
    private bool powerDown = false;

    // Public
    [Header("Object")]
    public AudioSource bgm;

    [Space(10)]
    [Header("Array")]
    public AudioSource[] outageStepSounds;


    private void Awake() {
        // Prevent from accidently turned true/false boolean
        powerDown = false;
    }

    private void Update() {
        //Audio Failure Animation Procedural
        if (powerDown) {
            bgm.pitch = 1.0f;
            bgm.Stop();
        } else {
            float randomVal = Random.Range(-3.0f, 1.0f);
            bgm.pitch = randomVal;
        }
    }

    //Outage Animation Procedural
    public void PlayStep0() {
        outageStepSounds[0].Play();
    }

    public void PlayStep1() {
        outageStepSounds[1].Play();
    }

    public void PlayStep2() {
        outageStepSounds[2].Play();
    }

    public void PowerDown() {
        outageStepSounds[3].Play();
        powerDown = true;
    }
    
    //Loading a Scene
    public void Loading() {
        LoadingManager.sceneName = "Dialogue";
        SceneManager.LoadScene("Loading");
    }
}
