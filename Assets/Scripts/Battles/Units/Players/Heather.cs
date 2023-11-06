using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heather : Unit
{
	public Heather(int bsAtk, int bsDef,
	int bsSpd, int bsHp, int bsStm, int currAtk, int currDef, int currSpd,
	int currHp, int currStm) :
	base("Heather", false, bsAtk, bsDef, bsSpd, bsHp, bsStm,
		currAtk, currDef, currSpd, currHp, currStm)
	{
		Debug.Log("Created " + charName + " with " + currHp + " HP");
		this.type = UnitType.player;

		this.moves.Add(new Move("Kick", 1, 0));
		this.moves.Add(new Move("Ramp Up", 0, 10));
		this.moves.Add(new Move("Help Up", -1, 15));
		this.moves.Add(new Move("AxeKick", 1, 25));

	}

	public override Unit ChooseTarget(string targetName)
	{
		foreach (Unit unit in BattleController.battlers)
		{
			if (unit.charName == targetName)
			{
				return unit;
			}
		}
		return null;
	}

	public override bool Turn(string move, Unit target)
	{
		if (!isStunned)
		{
            switch (move)
            {
				case "Kick":
					Kick(target);
					break;
				case "Ramp Up":
					RampUp();
					break;
				case "Help Up":
					HelpUp(target);
					break;
				case "AxeKick":
					AxeKick(target);
					break;
            }
		}
		else
		{
			this.isStunned = false;
			DialogLineMapper.QueueLine(this.charName + " is stunned...");
		}

		if(isPoisoned)
		{
			DialogLineMapper.QueueLine(this.charName + " is poisoned");
			this.TakeDamage(BattleController.CalculatePoisonDamage(this));

			int roll = Random.Range(0, 100);
			if(roll % 3 == 0)
			{
				this.isPoisoned = false;
				DialogLineMapper.QueueLine(this.charName + " has recovered from poison");
			}
		}

		return true;
	}
	public override void TakeDamage(int damage)
	{
		base.TakeDamage(damage);
	}

	//Free damage move
	public void Kick(Unit target)
	{

        int damage = BattleController.CalculateDamage(15, this, target);
		DialogLineMapper.QueueLine("Heather gives " + target.charName + " a sturdy kick in the chest for " + damage + " damage!");
		target.TakeDamage(damage);
	}

	//Low cost, spd up
	public void RampUp()
	{
		this.TakeStamina(10);
		DialogLineMapper.QueueLine("Heather hypes herself up");
		this.ChangeStat(1, StatType.speed);
	}

	//Gives an ally spd up and restores stamina to them
	public void HelpUp(Unit target)
	{
		this.TakeStamina(15);
		DialogLineMapper.QueueLine("Heather offers " + target.charName + " a helping hand");
		target.ChangeStat(1, StatType.speed);
	}

	//Deals damage++ has chance to damage self???
	public void AxeKick(Unit target)
	{
		this.TakeStamina(25);

		DialogLineMapper.QueueLine("Heather swings down her leg with absolute force");
		target.TakeDamage(BattleController.CalculateDamage(150, this, target));

		//20% chance to take damage
		int rand = Random.Range(0, 5);

		if(rand == 0)
		{
			DialogLineMapper.QueueLine("The impact sends a strong jolt up heather's leg");
			this.TakeDamage(BattleController.CalculateDamage(10, this, this));
		}
	}
}
