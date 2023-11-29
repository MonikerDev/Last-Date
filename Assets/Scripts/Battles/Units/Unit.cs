using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum UnitType
{
    player,
    enemy
}

public enum StatType
{
    attack,
    speed,
    defense
}

//Maybe add ability method that triggers on battle start sometimes???

public abstract class Unit : IComparable<Unit>
{
    public string charName;

    public int bsAtk, bsDef, bsSpd, bsHp, bsStm;
    public float currAtk, currDef, currSpd, currHp, currStm;

    //Non Saved data
    public bool isDead;
    public float atkMod, defMod, spdMod;

    public UnitType type;

    public List<Move> moves = new List<Move>();

    //Statuses
    public bool isStunned = false;
    public bool isPoisoned = false;

    protected Unit(string charName, bool isDead, int bsAtk, int bsDef, int bsSpd, 
        int bsHp, int bsStm, int currAtk, int currDef, int currSpd, int currHp, int currStm)
    {
        this.charName = charName;
        this.isDead = isDead;

        this.bsAtk = bsAtk;
        this.bsDef = bsDef;
        this.bsSpd = bsSpd;
        this.bsHp = bsHp;
        this.bsStm = bsStm;

        this.currAtk = currAtk;
        this.currDef = currDef;
        this.currSpd = currSpd;
        this.currHp = currHp;
        this.currStm = currStm;
    }

    public virtual void TakeDamage(int damage) 
    {
        Debug.Log(this.charName + ": " + this.currHp);
        if((currHp - damage) > bsHp)
        {
            currHp = bsHp;
        }
        else if((currHp - damage) < 0)
		{
            currHp = 0;
		}
        else
        {
			currHp -= damage;
		}
		Debug.Log(this.charName + ": " + this.currHp);

		if (currHp <= 0)
        {
            Debug.Log(this.charName + " has died");
            isDead = true;
        }

		Debug.Log(this.charName + " took " + damage + " damage");

        BattleController.UpdateHealthbar(this);
	}

	public virtual Unit ChooseTarget(String moveName) 
    {
        return null;
    }

    public virtual bool Turn(string move, Unit target) { return true; }

    public virtual void ChangeStat(int stages, StatType stat)
    {
        switch (stat)
        {
            case StatType.attack:
                if((this.atkMod + stages) > 6)
                {
                    this.atkMod = 6;
                    DialogLineMapper.QueueLine(this.charName + "'s attack can't go any higher!");
                }
                else if ((this.atkMod + stages) < -6)
                {
                    this.atkMod = -6;
                    DialogLineMapper.QueueLine(this.charName + "'s attack can't go any lower!");
                }
                else
                {
                    this.atkMod += stages;
                }

                this.currAtk = Mathf.Round((this.bsAtk * ((100f + (25f * atkMod)) / 100 )));

                if (Mathf.Sign(atkMod) == -1)
                {
                    DialogLineMapper.QueueLine(this.charName + "'s Attack was lowered!");
                }
                else
                {
                    DialogLineMapper.QueueLine(this.charName + "'s Attack was raised!");
                }

                break;

            case StatType.defense:
				if ((this.defMod + stages) > 6)
				{
					this.defMod = 6;
					DialogLineMapper.QueueLine(this.charName + "'s defense can't go any higher!");
				}
				else if ((this.defMod + stages) < -6)
				{
					this.defMod = -6;
					DialogLineMapper.QueueLine(this.charName + "'s def can't go any lower!");
				}
				else
				{
					this.defMod += stages;
				}

				this.currDef = Mathf.Round((this.bsDef * ((100f + (25f * defMod)) / 100)));

                if (Mathf.Sign(defMod) == -1)
                {
                    DialogLineMapper.QueueLine(this.charName + "'s Defense was lowered!");
                }
                else
                {
                    DialogLineMapper.QueueLine(this.charName + "'s Defense was raised!");
                }

                break;

            case StatType.speed:

				if ((this.spdMod + stages) > 6)
				{
					this.spdMod = 6;
					DialogLineMapper.QueueLine(this.charName + "'s speed can't go any higher!");
				}
				else if ((this.spdMod + stages) < -6)
				{
					this.spdMod = -6;
					DialogLineMapper.QueueLine(this.charName + "'s speed can't go any lower!");

				}
				else
				{
					this.spdMod += stages;
				}

				this.currSpd = Mathf.Round((this.bsSpd * ((100f + (25f * spdMod)) / 100)));

                if(Mathf.Sign(spdMod) == -1)
				{
                    DialogLineMapper.QueueLine(this.charName + "'s speed was lowered!");
				}
				else
				{
                    DialogLineMapper.QueueLine(this.charName + "'s speed was raised!");
				}

				break;
        }
    }

    public void TakeStamina(int stam)
	{
        if((this.currStm - stam) > this.bsStm)
		{
            this.currStm = this.bsStm;
		}
        else if((this.currStm - stam) < 0)
		{
            this.currStm = 0;
		}
		else
		{
            this.currStm -= stam;
		}
	}

	public int CompareTo(Unit other)
	{
		if(this.currSpd > other.currSpd) return -1;
        else if(this.currSpd <  other.currSpd) return 1;
        else
        {
            int decider = UnityEngine.Random.Range(1, 100);
            if(decider % 2 == 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
	}
}

//Num targets has specific formatting
// -2 == all allies
// -1 == one ally
// 0 == self
// 1 == 1 enemy
// 2 == all enemies
public struct Move
{
    public string name { get; set; }
    public int numTargets { get; set; }
	public int cost { get; set; }

	public Move(string name, int numTargets, int cost)
    {
        this.name = name;
        this.numTargets = numTargets;
        this.cost = cost;
    }
}
