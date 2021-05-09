using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject toolbarUI;
    public GameObject toolUI;

    private GameObject toolSoundSource;
    public AudioSource audioSource;

    private Dictionary<int, GameObject> toolUIObjects;

    private void Awake()
    {
        toolSoundSource = new GameObject();
        toolSoundSource.transform.parent = this.transform;
        toolSoundSource.name = "ToolSoundSource";
        audioSource = toolSoundSource.AddComponent<AudioSource>();
    }

    private void Start()
    {
        inventory = new Dictionary<int, Tool>();
        toolUIObjects = new Dictionary<int, GameObject>();

        for (int i = 1; i <= maxSize; i++)
        {
            inventory.Add(i, null);
            toolUIObjects.Add(i, Instantiate(toolUI));
            toolUIObjects[i].transform.parent = toolbarUI.transform;
            toolUIObjects[i].SetActive(false);
        }

        foreach (Tool tool in transform.GetComponentsInChildren<Tool>())
        {
            Pickup(tool);
        }
    }

    private void ToggleHighlightToolUI(GameObject slot, bool state)
    {
        Transform highlight = slot.transform.Find("Highlight");
        highlight.gameObject.SetActive(state);
    }

    private void UpdateToolUI(int id, Tool tool)
    {
        GameObject toolUI = toolUIObjects[id];
        toolUI.SetActive(true);

        Transform idUI = toolUI.transform.Find("NumID");
        Transform nameUI = toolUI.transform.Find("Name");
        Transform imageUI = toolUI.transform.Find("Image");
        Transform highlightUI = toolUI.transform.Find("Highlight");


        Text idText = idUI.GetComponent<Text>();
        idText.text = id.ToString();

        Text nameText = nameUI.GetComponent<Text>();
        nameText.text = tool.transform.name;

        Image image = imageUI.GetComponent<Image>();
        image.sprite = tool.icon;

        Image highlight = highlightUI.GetComponent<Image>();
        highlight.sprite = tool.highlight;

        ToggleHighlightToolUI(toolUIObjects[id], true);
    }



    public void UnequipEquipped()
    {
        if (equippedTool != null)
        {
            equippedTool.OnUnequip();
            equippedTool.gameObject.SetActive(false);
            equippedTool = null;

            ToggleHighlightToolUI(toolUIObjects[equippedToolIndex], false);

            equippedToolIndex = -1;
        }
    }

    public void Equip(int index)
    {
        if (inventory[index] != null)
        {
            bool equip = equippedToolIndex != index;

            if (equippedTool != null)
            {
                UnequipEquipped();
            }

            if (equip)
            {
                inventory[index].gameObject.SetActive(true);
                inventory[index].transform.parent = Camera.main.transform; // TODO finding the camera should be improved. What if we have multiple cameras?
                inventory[index].Equip();
                equippedTool = inventory[index];
                equippedToolIndex = index;

                ToggleHighlightToolUI(toolUIObjects[equippedToolIndex], true);

            }
        } 
    }

    public void Pickup(Tool newTool)
    {
        if (!inventory.ContainsValue(newTool)) {
            foreach (KeyValuePair<int, Tool> pair in inventory)
            {
                if (pair.Value == null)
                {
                    UpdateToolUI(pair.Key, newTool);
                    inventory[pair.Key] = newTool;
                    newTool.Pickup(transform.parent.gameObject, this);

                    int key = pair.Key;
                    Equip(key);

                    return;
                }
            }
        }
    }

    public void DropEquipped()
    {
        if (equippedTool != null)
        {
            ToggleHighlightToolUI(toolUIObjects[equippedToolIndex], false);

            toolUIObjects[equippedToolIndex].SetActive(false);

            equippedTool.gameObject.SetActive(true);
            equippedTool.Drop();
            equippedTool.transform.parent = null;

            inventory[equippedToolIndex] = null;
            equippedToolIndex = -1;

            equippedTool = null;
        }
    }

    private void Update()
    {
        if (equippedTool != null)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                equippedTool.StartAction();
            }

            if (Input.GetButton("Fire1"))
            {
                equippedTool.ContinueAction();
            }

            if (Input.GetButtonUp("Fire1"))
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
