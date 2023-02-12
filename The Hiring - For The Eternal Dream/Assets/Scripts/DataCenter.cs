using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCenter : MonoBehaviour
{
    // Private

    // Static
    public static bool onCutScene = false;
    public static Vector2 activeSpawnPos = new Vector2();
    public static int stage = 0;

    // Public
    [Space(10)]
    [Header("Array")]
    public Items[] items = new Items[] {};
    public Vector2[] playerSpawnPos = new Vector2[] {};

    [System.Serializable]
    public class Items {
        public ItemID itemID = new ItemID{};
        public enum ItemID { 
            Empty, Item000, Item001, Item002, Item003
        };
    };


    private void Awake() {
        for (int i = 0; i < items.Length; i++) {
            items[i].itemID = 0;
        }

        GameObject[] dataCenterObjects = GameObject.FindGameObjectsWithTag("DataCenter");
        if (dataCenterObjects.Length > 1) Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

}
