using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemDefault> items = new List<ItemDefault>();
    public GameObject playertag;
    public bool canAdd()
    {
        if (items.Count < 3)
        {
            return true;
        }
        foreach (ItemDefault item in items) {
            if (item == null)
            {
                return true ;
            }
        }
        return false;
    }
}
public enum itemType
{
    None,
    red,
    blue,
    green,
}
