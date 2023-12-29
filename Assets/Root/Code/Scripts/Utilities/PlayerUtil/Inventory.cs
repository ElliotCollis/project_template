using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HowlingMan
{
    public class Inventory
    {

        public Inventory ()
        {
            inventoryItems = new List<InventoryItem>();
        }

        public Inventory (int[] instanceIds)
        {
            inventoryItems = new List<InventoryItem>();
            PopulateInventory(instanceIds);
        }

        List<InventoryItem> inventoryItems;

        public List<InventoryItem> InventoryItems
        {
            get
            {
                return inventoryItems;
            }
        }

        public void PopulateInventory  (int[] instanceIds)
        {
            foreach (var id in instanceIds)
            {
                inventoryItems.Add(new InventoryItem(id));
            }
        }

        public InventoryItem GetInventoryItem(int index)
        {
            InventoryItem item = inventoryItems[index];
            inventoryItems.Remove(item);
            return item;
        }

        public InventoryItem GetInventoryItem(InventoryItem item)
        {
            inventoryItems.Remove(item);
            return item;
        }

        public void AddInventoryItem (InventoryItem item)
        {
            inventoryItems.Add(item);
        }
    }

    public class InventoryItem
    {
        public int instanceID;
        public string itemName;
        // type, use, and reference.

        public InventoryItem(int id)
        {
            instanceID = id;
        }

        public InventoryItem(int id, string name)
        {
            SetItemInformation(id, name);
        }

        public void SetItemInformation(int id, string name)
        {
            instanceID = id;
            itemName = name;
        }
    }
}
