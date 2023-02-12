using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Private

    // Public
    [Header("String")]
    [TextArea] public string information;

    [Space(10)]
    [Header("Array")]
    public List<Attachment> attachment = new List<Attachment>() {};

    [System.Serializable]
    public class Attachment {
        public GameObject item;
        public Transform spawnPoint;
    }


    public void SpawnItem() {
        Instantiate(
            attachment[0].item, 
            attachment[0].spawnPoint.position, 
            Quaternion.identity, 
            this.transform
        ).GetComponent<ItemWorld>().itemID = (ItemWorld.ItemID)Random.Range(0, 3);

        attachment.Clear();
    }
}