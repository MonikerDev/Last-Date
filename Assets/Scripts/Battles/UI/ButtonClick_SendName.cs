using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonClick_SendName : MonoBehaviour
{
    public void ChooseMain()
    {
        BattleController.waitingForMainChoice = false;
        Debug.Log(BattleController.waitingForMainChoice);
    }

    public void ChooseMove()
    {
        BattleController.moveName = this.GetComponentInChildren<TMP_Text>().text;
        BattleController.waitingForMoveChoice = false;
    }

    public void ChooseTarget()
    {
        BattleController.targetName = this.GetComponentInChildren<TMP_Text>().text;
        BattleController.waitingForTarget = false;
    }
}
