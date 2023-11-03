using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class Shrieker : Unit
{
    public Shrieker(string name, int bsAtk, int bsDef,
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
        return BattleController.players[0];
    }

    public override bool Turn(string move, Unit target)
    {
        if(!isStunned)
        {
            int choice = Random.Range(1, 4);

			switch (choice)
			{
				case 1:
					Scratch(target);
					break;

				case 2:
					Shriek();
					break;

				case 3:
					CallForHelp();
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
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    //Free attack
    public void Scratch(Unit target)
    {
        DialogLineMapper.QueueLine(this.charName + " flails wildly with it's calws!");
        target.TakeDamage(BattleController.CalculateDamage(10, this, target));

        BattleController.targets.Add(target);
    }

    //Med cost debuff
    public void Shriek()
    {
        DialogLineMapper.QueueLine(this.charName + " lets out an ear-piercing shriek");

        foreach (Unit target in BattleController.players)
        {
            int roll = Random.Range(0, 10);
            if (roll % 4 == 0)
            {
                target.isStunned = true;
            }

            target.TakeDamage(BattleController.CalculateDamage(3, this, target));
            DialogLineMapper.QueueLine(target.charName + " is reeling!");
        }

        BattleController.targets.AddRange(BattleController.players);
    }

    //High cost summon
    public void CallForHelp()
    {
        int decision = Random.Range(0, 8) + 2;

        DialogLineMapper.QueueLine(this.charName + " lets out a piercing shriek");

        if(decision < 4)
        {
            DialogLineMapper.QueueLine("But nothing came...");
        }

        //enemy logic should make this irrelevant
        //But this is here as a failsafe to prevent
        //Fifth invisible enemies
        if(BattleController.enemies.Count < 4)
        {
            string enemyType = "";

            switch (decision)
            {
                case 5:
                    enemyType = "Shambler";
                    break;
                case 6:
                    enemyType = "Strider";
                    break;
                case 7:
                    enemyType = "Shrieker";
                    break;
            }

            if(enemyType != "")
            {
                DialogLineMapper.QueueLine("A new" + enemyType + "appears!");
                BattleController.AddCharacter(enemyType);
            }

        }

    }
}
