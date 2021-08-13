using System.Collections;
using System.Collections.Generic;
using peroth;
using UnityEngine;

public class AddItemInGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InventoryManager.instance.AddItem(ItemCode.ItemA);
        InventoryManager.instance.ResetInventory();
        InventoryManager.instance.SaveItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
