using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Cynthia : Unit
{
    public Cynthia(int bsAtk, int bsDef, 
        int bsSpd, int bsHp, int bsStm, int currAtk, int currDef, int currSpd, 
        int currHp, int currStm) : 
        base("Cynthia", false, bsAtk, bsDef, bsSpd, bsHp, bsStm, 
            currAtk, currDef, currSpd, currHp, currStm)
    {
        Debug.Log("Created " + charName + " with " + currHp + " HP");
        this.type = UnitType.player;
        moves.Add(new Move("Punch", 1, 0));
        moves.Add(new Move("UwU", 2, 15));
        moves.Add(new Move("Ponder Life", 0, 10));
        moves.Add(new Move("Cheer On", -1, 5));
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
        if (this.currStm < this.bsStm && (this.currStm + 2) <= bsStm)
        {
            currStm += 2;
        }
        else
        {
            currStm = bsStm;
        }

		if (!isStunned)
		{
            switch (move)
            {
                case "Punch":
                    Punch(target);
                    break;
                case "UwU":
                    UwU();
                    break;
                case "Ponder Life":
                    PonderLife();
                    break;
                case "Cheer On":
                    CheerOn(target);
                    break;
            }

            BattleController.UpdateStaminaBar(this);
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

    //Methods for her moves will go here,
    //goal of 4 per character
    public void Punch(Unit target)
    {
        int damage = BattleController.CalculateDamage(15, this, target);
        DialogLineMapper.QueueLine("Cynthia clocks " + target + " in the face for " + damage + " damage!");
        //Debug.Log("Cynthia punches " + target.charName + " for " + damage + " damage");
        target.TakeDamage(damage);

        BattleController.targets.Add(target);
    }

    //med cost
    public void UwU()
    {
        this.TakeStamina(15);

        //Debug.Log("Cynthia lets out an \"UwU\"");
        DialogLineMapper.QueueLine("Cynthia lets out a heartfelt \"UwU\"");

        foreach (Unit target in BattleController.enemies)
        {
            DialogLineMapper.QueueLine(target.charName + " is completely mortified...");
            target.ChangeStat(-1, StatType.attack);
            target.ChangeStat(-1, StatType.defense);
        }
    }

    public void PonderLife()
    {
        this.TakeStamina(10);
        
        DialogLineMapper.QueueLine("Cynthia stops to consider where her decisions are taking her");

        int choice = Random.Range(1, 10);

        if(choice % 2 == 0)
        {
            DialogLineMapper.QueueLine("After some thought... She decides to keep fighting!");
            this.ChangeStat(1, StatType.attack);
            this.ChangeStat(1, StatType.speed);
            DialogLineMapper.QueueLine("Cynthia feels envigorated!");
        }
        else
        {
            DialogLineMapper.QueueLine("After some thought... She becomes lost in thought!");
            this.ChangeStat(1, StatType.defense);
            this.ChangeStat(-1, StatType.speed);
            DialogLineMapper.QueueLine("Cynthia is hesitating...");
        }
    }

    public void CheerOn(Unit target)
    {
        this.TakeStamina(5);

        //This move will add stamina when that's a thing lol
        DialogLineMapper.QueueLine("Cynthia yells to " + target.charName + " telling them she believes in them");
        target.ChangeStat(1, StatType.speed);
        DialogLineMapper.QueueLine(target.charName + " is feeling more confident!");
    }
}

