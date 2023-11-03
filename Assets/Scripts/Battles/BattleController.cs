using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using NUnit.Framework.Constraints;
using System.Data;
using UnityEngine.UI;
using UnityEngine.U2D;
using static UnityEngine.GraphicsBuffer;
using System.Runtime.CompilerServices;
using static UnityEngine.UI.Button;

public class BattleController : MonoBehaviour
{
    //BattleSprites
    public Image player1;
	public Image player2;
	public Image player3;
	public Image player4;
    public static Image[] playerImages = new Image[4];
    public static Dictionary<string, Image> imgMap = new Dictionary<string, Image>();

    public Image enemy1;
    public Image enemy2;
    public Image enemy3;
    public Image enemy4;
    public static Image[] enemyImages = new Image[4];

    public BattlerManager spriteManager;
    public static BattlerManager battlerManager;

    private static int playerSpot = 0;
    private static int enemySpot = 0;

    //Accurate battle name counters
    private static int shriekerCount = 0;
    private static int shamblerCount = 0;
    private static int striderCount = 0;

    //Battle Initialization
    public List<string> characters = new List<string>();


	//Typer componenets
	public bool doneTyping = false;

    //Ui Components
    public TMP_Text output;

    public GameObject mainButtonContainer;
    public GameObject actionButtonContainer;
    public GameObject targetButtonContainer;

    public List<Button> mainButtons = new List<Button>();
    public List<TMP_Text> attackOptions = new List<TMP_Text>();
    public List<Button> attackButtons = new List<Button>();
    public List<TMP_Text> targetOptions = new List<TMP_Text>();
    public List<GameObject> targetButtons = new List<GameObject>();

    public static bool waitingForMainChoice = true;
    public static bool waitingForMoveChoice = true;
    public static bool waitingForTarget = true;

    public static string moveName, targetName;

    //Unit Lists
    public static List<Unit> battlers = new List<Unit>();
    public static List<Unit> players = new List<Unit>();
    public static List<Unit> enemies = new List<Unit>();
    public static List<Unit> targets = new List<Unit>();
    private static List<Unit> targetsList = new List<Unit>();
    bool isOver = false;

    public GameObject portrait1;
    public GameObject portrait2;
    public GameObject portrait3;
    public GameObject portrait4;
    public static GameObject[] PlayerPortraits = new GameObject[4];

    //Battle initializer that uses a list of strings for
    //unit names and creates them???

    public void Start()
    {
        battlers.Clear();
        players.Clear();
        enemies.Clear();
        targets.Clear();

        playerSpot = 0;
        enemySpot = 0;

        battlerManager = spriteManager;

        playerImages[0] = player1;
        playerImages[1] = player2;
        playerImages[2] = player3;
        playerImages[3] = player4;

        enemyImages[0] = enemy1;
        enemyImages[1] = enemy2;
        enemyImages[2] = enemy3;
        enemyImages[3] = enemy4;

        PlayerPortraits[0] = portrait1;
		PlayerPortraits[1] = portrait2;
		PlayerPortraits[2] = portrait3;
		PlayerPortraits[3] = portrait4;

		characters.Add("Cynthia");
		characters.Add("Asheton");
		//characters.Add("Heather");
		//characters.Add("Logan");
		//characters.Add("Bea");
		//characters.Add("Madison");
		//characters.Add("Emilia");

		characters.Add("Shambler");


        //characters.AddRange(GlobalUtility.CreateEncounter());

        foreach (string character in characters)
		{
			AddCharacter(character);
		}

		StartCoroutine(Battle());
    }

    // Start is called before the first frame update
    public IEnumerator Battle()
    {
        int battlerIndex = -1;

        do
        {
            Debug.Log("Players: " + players.Count + "\nEnemies: " + enemies.Count);

            if (battlerIndex < (battlers.Count - 1))
            {
                battlerIndex++;
            }
            else
            {
                battlerIndex = 0;
            }

            Debug.Log("current battle index: " + battlerIndex);

            if (battlerIndex == 0)
            {
                battlers.Sort();

                string debugMsg = "Turn order is currently: ";

                foreach (Unit unit in battlers)
                {
                    debugMsg += " " + unit.charName + " (" + unit.currSpd + ")" + ", ";
                }

                Debug.Log(debugMsg);

                Debug.Log("TOP OF THE ROUND");
            }

            if (!battlers[battlerIndex].isDead)
            {
                output.text = "";

                if (battlers[battlerIndex].type == UnitType.player)
                {
                    battlers[battlerIndex].TakeStamina(-5);
                    UpdateStaminaBar(battlers[battlerIndex]);

                    if (!battlers[battlerIndex].isStunned && !(battlers[battlerIndex] is Asheton && ((battlers[battlerIndex] as Asheton).isSneaking == true)) && !(battlers[battlerIndex] is Emilia && ((battlers[battlerIndex] as Emilia).plansLaid == true)))
                    {
                        yield return DialogLineMapper.Type((done) => doneTyping = done, "It is " + battlers[battlerIndex].charName + "'s turn", output);

                        output.enabled = false;

                        mainButtonContainer.SetActive(true);

                        yield return new WaitWhile(() => waitingForMainChoice);

                        foreach (Button button in attackButtons)
                        {
                            button.interactable = true;
                        }

                        waitingForMainChoice = true;
                        Debug.Log(waitingForMainChoice);

                        int moveCount = 0;

                        for (int i = 0; i < battlers[battlerIndex].moves.Count; i++)
                        {
                            attackOptions[i].text = battlers[battlerIndex].moves[i].name;

                            //If move too expensive
                            if(battlers[battlerIndex].moves[i].cost > battlers[battlerIndex].currStm)
							{
                                attackButtons[i].interactable = false;
                            }
                        }

                        mainButtonContainer.SetActive(false);
                        actionButtonContainer.SetActive(true);

                        yield return new WaitWhile(() => waitingForMoveChoice);
                        waitingForMoveChoice = true;

                        actionButtonContainer.SetActive(false);

                        Move move = SaveChosenMove(battlers[battlerIndex]);

                        switch (move.numTargets)
                        {
                            case 1:
                                targetsList.AddRange(enemies);
                                break;
                            case -1:
                                targetsList.AddRange(players);
                                targetsList.Remove(battlers[battlerIndex]);
                                break;
                            default:
                                targetsList.Clear();
                                break;
                        }

                        if (targetsList.Count > 0)
                        {

                            for (int i = 0; i < targetsList.Count; i++)
                            {
                                targetOptions[i].text = targetsList[i].charName;
                            }

                            if (targetsList.Count < targetOptions.Count)
                            {
                                for (int i = targetsList.Count; i < targetOptions.Count; i++)
                                {
                                    targetButtons[i].SetActive(false);
                                }
                            }

                            targetButtonContainer.SetActive(true);

                            yield return new WaitWhile(() => waitingForTarget);
                            waitingForTarget = true;

                            Debug.Log(targetName);

                            battlers[battlerIndex].Turn(moveName, battlers[battlerIndex].ChooseTarget(targetName));
                        }
                        else
                        {
                            battlers[battlerIndex].Turn(moveName, battlers[battlerIndex]);
                        }

                        targetsList.Clear();
                        targetButtonContainer.SetActive(false);
                        output.enabled = true;

                    }
                    else if (battlers[battlerIndex] is Asheton && (battlers[battlerIndex] as Asheton).isSneaking == true)
                    {
                        battlers[battlerIndex].Turn("", battlers[battlerIndex]);
                    }
                    else
                    {
                        battlers[battlerIndex].Turn("stunned", battlers[battlerIndex]);
                    }

                    while (DialogLineMapper.hasLines)
                    {
						yield return DialogLineMapper.Type((done) => doneTyping = done, DialogLineMapper.GetNextLine(), output);
					}

                    foreach (Unit target in targets)
                    {
                        Debug.Log(target.charName + ": " + target.isDead);

                        if (target.isDead)
                        {
                            DialogLineMapper.QueueLine(target.charName + " has died");

                            Debug.Log(enemies.Count);
                            Debug.Log(target.charName);

                            for (int i = 0; i < enemies.Count; i++)
                            {
                                Debug.Log(enemies[i].charName + " : " + target.charName);
                                if (enemies[i].charName == target.charName)
                                {
                                    enemyImages[i].enabled = false;
                                    Debug.Log("I worked");
                                    break;
                                }
                            }

                            enemies.Remove(target);
                            battlers.Remove(target);
                        }
                    }
                    targets.Clear();

                    while (DialogLineMapper.hasLines)
                    {
                        yield return DialogLineMapper.Type((done) => doneTyping = done, DialogLineMapper.GetNextLine(), output);
                    }

                }
                else if (battlers[battlerIndex].type == UnitType.enemy)
                {
                    moveName = "";

                    yield return DialogLineMapper.Type((done) => doneTyping = done, "It is " + battlers[battlerIndex].charName + "'s turn", output);

                    battlers[battlerIndex].Turn("does not matter", battlers[battlerIndex].ChooseTarget("does not matter"));

					while (DialogLineMapper.hasLines)
					{
						yield return DialogLineMapper.Type((done) => doneTyping = done, DialogLineMapper.GetNextLine(), output);
					}

                    foreach (Unit target in targets)
                    {
                        Debug.Log(target.charName + ": " + target.isDead);

                        if (target.isDead)
                        {
                            DialogLineMapper.QueueLine(target.charName + " has died");

                            Debug.Log(players.Count);
                            Debug.Log(target.charName);

                            imgMap[target.charName].enabled = false;

                            players.Remove(target);
                            battlers.Remove(target);

                            yield return DialogLineMapper.Type((done) => doneTyping = done, DialogLineMapper.GetNextLine(), output);
                        }
                    }
                    targets.Clear();
                }

            }
  
            if (players.Count == 0)
            {
                DialogLineMapper.QueueLine("You Lose");

                yield return DialogLineMapper.Type((done) => doneTyping = done, DialogLineMapper.GetNextLine(), output);

                isOver = true;
            }
            if (enemies.Count == 0)
            {
                DialogLineMapper.QueueLine("You Win");

                yield return DialogLineMapper.Type((done) => doneTyping = done, DialogLineMapper.GetNextLine(), output);

                isOver = true;
            }

            Debug.Log("Current Battlers" + battlers.Count);

        }
        while (!isOver);

        foreach(Image image in playerImages)
        {
            image.enabled = false;
        }
		foreach (Image image in enemyImages)
		{
			image.enabled = false;
		}
	}

    public static Move SaveChosenMove(Unit caller)
    {
        Move chosenMove = new Move("", 0, 0);

        for (int i = 0; i < caller.moves.Count; i++)
        {
            if(caller.moves[i].name == moveName)
            {
                chosenMove = caller.moves[i];
                return chosenMove;
            }
        }
        return chosenMove;
    }

    public static int CalculateDamage(int power, Unit attacker, Unit defender)
    {
        return Mathf.RoundToInt((attacker.currAtk + power + Random.Range(0, 10)) / defender.currDef) + 1;
    }

    public static int CalculatePoisonDamage(Unit unit)
    {
        return Mathf.RoundToInt((unit.bsHp * 0.05f));
    }

    public static void AddCharacter(string character)
    {
        char enemyLetter;
        Slider[] sliders;

        switch (character)
        {
            case "Cynthia":
                if (playerSpot < 4)
                {
                    Cynthia cynthia = new Cynthia(10, 10, 10, 10, 100, 10, 10, 10, 10, 10);
                    battlers.Add(cynthia);
                    players.Add(cynthia);

                    //Hp Bar Code
                    PlayerPortraits[playerSpot].tag = cynthia.charName;
                    sliders = PlayerPortraits[playerSpot].GetComponentsInChildren<Slider>();

                    for(int i = 0; i < sliders.Length; i++)
					{
                        if(sliders[i].tag == "health")
						{
                            Debug.Log("It work");
                            sliders[i].maxValue = cynthia.bsHp;
                            sliders[i].value = cynthia.currHp;
						}
                        else if(sliders[i].tag == "stamina")
						{
                            sliders[i].maxValue = cynthia.bsStm;
                            sliders[i].value = cynthia.currStm;
                        }
					}
                    PlayerPortraits[playerSpot].GetComponentInChildren<TMP_Text>().text = cynthia.currHp + "\n" + cynthia.currStm;
                    if(battlerManager.CynthiaHead != null)
					{
                        PlayerPortraits[playerSpot].GetComponentInChildren<Image>().sprite = battlerManager.CynthiaHead;
					}
                    PlayerPortraits[playerSpot].SetActive(true);

                    if (battlerManager.Cynthia != null)
                    {
                        playerImages[playerSpot].sprite = battlerManager.Cynthia;
                        playerImages[playerSpot].enabled = true;
                        imgMap.Add(character, playerImages[playerSpot]);
                    }
                    playerSpot++;
                }

                break;
            case "Heather":

                if (playerSpot < 4)
                {
                    Heather heather = new Heather(12, 12, 10, 1, 12, 12, 12, 10, 1, 12);
                    battlers.Add(heather);
                    players.Add(heather);

                    if (battlerManager.Heather != null)
                    {
                        playerImages[playerSpot].sprite = battlerManager.Heather;
						playerImages[playerSpot].enabled = true;
                        imgMap.Add(character, playerImages[playerSpot]);
                    }
                    playerSpot++;
                }
                break;
            case "Bea":
                if (playerSpot < 4)
                {
                    Bea bea = new Bea(10, 10, 10, 10, 10, 10, 10, 10, 10, 10);
                    battlers.Add(bea);
                    players.Add(bea);

                    if (battlerManager.Bea != null)
                    {
                        playerImages[playerSpot].sprite = battlerManager.Bea;
                        playerImages[playerSpot].enabled = true;
                        imgMap.Add(character, playerImages[playerSpot]);
                    }
                    playerSpot++;
                }
				break;
            case "Logan":
                if (playerSpot < 4)
                {
                    Logan logan = new Logan(10, 10, 10, 10, 10, 10, 10, 10, 10, 10);
                    battlers.Add(logan);
                    players.Add(logan);

                    if (battlerManager.Logan != null)
                    {
                        playerImages[playerSpot].sprite = battlerManager.Logan;
                        playerImages[playerSpot].enabled = true;
                        imgMap.Add(character, playerImages[playerSpot]);
                    }
                    playerSpot++;
                }
				break;
            case "Asheton":
				if (playerSpot < 4)
				{
					Asheton asheton = new Asheton(12, 12, 10, 120, 12, 12, 12, 10, 120, 12);
					battlers.Add(asheton);
					players.Add(asheton);

                    PlayerPortraits[playerSpot].tag = asheton.charName;
                    sliders = PlayerPortraits[playerSpot].GetComponentsInChildren<Slider>();

                    for (int i = 0; i < sliders.Length; i++)
                    {
                        if (sliders[i].tag == "health")
                        {
                            sliders[i].maxValue = asheton.bsHp;
                            sliders[i].value = asheton.currHp;
                        }
                        else if (sliders[i].tag == "stamina")
                        {
                            sliders[i].maxValue = asheton.bsStm;
                            sliders[i].value = asheton.currStm;
                        }
                    }
                    PlayerPortraits[playerSpot].GetComponentInChildren<TMP_Text>().text = asheton.currHp + "\n" + asheton.currStm;
                    if (battlerManager.AshetonHead != null)
                    {
                        PlayerPortraits[playerSpot].GetComponentInChildren<Image>().sprite = battlerManager.AshetonHead;
                    }
                    PlayerPortraits[playerSpot].SetActive(true);

                    if (battlerManager.Asheton != null)
					{
						playerImages[playerSpot].sprite = battlerManager.Asheton;
						playerImages[playerSpot].enabled = true;
                        imgMap.Add(character, playerImages[playerSpot]);
                    }
                    playerSpot++;
				}
				break;
            case "Emilia":

                if (playerSpot < 4)
                {
                    Emilia emilia = new Emilia(12, 12, 10, 120, 12, 12, 12, 10, 120, 12);
                    battlers.Add(emilia);
                    players.Add(emilia);

                    if (battlerManager.Emilia != null)
                    {
                        playerImages[playerSpot].sprite = battlerManager.Emilia;
						playerImages[playerSpot].enabled = true;
                        imgMap.Add(character, playerImages[playerSpot]);
                    }
                }
                break;
            case "Madison":

                if (playerSpot < 4)
                {
                    Madison madison = new Madison(12, 12, 10, 12, 12, 12, 12, 10, 12, 12);
                    battlers.Add(madison);
                    players.Add(madison);

                    if (battlerManager.Madison != null)
                    {
                        playerImages[playerSpot].sprite = battlerManager.Madison;
						playerImages[playerSpot].enabled = true;
                        imgMap.Add(character, playerImages[playerSpot]);
                    }
                    playerSpot++;
                }

                break;
            case "Shambler":

                if (enemySpot < enemyImages.Length)
                {
                    enemyLetter = (char)(shamblerCount + 65);
                    shamblerCount++;
                    Shambler shambler = new Shambler("Shambler " + enemyLetter, 12, 12, 12, 22, 12, 12, 12, 12, 30, 12);
                    battlers.Add(shambler);
                    enemies.Add(shambler);

                    if (battlerManager.Shambler != null)
                    {
                        enemyImages[enemySpot].sprite = battlerManager.Shambler;
						enemyImages[enemySpot].enabled = true;
					}
                    enemySpot++;
                }
                break;
            case "Shrieker":
				if (enemySpot < enemyImages.Length)
				{
					enemyLetter = (char)(shriekerCount + 65);
					shriekerCount++;
					Shrieker shrieker = new Shrieker("Shrieker " + enemyLetter, 12, 12, 12, 9, 12, 12, 12, 12, 30, 12);
					battlers.Add(shrieker);
					enemies.Add(shrieker);

					if (battlerManager.Shrieker != null)
					{
						enemyImages[enemySpot].sprite = battlerManager.Shrieker;
						enemyImages[enemySpot].enabled = true;
					}
					enemySpot++;
				}
				break;
            case "Strider":
				if (enemySpot < enemyImages.Length)
				{
					enemyLetter = (char)(striderCount + 65);
					striderCount++;
					Strider strider = new Strider("Strider " + enemyLetter, 12, 12, 12, 30, 12, 12, 12, 12, 30, 12);
					battlers.Add(strider);
					enemies.Add(strider);

					if (battlerManager.Strider != null)
					{
						enemyImages[enemySpot].sprite = battlerManager.Strider;
                        enemyImages[enemySpot].enabled = true;
					}
					enemySpot++;
				}
				break;
        }
    }

    public static void UpdateHealthbar(Unit target)
	{
        foreach (GameObject obj in PlayerPortraits)
        {
            if (obj.tag == target.charName)
            {
                Slider[] sliders = obj.GetComponentsInChildren<Slider>();

                for(int i = 0; i < sliders.Length; i++)
				{
                    if(sliders[i].tag == "health")
					{
                        sliders[i].value = target.currHp;
                        obj.GetComponentInChildren<TMP_Text>().text = target.currHp + "\n" + target.currStm;
                    }
				}
            }
        }
    }

    public static void UpdateStaminaBar(Unit target)
	{
        foreach (GameObject obj in PlayerPortraits)
        {
            if (obj.tag == target.charName)
            {
                Slider[] sliders = obj.GetComponentsInChildren<Slider>();

                for (int i = 0; i < sliders.Length; i++)
                {
                    if (sliders[i].tag == "stamina")
                    {
                        sliders[i].value = target.currHp;
                        obj.GetComponentInChildren<TMP_Text>().text = target.currHp + "\n" + target.currStm;
                    }
                }
            }
        }
    }
}


