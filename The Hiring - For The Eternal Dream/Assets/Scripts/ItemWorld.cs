using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    // Private

    // Public
    [Header("Option")]
    public ItemID itemID = new ItemID{};
    public enum ItemID { 
        Item000, Item001, Item002
    };

    [Space(10)]
    [Header("Array")]
    public GameObject[] itemImage = new GameObject[] {};


    private void Update() {
        switch (itemID) {
            case ItemID.Item000:
                SelectedItem(0);
            break;
            case ItemID.Item001:
                SelectedItem(1);
            break;
            case ItemID.Item002:
                SelectedItem(2);
            break;
        }
    }

    private void SelectedItem(int wich) {
        for (int i = 0; i < itemImage.Length; i++) {
            if (i == wich) {
                itemImage[i].SetActive(true);
            } else {
                itemImage[i].SetActive(false);
            }
        }
    }

    public void DestroyThisObject() {
        Destroy(this.gameObject);
    }
}
