using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Author: Bryan Dedeurwaerder
 * Project: Little Chits
 * 
 * Description: 
 */

public class InventorySystem : MonoBehaviour
{
    public Dictionary<int, Tool> inventory;
    public int maxSize = 9;

    //public GameObject body;
    private Tool equippedTool;
    private int equippedToolIndex;

    private void Start()
    {
        inventory = new Dictionary<int, Tool>();
        foreach (Tool tool in transform.GetComponentsInChildren<Tool>())
        {
            Pickup(tool);
        }
    }

    public void Pickup(Tool newTool)
    {
        for (int i = 1; i < maxSize; i++)
        {
            if (!inventory.ContainsKey(i))
            {
                inventory.Add(i, newTool);
                newTool.OnPickup(transform.parent.gameObject);
                Equip(i);
                break;
            }
        }
    }

    public void DropEquipped()
    {
        if (equippedTool != null)
        {
            equippedTool.gameObject.SetActive(true);
            equippedTool.OnDrop();
            equippedTool.transform.parent = null;

            inventory.Remove(equippedToolIndex);

            equippedTool = null;
        }
    }

    public void UnequipEquipped()
    {
        if (equippedTool)
        {
            equippedTool.OnUnequip();
            equippedTool.gameObject.SetActive(false);
            equippedTool = null;
        }
    }

    public void Equip(int index)
    {
        if (inventory.ContainsKey(index))
        {
            if (equippedTool != null)
            {
                UnequipEquipped();
            }

            inventory[index].gameObject.SetActive(true);
            inventory[index].transform.parent = Camera.main.transform; // TODO finding the camera should be improved. What if we have multiple cameras?
            inventory[index].OnEquip();
            equippedTool = inventory[index];
            equippedToolIndex = index;
        }
        else
        {
            Debug.Log("Trying to access a tool that does not exist");
        }

    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (equippedTool != null)
            {
                equippedTool.StartAction();
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (equippedTool != null)
            {
                equippedTool.EndAction();
            }
        }

        foreach (KeyValuePair<int, Tool> pair in inventory)
        {
            if (pair.Key < 10)
            {
                if (Input.GetButtonDown("Tool" + pair.Key.ToString()))
                {
                    Equip(pair.Key);
                }
            }
        }

        if (Input.GetButtonDown("Drop"))
        {
            DropEquipped();
        }
    }

}
