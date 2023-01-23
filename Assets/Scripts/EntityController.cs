using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public EntityAttributes attributes = new EntityAttributes();
    public List<MonoBehaviour> dependencies = new List<MonoBehaviour>();
    public ActionRadiusController actionRadiusController;
    public List<AppendageController> appendages = new List<AppendageController>();

    public List<InventoryItem> inventory = new List<InventoryItem>();
    public bool CanPickUpItems = false;

    public void Register<T>(T component) where T : MonoBehaviour
    {
        switch (component)
        {
            case ActionRadiusController ar:
                actionRadiusController = ar;
                break;
            case AppendageController ap:
                appendages.Add(ap);
                break;
            default:
                dependencies.Add(component);
                break;
        }
    }

    public void Unregister<T>(T component) where T : MonoBehaviour
    {
        switch (component)
        {
            case ActionRadiusController ar:
                actionRadiusController = null;
                break;
            case AppendageController ap:
                appendages.Remove(ap);
                break;
            default:
                dependencies.Remove(component);
                break;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        attributes.curHealth -= damage;
        if (attributes.curHealth <= 0)
        {
            DestroyAll();
        }
    }

    public virtual void DestroyAll()
    {
        foreach (var appendage in appendages.ToList())
        {
            if (appendage != null)
            {
                appendage.DestroyAll();
            }
        }
        Destroy(gameObject);
    }

    public bool PickUpItem(Pickupable item)
    {
        if (!CanPickUpItems) return false;
        var inventoryItem = new InventoryItem();
        inventoryItem.displayName = item.pickupObj.name;
        inventoryItem.quantity = item.amount;
        inventoryItem.pickupObj = item.pickupObj;
        AddToInventory(inventoryItem);
        return true;
    }

    private void AddToInventory(InventoryItem item)
    {
        if (inventory.Any(i => i.pickupObj == item.pickupObj))
        {
            inventory.First(i => i.pickupObj == item.pickupObj).quantity += item.quantity;
        }
        else
        {
            inventory.Add(item);
        }
    }
}

[Serializable]
public class EntityAttributes
{
    public string name;
    public List<string> tags = new List<string>();
    public int curLevel;
    public int curExperience;
    public int expToLevel;
    public int curHealth;
    public int maxHealth;
    public int actionSpeed;
    public int moveSpeed;
    public int attackSpeed;

    public int strength;
    public int dexterity;
    public int intelligence;
    public int luck;

    public int physRes;
    public int elemRes;

    public EntityAttributes()
    {

    }
}

[Serializable]
public class InventoryItem
{
    public string displayName;
    public int quantity;
    public PickupSO pickupObj;
}