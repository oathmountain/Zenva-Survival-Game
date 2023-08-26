using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "New Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public ItemData itemToCraft;
    public ResourceCost[] cost;
}

[Serializable]
public class ResourceCost
{
    public ItemData item;
    public int quantity;
}
