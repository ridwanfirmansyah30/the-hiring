using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackgroundManager : MonoBehaviour
{
    // Public
    [Header("Setting")]
    public bool run = false;

    [Space(10)]
    [Header("Object")]
    public Transform lightBackground;

    [Space(10)]
    [Header("Number")]
    public float standardFogSize = 0.0f;
    public float standardLightSize = 0.0f;

    [Space(10)]
    [Header("Array")]
    public Transform[] fogBackgrounds;


    private void Update() {
        if (run) {
            var camWidth = Camera.main.scaledPixelWidth;
            var camHeight = Camera.main.scaledPixelHeight;
            
            // Synch background scale with camera
            for (int i = 0; i < fogBackgrounds.Length; i++) {
                fogBackgrounds[i].localScale = new Vector2(camWidth / standardFogSize, camHeight / standardFogSize);
            }

            lightBackground.localScale = new Vector2(camWidth / standardLightSize, camHeight / standardLightSize);
        }
    }
}
