using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strider : Unit
{
    public Strider(string name, int bsAtk, int bsDef,
        int bsSpd, int bsHp, int bsStm, int currAtk, int currDef, int currSpd,
        int currHp, int currStm) :
        base(name, false, bsAtk, bsDef, bsSpd, bsHp, bsStm,
        currAtk, currDef, currSpd, currHp, currStm)
    {
        Debug.Log("Created " + charName + " with " + currHp + " HP");
        this.type = UnitType.enemy;
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

    public override bool Turn(string move, Unit target)
    {
		if (!isStunned)
		{

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
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}
