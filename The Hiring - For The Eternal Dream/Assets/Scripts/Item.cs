using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Private

    // Public
    [Header("Option")]
    public ItemID itemID = new ItemID{};
    public enum ItemID { 
        Empty, Item000, Item001, Item002, Item003
    };

    [Space(10)]
    [Header("Object")]
    public GameObject itemContainer;
    public GameObject[] itemImage;


    private void Update() {
        SetGraphic();
    }

    public void SetGraphic() {
        if (itemID != ItemID.Empty) {
            itemContainer.SetActive(true);

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
        } else {
            itemContainer.SetActive(false);
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

}
