using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleItem : Item
{
	public BattleItem(int qty, float value) : base(qty, value)
	{
	}



	//Removes 1 quanitity when used,
	public virtual void Effect(List<Unit> target)
    {
        Debug.Log("Item used");
        this.Quantity--;

        if(this.Quantity == 0)
        {
            GlobalVariableStorage.items.Remove(this);
        }
    }
}
