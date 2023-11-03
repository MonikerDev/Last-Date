using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Shambler : Unit
{
    Unit currTarget;

    public Shambler(string name, int bsAtk, 
        int bsDef, int bsSpd, int bsHp, int bsStm, int currAtk, 
        int currDef, int currSpd, int currHp, int currStm) : 
        base(name, false, bsAtk, bsDef, bsSpd, bsHp, bsStm, 
            currAtk, currDef, currSpd, currHp, currStm)
    {
        Debug.Log("Created " + charName + " with " + currHp + " HP");
        this.type = UnitType.enemy;
    }

    public override Unit ChooseTarget(string unit)
    {
        List<Unit> opponents = BattleController.players;

        //Shamblers will focus the slowest character down
        if(opponents.Count <= 0)
        {
            Debug.LogWarning("No Units found");
        }
        else if(opponents.Count == 1)
        {
            currTarget = opponents[0];
        }
        else if(opponents.Count > 1)
        {
            for(int i = 0; i < opponents.Count; i++)
            {
                Unit temp = opponents[i];

                for(int j = i + 1; j < opponents.Count; j++)
                {
                    if(temp.currSpd < opponents[j].currSpd)
                    {
                        opponents[i] = opponents[j];
                        opponents[j] = temp;
                    }
                }
            }

            currTarget = opponents[opponents.Count - 1];
        }

        Debug.Log("Shambler is targeting " + currTarget.charName);

        return currTarget;
    }

    public override bool Turn(string move, Unit target)
    {
		//Scoring system based on current health of both target and this unit to figure out if it is worth attacking or protecting self.
		if (!isStunned)
		{
            Slice(target);
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

    //Moves to call
    public void Slice(Unit target)
    {
        int damage = BattleController.CalculateDamage(10, this, target);
        Debug.Log("The Shambler slices " + target.charName + " with its arm doing " + damage + " damage");
        target.TakeDamage(damage);

        BattleController.targets.Add(target);
    }
    public int Shamble()
    {
        return 0;
    }
    public void Groan()
    {
        Debug.Log("The Shambler bellows, invigorating itself");
        this.ChangeStat(1, StatType.defense);
        this.TakeDamage(-5);
    }
}
