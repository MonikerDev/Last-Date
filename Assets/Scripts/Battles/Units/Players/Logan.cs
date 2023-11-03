using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logan : Unit
{
    bool upgraded = false;

    public Logan(int bsAtk, int bsDef,
            int bsSpd, int bsHp, int bsStm, int currAtk, int currDef, int currSpd,
            int currHp, int currStm) :
            base("Logan", false, bsAtk, bsDef, bsSpd, bsHp, bsStm,
            currAtk, currDef, currSpd, currHp, currStm)
    {
        Debug.Log("Created " + charName + " with " + currHp + " HP");
        this.type = UnitType.player;
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
            if (roll % 3 == 0 && this.isDead == false)
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

    //Free damaging move
    public void BagSwipe()
    {

    }

    //next tinker effect will be better
    public void Upgrade()
    {
        
    }

    //Low Cost
    //Random Effect From generous selction
    public void Tinker()
    {
        int effectCount = 35;
        int effect = Random.Range(0, 1000) % effectCount;
        int singleTarget = Random.Range(0, 4);

        Debug.Log("Tinker effect num: " + effect);

        switch (effect)
		{
            //statdowns 6

            //Single Attack Down

            //Single Defense Down

            //Single Speed Down

            //All attack down

            //All defense down

            //All speed down

            //self stun 1

            //enemy statups 6
            //self damage 1
            //statups 6
            //enemey stat downs 6
            //enemy damage 6

            //Light Single Enemy Damage

            //

            //enemy stun

            //Crescendo all stats up, enemy damage, 100% stun
        }

    }
}
