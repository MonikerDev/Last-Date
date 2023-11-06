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
        moves.Add(new Move("Bag Swipe", 1, 0));
        moves.Add(new Move("Breathe", 1, 0));
        moves.Add(new Move("Upgrade", 0, 30));
        moves.Add(new Move("Tinker", 15, 0));
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
    public void BagSwipe(Unit target)
    {
        DialogLineMapper.QueueLine(this.charName + " swings her bag at " + target.charName);
        target.TakeDamage(BattleController.CalculateDamage(10, this, target));
    }

    //Recover Energy
    public void Breathe()
	{
        DialogLineMapper.QueueLine(this.charName + " takes a deep breath...");
        this.TakeStamina(-20);
        DialogLineMapper.QueueLine(this.charName + " is feeling a little refreshed!");
	}

    //next tinker effect will be better
    public void Upgrade()
    {
        this.TakeStamina(30);
        DialogLineMapper.QueueLine(this.charName + " carefully plans her next gadget...");
        upgraded = true;
    }

    //Low Cost
    //Random Effect From generous selction
    public void Tinker()
    {
        this.TakeStamina(15);

        int upgradePotency = 25;
        int effect = Random.Range(0, 101);
        int singleTargetEnemy = Random.Range(0, BattleController.enemies.Count);
        int singleTargetPlayer = Random.Range(0, BattleController.players.Count);

		if (upgraded)
		{
            if(effect + upgradePotency > 100)
			{
                effect = 100;
			}
			else
			{
                effect += upgradePotency;
			}

            upgraded = false;
		}

        Debug.Log("Tinker effect num: " + effect);

        //Self damage 1-3
		if(effect < 4)
		{
            this.TakeDamage(Mathf.RoundToInt(this.bsHp * 0.1f));
		}
        //All players speed down 4-5
        else if(effect > 3 && effect < 6)
		{
            foreach(Unit u in BattleController.players)
			{
                u.ChangeStat(-1, StatType.speed);
			}
		}
        //All players defense down 6-7
        else if (effect > 5 && effect < 8)
        {
            foreach (Unit u in BattleController.players)
            {
                u.ChangeStat(-1, StatType.defense);
            }
        }
        //All players attack down 8-9
        else if (effect > 7 && effect < 10)
        {
            foreach (Unit u in BattleController.players)
            {
                u.ChangeStat(-1, StatType.attack);
            }
        }
        //All enemies speed up 10-11
        else if (effect > 9 && effect < 12)
        {
            foreach (Unit u in BattleController.enemies)
            {
                u.ChangeStat(1, StatType.speed);
            }
        }
        //All enemies defense up 12-13
        else if (effect > 11 && effect < 14)
        {
            foreach (Unit u in BattleController.enemies)
            {
                u.ChangeStat(1, StatType.defense);
            }
        }
        //All enemies attack up 14-15
        else if (effect > 13 && effect < 16)
        {
            foreach (Unit u in BattleController.enemies)
            {
                u.ChangeStat(1, StatType.attack);
            }
        }
        //Single Player Speed down 16-18
        else if (effect > 15 && effect < 19)
        {
            BattleController.players[singleTargetPlayer].ChangeStat(-1, StatType.speed);
        }
        //Single Player Defense down 19-22
        else if (effect > 18 && effect < 23)
        {
            BattleController.players[singleTargetPlayer].ChangeStat(-1, StatType.defense);
        }
        //Single Player Attack Down 23-25
        else if (effect > 22 && effect < 26)
        {
            BattleController.players[singleTargetPlayer].ChangeStat(-1, StatType.attack);
        }
        //Single Enemy Speed up 26-28
        else if (effect > 25 && effect < 29)
        {
            BattleController.enemies[singleTargetEnemy].ChangeStat(1, StatType.speed);
        }
        //Single Enemey Defense up 29-31
        else if (effect > 28 && effect < 32)
        {
            BattleController.enemies[singleTargetEnemy].ChangeStat(1, StatType.defense);
        }
        //Single Enenmy Attack up 32-34
        else if (effect > 31 && effect < 35)
        {
            BattleController.enemies[singleTargetEnemy].ChangeStat(1, StatType.attack);
        }
        //One enemy speed down 35-38
        else if (effect > 25 && effect < 29)
        {
            BattleController.enemies[singleTargetEnemy].ChangeStat(-1, StatType.speed);
        }
        //One enemy defense down 39-42
        else if (effect > 28 && effect < 32)
        {
            BattleController.enemies[singleTargetEnemy].ChangeStat(-1, StatType.defense);
        }
        //One enemy attack down 43-46
        else if (effect > 31 && effect < 35)
        {
            BattleController.enemies[singleTargetEnemy].ChangeStat(-1, StatType.attack);
        }
        //Light damage to one enemy 47-54
        else if (effect > 46 && effect < 55)
        {
            BattleController.enemies[singleTargetEnemy].TakeDamage(BattleController.CalculateDamage(50, this, BattleController.enemies[singleTargetEnemy]));
        }
        //Light damage to all enemies 55-60
        else if (effect > 54 && effect < 61)
        {
            foreach (Unit u in BattleController.enemies)
            {
                u.TakeDamage(BattleController.CalculateDamage(50, this, u));
            }
        }
        //Single Player Speed up 61-64
        else if (effect > 60 && effect < 65)
        {
            BattleController.players[singleTargetPlayer].ChangeStat(1, StatType.speed);
        }
        //Single Player Defense up 65-68
        else if (effect > 64 && effect < 69)
        {
            BattleController.players[singleTargetPlayer].ChangeStat(1, StatType.defense);
        }
        //Single Player Attack up 69-72
        else if (effect > 68 && effect < 73)
        {
            BattleController.players[singleTargetPlayer].ChangeStat(1, StatType.attack);
        }
        //All players speed up 73-74
        else if (effect > 72 && effect < 75)
        {
            foreach (Unit u in BattleController.players)
            {
                u.ChangeStat(1, StatType.speed);
            }
        }
        //All players defense up 75-76
        else if (effect > 74 && effect < 77)
        {
            foreach (Unit u in BattleController.players)
            {
                u.ChangeStat(1, StatType.defense);
            }
        }
        //All players attack down 77-78
        else if (effect > 76 && effect < 79)
        {
            foreach (Unit u in BattleController.players)
            {
                u.ChangeStat(1, StatType.attack);
            }
        }
        //All enemies speed down 79-80
        else if (effect > 78 && effect < 81)
        {
            foreach (Unit u in BattleController.enemies)
            {
                u.ChangeStat(-1, StatType.speed);
            }
        }
        //All enemies defense down 81-82
        else if (effect > 80 && effect < 83)
        {
            foreach (Unit u in BattleController.enemies)
            {
                u.ChangeStat(-1, StatType.defense);
            }
        }
        //All enemies attack down 83-84
        else if (effect > 82 && effect < 85)
        {
            foreach (Unit u in BattleController.enemies)
            {
                u.ChangeStat(-1, StatType.attack);
            }
        }
        //Single Medium Damage 85-90
        else if (effect > 84 && effect < 91)
        {
            BattleController.enemies[singleTargetEnemy].TakeDamage(BattleController.CalculateDamage(100, this, BattleController.enemies[singleTargetEnemy]));
        }
        //All Medium Damage 91-94
        else if (effect > 90 && effect < 95)
        {
            foreach (Unit u in BattleController.enemies)
            {
                u.TakeDamage(BattleController.CalculateDamage(100, this, u));
            }
        }
        //Single Heavy Damage 95-98
        else if (effect > 94 && effect < 99)
        {
            BattleController.enemies[singleTargetEnemy].TakeDamage(BattleController.CalculateDamage(200, this, BattleController.enemies[singleTargetEnemy]));
        }
        //All Heavy Damage 99-100
        else if (effect > 98 && effect <= 100)
        {
            foreach (Unit u in BattleController.enemies)
            {
                u.TakeDamage(BattleController.CalculateDamage(200, this, u));
            }
        }
        //Crescendo all stats up, enemy damage, 100% stun
        else if(effect == 0)
		{
            foreach(Unit u in BattleController.players)
			{
                u.ChangeStat(2, StatType.speed);
                u.ChangeStat(2, StatType.defense);
                u.ChangeStat(2, StatType.attack);
			}

            foreach(Unit u in BattleController.enemies)
			{
                u.TakeDamage(BattleController.CalculateDamage(200, this, u));
                u.isStunned = true;
            }
        }
    }
}
