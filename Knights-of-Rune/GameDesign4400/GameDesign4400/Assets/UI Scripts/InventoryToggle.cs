using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [Header("Main Inventory UI Group")]
    public GameObject mainInventoryGroup;

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        mainInventoryGroup.SetActive(isOpen);

        
        Cursor.visible = isOpen;
        Time.timeScale = isOpen ? 0 : 1;
    }
}
