using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots;
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatNames;
    public TextMeshProUGUI selectedItemStatValues;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex;

    //components
    private PlayerController controller;
    private PlayerNeeds needs;

    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    //singleton
    public static Inventory instance;

    void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
        needs = GetComponent<PlayerNeeds>();
    }

    void Start()
    {
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uiSlots.Length];

        //initialize the slots
        for(int x = 0; x < slots.Length; x++)
        {
            slots[x] = new ItemSlot();
            uiSlots[x].index = x;
            uiSlots[x].Clear();
        }

        ClearSelectedItemWindow();
    }

    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
            onCloseInventory.Invoke();
            controller.ToggleCursor(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
            onOpenInventory.Invoke();
            ClearSelectedItemWindow();
            controller.ToggleCursor(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem(ItemData item)
    {
        if (item.canStack)
        {
            ItemSlot slotToStackTo = GetItemStack(item);

            if(slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if(emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }
        ThrowItem(item);
    }

    void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360.0f));
    }

    void UpdateUI()
    {
        for(int x = 0; x < slots.Length; x++)
        {
            if(slots[x].item != null)
            {
                uiSlots[x].Set(slots[x]);
            }
            else
            {
                uiSlots[x].Clear();
            }
        }
    }

    ItemSlot GetItemStack(ItemData item)
    {
        for(int x = 0; x < slots.Length; x++)
        {
            if(slots[x].item == item && slots[x].quantity < item.maxStackAmount)
            {
                return slots[x];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for(int x = 0; x < slots.Length; x++)
        {
            if(slots[x].item == null)
            {
                return slots[x];
            }
        }
        return null;
    }

    public void SelectItem(int index)
    {
        if(slots[index].item == null)
        {
            return;
        }

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        //set stat values and stat names
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;

        for(int x = 0; x<selectedItem.item.consumables.Length; x++)
        {
            selectedItemStatNames.text += selectedItem.item.consumables[x].type.ToString() + "\n";
            selectedItemStatNames.text += selectedItem.item.consumables[x].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    void ClearSelectedItemWindow()
    {
        //clear the text elements
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;

        //disable all buttons
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void OnUseButton()
    {
        if(selectedItem.item.type == ItemType.Consumable)
        {
            for(int x = 0; x < selectedItem.item.consumables.Length; x++)
            {
                float consumableValue = selectedItem.item.consumables[x].value;
                switch (selectedItem.item.consumables[x].type)
                {
                    case ConsumableType.Health: needs.Heal(consumableValue); 
                        break;
                    case ConsumableType.Hunger: needs.Eat(consumableValue);
                        break;
                    case ConsumableType.Thirst: needs.Drink(consumableValue);
                        break;
                    case ConsumableType.Sleep: needs.Sleep(consumableValue);
                        break;
                }
            }
        }
        RemoveSelectedItem();
    }

    public void OnEquipButton()
    {

    }

    public void OnUnEquipButton()
    {

    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    void UnEquip (int index)
    {

    }

    void RemoveSelectedItem()
    {
        selectedItem.quantity--;
        if(selectedItem.quantity == 0)
        {
            if(uiSlots[selectedItemIndex].equipped == true)
            {
                UnEquip(selectedItemIndex);
            }
            selectedItem.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public void RemoveItem(ItemData item)
    {

    }

    public bool HasItems(ItemData item, int quantity)
    {
        return false;
    }
}

public class ItemSlot
{
    public ItemData item;
    public int quantity;
}