using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asheton : Unit
{
    //Backstab Variables
    public bool isSneaking = false;
    Unit backstabTarget;

    public Asheton(int bsAtk, int bsDef,
        int bsSpd, int bsHp, int bsStm, int currAtk, int currDef, int currSpd,
        int currHp, int currStm) :
        base("Ashe", false, bsAtk, bsDef, bsSpd, bsHp, bsStm,
        currAtk, currDef, currSpd, currHp, currStm)
    {
        Debug.Log("Created " + charName + " with " + currHp + " HP");
        this.type = UnitType.player;

        this.moves.Add(new Move("Knife Trick", 1, 0));
        this.moves.Add(new Move("Back Stab", 1, 10));
        this.moves.Add(new Move("Pin Down", 1, 5));
        this.moves.Add(new Move("Hire", -1, 20));
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
			if (!isSneaking)
			{
                switch (move)
                {
                    case "Knife Trick":
                        KnifeTrick(target);
                        break;
                    case "Back Stab":
                        BackStab(target);
                        break;
                    case "Pin Down":
                        PinDown(target);
                        break;
                    case "Hire":
                        Hire(target);
                        break;
                }
			}
			else
			{
				BackStab(target);
			}
		}
		else
		{
			this.isStunned = false;
            if (this.isSneaking) { this.isSneaking = false; }
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
    public override void TakeDamage(int damage)
    {
        if (!isSneaking)
        {
            base.TakeDamage(damage);
        }
        else
        {
            DialogLineMapper.QueueLine("Ashe has vanished from the battlefield...");
        }
    }

    //Free base damage
    public void KnifeTrick(Unit target)
    {

        int damage = BattleController.CalculateDamage(15, this, target);


		target.TakeDamage(damage);

        DialogLineMapper.QueueLine("Asheton flashes her blades at " + target.charName + " cutting them for " + damage + " damage.");

        BattleController.targets.Add(target);
    }

    //Damage++ / med cost
    public void BackStab(Unit target)
    {

        if (!isSneaking)
        {
            this.TakeStamina(10);

            DialogLineMapper.QueueLine("Ashe vanishes in the shadows...");
            backstabTarget = target;
            isSneaking = true;
        }
        else
        {
            int damage = BattleController.CalculateDamage(150, this, backstabTarget);
            backstabTarget.TakeDamage(damage);

            DialogLineMapper.QueueLine("Ashe stabs " + backstabTarget.charName + " in the back for " + damage + " damage");
            isSneaking = false;

            BattleController.targets.Add(backstabTarget);
        }
	}

    //Damage Spd down
    public void PinDown(Unit target)
    {
        this.TakeStamina(5);

        DialogLineMapper.QueueLine("Ashe puts pressure on " + target.charName + " preventing them from moving");
		int damage = BattleController.CalculateDamage(50, this, target);

		target.ChangeStat(-1, StatType.speed);
        target.TakeDamage(damage);
    }

    //Atk up, speed up, gives target another turn
    public void Hire(Unit target)
    {
        this.TakeStamina(20);

        DialogLineMapper.QueueLine(this.charName + " offers " + target.charName + " compensation to handle this for her...");
        DialogLineMapper.QueueLine(target.charName + " is better equipped to handle the situation");

        target.ChangeStat(1, StatType.attack);
        target.ChangeStat(1, StatType.speed);
        target.TakeStamina(-10);
    }
}
