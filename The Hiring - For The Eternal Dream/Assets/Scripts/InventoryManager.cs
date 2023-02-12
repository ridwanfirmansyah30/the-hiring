using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Private
    private DataCenter dataCenter;

    // Public
    [Header("Object")]
    public GameObject itemObject;
    public Transform itemContainer;

    [Space(10)]
    [Header("Array")]
    public List<Item> items = new List<Item>();


    private void Awake() {
        dataCenter = GameObject.FindGameObjectWithTag("DataCenter").GetComponent<DataCenter>();
        
        while (items.Count < dataCenter.items.Length) {
            items.Add(Instantiate(itemObject, itemContainer.position, Quaternion.identity, itemContainer).GetComponent<Item>());
        }

        SynchronizingData();
    }

    private void Update() {
        SynchronizingData();
    }

    private void SynchronizingData() {
        for (int i = 0; i < dataCenter.items.Length; i++) {
            for (int j = 0; j < items.Count; j++) {
                if (i == j) {
                    items[j].itemID = (Item.ItemID)dataCenter.items[i].itemID;
                }

                items[j].SetGraphic();
            }
        }
    }
}
