using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Emilia : Unit
{
	//For the Deny move
	private bool denied = false;

	//For the CarefullyLaidPlans move
	public bool plansLaid = false;
	private int turnsPlanning = 0;
	private Unit plannedTarget;

	public Emilia(int bsAtk, int bsDef,
   int bsSpd, int bsHp, int bsStm, int currAtk, int currDef, int currSpd,
   int currHp, int currStm) :
   base("Emi", false, bsAtk, bsDef, bsSpd, bsHp, bsStm,
	   currAtk, currDef, currSpd, currHp, currStm)
	{
		Debug.Log("Created " + charName + " with " + currHp + " HP");
		this.type = UnitType.player;

		this.moves.Add(new Move("Jab", 1, 0));
		this.moves.Add(new Move("Return the Favor", 1, 10));
		this.moves.Add(new Move("Deny", 0, 15));
		this.moves.Add(new Move("Carefully Laid Plans", 1, 10));
	}

	public override Unit ChooseTarget(string targetName)
	{
		foreach (Unit enemy in BattleController.enemies)
		{
			if (enemy.charName == targetName)
			{
				return enemy;
			}
		}
		return null;
	}

	public override void TakeDamage(int damage)
    {
		if (denied)
		{
			DialogLineMapper.QueueLine("Emi holds her resolve!");
			base.TakeDamage(1);
			denied = false;
		}
		else
		{
            base.TakeDamage(damage);
        }
    }

    public override bool Turn(string move, Unit target)
	{
		if (denied)
		{
			denied = false;
		}

		if (!isStunned)
		{
			if (!plansLaid)
			{
				switch (move)
				{
					case "Jab":
						Jab(target);
						break;
					case "Return the Favor":
						ReturnTheFavor(target);
						break;
					case "Deny":
						Deny();
						break;
					case "Carefully Laid Plans":
						CarefullyLaidPlans(target);
						break;
				}
			}
			else
			{
				ExecutePlans();
			}
		}
		else
		{
			this.isStunned = false;
			DialogLineMapper.QueueLine(this.charName + " is stunned...");
		}

		if (isPoisoned)
		{
			DialogLineMapper.QueueLine(this.charName + " is poisoned");
			this.TakeDamage(BattleController.CalculatePoisonDamage(this));

			int roll = Random.Range(0, 100);
			if (roll % 3 == 0)
			{
				this.isPoisoned = false;
				DialogLineMapper.QueueLine(this.charName + " has recovered from poison");
			}
		}

		return true;
	}

	//Free attack
	public void Jab(Unit target)
	{

		int damage = BattleController.CalculateDamage(25, this, target); ;
		DialogLineMapper.QueueLine("Emi gives " + target.charName + " quick jab");
		target.TakeDamage(damage);
	}

	//Low cost
	public void ReturnTheFavor(Unit target)
	{
		this.TakeStamina(10);

		int power = Mathf.RoundToInt(this.bsHp - this.currHp) + 1;

		target.TakeDamage(BattleController.CalculateDamage(power, this, target));
		DialogLineMapper.QueueLine(this.charName + " takes out her current pain on " + target.charName);
	}

	//Med Cost
	public void Deny()
	{
		this.TakeStamina(15);

		if(!this.denied)
		{
			DialogLineMapper.QueueLine("Emi refuses to go down now!");
			this.denied = true;
		}
		else
		{
			DialogLineMapper.QueueLine("Emi is already resolved to stay standing");
		}
	}

	//Low Cost or Med Cost?
	public void CarefullyLaidPlans(Unit target)
	{
		this.TakeStamina(10);

		if (!plansLaid)
		{
			DialogLineMapper.QueueLine("Emi is watching for weaknesses");
			plannedTarget = target;
			plansLaid = true;
		}
		else
		{
			DialogLineMapper.QueueLine("Emi is already planning something...");
		}
	}

	//Medium cost - Takes 3 turns
	public void ExecutePlans()
	{

		if(turnsPlanning < 2)
		{
			this.ChangeStat(-1, StatType.speed);
			this.ChangeStat(1, StatType.defense);
			DialogLineMapper.QueueLine("Emi is watching...");
			turnsPlanning++;
		}
		else if(turnsPlanning == 2)
		{
			DialogLineMapper.QueueLine("Emi found a weakness!");
            int damage = BattleController.CalculateDamage(200, this, plannedTarget);
			plannedTarget.TakeDamage(damage);

			plansLaid = false;
			turnsPlanning = 0;
		}
	}
}
