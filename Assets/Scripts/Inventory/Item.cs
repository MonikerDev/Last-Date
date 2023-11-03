using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public int Quantity { get; set; }
    public float value { get; set; }

    public Item(int qty, float value)
    {
        Quantity = qty;
        this.value = value;
    }
}
