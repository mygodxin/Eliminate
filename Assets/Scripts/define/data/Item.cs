using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Item
{
    [SerializeField]
    public ItemDef type;
    [SerializeField]
    public int count;
    public Item(ItemDef type, int count)
    {
        this.type = type;
        this.count = count;
    }
}

[Serializable]
public class ItemWrap
{
    public List<Item> itemList = new List<Item>();
    public ItemWrap()
    {
    }

    public void init()
    {
        itemList.Add(new Item(ItemDef.SAME_BALL, 0));
        itemList.Add(new Item(ItemDef.LIGHTNING, 0));
        itemList.Add(new Item(ItemDef.HAMMER, 0));
    }

    public int getItemCount(ItemDef type)
    {
        foreach (var item in itemList)
        {
            if (item.type == type)
            {
                return item.count;
            }
        }
        return 0;
    }

    public void addItemCount(ItemDef type, int count)
    {
        foreach (var item in itemList)
        {
            if (item.type == type)
            {
                item.count += count;
            }
        }
    }
}