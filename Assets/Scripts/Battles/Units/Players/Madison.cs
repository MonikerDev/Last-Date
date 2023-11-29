using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Madison : Unit
{
	private bool isAiming;
	private Unit aimedTarget;

	public Madison(int bsAtk, int bsDef,
	int bsSpd, int bsHp, int bsStm, int currAtk, int currDef, int currSpd,
	int currHp, int currStm) :
	base("Madi", false, bsAtk, bsDef, bsSpd, bsHp, bsStm,
		currAtk, currDef, currSpd, currHp, currStm)
	{
		Debug.Log("Created " + charName + " with " + currHp + " HP");
		this.type = UnitType.player;

		this.moves.Add(new Move("Shoot", 1, 0));
		this.moves.Add(new Move("Suppressing Fire", 2, 10));
		this.moves.Add(new Move("Aim", 1, 5));
		this.moves.Add(new Move("Bullseye", 1, 25));
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
				case "Shoot":
					Shoot(target);
					break;
				case "Suppressing Fire":
					SuppressingFire();
					break;
				case "Aim":
					Aim(target);
					break;
				case "Bullseye":
					Bullseye(target);
					break;
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

	public void Shoot(Unit target)
	{

		int damage = BattleController.CalculateDamage(45, this, target);
		Debug.Log("Heather gives " + target.charName + " a sturdy kick in the chest for " + damage + " damage!");
		target.TakeDamage(damage);

		BattleController.targets.Add(target);
	}

	//Dmg def down, hit all, high cost
	public void SuppressingFire()
	{
		this.TakeStamina(20);
		DialogLineMapper.QueueLine(this.charName + " sprays a line of suppressing fire!");

		foreach(Unit target in BattleController.enemies)
		{
			target.TakeDamage(BattleController.CalculateDamage(30, this, target));
			target.ChangeStat(-1, StatType.defense);
			target.ChangeStat(-1, StatType.speed);
			DialogLineMapper.QueueLine(target.charName + " stumbles backwards...");
		}
	}

	//Atk up, stamina up
	public void Aim(Unit target)
	{
		this.TakeStamina(10);
		this.ChangeStat(1, StatType.attack);

		isAiming = true;
		aimedTarget = target;
		DialogLineMapper.QueueLine(this.charName + " takes aim at " + target.charName + "!");
	}

	//Takes one turn to aim, then does
	//Dmg+++ enemy spd and def down
	public void Bullseye(Unit target)
	{
		this.TakeStamina(25);

		if (isAiming)
		{
			aimedTarget.TakeDamage(2 * BattleController.CalculateDamage(200, this, aimedTarget));
			DialogLineMapper.QueueLine(this.charName + " follows through with her aimed shot, hitting a bullseye!");
			this.aimedTarget = null;
			this.isAiming = false;
		}
		else
		{
			target.TakeDamage(BattleController.CalculateDamage(100, this, target));
			DialogLineMapper.QueueLine(this.charName + " hits a bullseye!");
		}
	}
}
