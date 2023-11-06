using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bea : Unit
{
    public Bea(int bsAtk, int bsDef,
            int bsSpd, int bsHp, int bsStm, int currAtk, int currDef, int currSpd,
            int currHp, int currStm) :
            base("Bea", false, bsAtk, bsDef, bsSpd, bsHp, bsStm,
            currAtk, currDef, currSpd, currHp, currStm)
    {
        Debug.Log("Created " + charName + " with " + currHp + " HP");
        this.type = UnitType.player;

        this.moves.Add(new Move("Riff", 2, 0));
        this.moves.Add(new Move("Let Loose", 0, 10));
        this.moves.Add(new Move("Song of Ferocity", -2, 15));
        this.moves.Add(new Move("Crank it to 11", 2, 30));

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
        base.TakeDamage(damage);
    }

    public override bool Turn(string move, Unit target)
    {
		if (!isStunned)
		{
			switch (move)
			{
                case "Riff":
                    Riff();
                    break;
                case "Let Loose":
                    LetLoose();
                    break;
                case "Song of Ferocity":
                    SongOfFerocity();
                    break;
                case "Crank it to 11":
                    CrankItTo11();
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

    //Free Damage move
    public void Riff()
    {
        DialogLineMapper.QueueLine("Bea gives her guitar a hearty strum.");

        foreach(Unit target in BattleController.enemies)
        {
            DialogLineMapper.QueueLine(target.charName + " placeholder dialog");
            int damage = BattleController.CalculateDamage(10, this, target);
            target.TakeDamage(damage);
        }

        BattleController.targets.AddRange(BattleController.enemies);
    }

    //Med cost
    //Increases atk, heals a small amount, reduces def
    public void LetLoose()
    {
        this.TakeStamina(10);

        DialogLineMapper.QueueLine("Bea lets go of her restraints with a sigh");
        this.ChangeStat(1, StatType.attack);
        this.TakeDamage(-2);
    }

    //High Cost
    //Gives all allies atk and spd up
    public void SongOfFerocity()
    {
        this.TakeStamina(15);

        DialogLineMapper.QueueLine("Bea inspires the party to give it their all...");
        foreach(Unit target in BattleController.players)
        {
            DialogLineMapper.QueueLine(target.charName + " is feeling pumped up!");
            target.ChangeStat(1, StatType.attack);
            target.ChangeStat(1, StatType.speed);
        }
    }

    //Highest Cost
    //Damage++ and  50% stun
    public void CrankItTo11()
    {
		DialogLineMapper.QueueLine("Bea cranks it to 11!!!!");

        this.TakeStamina(30);

		foreach (Unit target in BattleController.enemies)
		{
			DialogLineMapper.QueueLine(target.charName + " is reeling...");
			int damage = BattleController.CalculateDamage(10, this, target);
			target.TakeDamage(damage);
            
            int roll = Random.Range(0, 10);

            if(roll % 2 == 0)
            {
				target.isStunned = true;
			}
		}

		BattleController.targets.AddRange(BattleController.enemies);
	}
}
