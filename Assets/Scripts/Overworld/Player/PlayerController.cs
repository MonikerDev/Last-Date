using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movSpeed;
    public float maxSpeed = 20;
	public float minSpeed = 5;
    public float accellerationRate = 0.02f;
    public float currAccellerationRate;
	public float deccellerationRate = 0.6f;
	public float currDeccellerationRate;
	public float direction;
    public static bool canMove = true;
	Rigidbody2D rb;

	public float accellerationDelay = 0.2f;
	public float currDelay;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
        currAccellerationRate = accellerationRate;
		currDeccellerationRate = deccellerationRate;
		movSpeed = minSpeed;
		currDelay = accellerationDelay;
	}

	// Update is called once per frame
	void Update()
    {
		//Reset accelleration if direction changed
		if(direction != Input.GetAxis("Horizontal"))
		{
			currAccellerationRate = accellerationRate;
			currDeccellerationRate = deccellerationRate;
			movSpeed = minSpeed;
		}

		//Save direction
		direction = Input.GetAxis("Horizontal");

		//Only apply movement code if can move and IS moving
		if (canMove && Input.GetAxis("Horizontal") != 0)
		{
			//Apply force to move
			rb.AddForce(new Vector2(Time.deltaTime* movSpeed * Input.GetAxis("Horizontal"), 0), ForceMode2D.Impulse);
            
			//Accellerate
            if(movSpeed < maxSpeed)
			{
				if (currDelay <= 0)
				{
					movSpeed += currAccellerationRate;
					currAccellerationRate += currAccellerationRate;
					currDelay = accellerationDelay;
				}
				else
				{
					currDelay -= Time.deltaTime;
				}
			}
			//Cap out speed
			else if(movSpeed > maxSpeed)
			{
				movSpeed = maxSpeed;
			}
		}
		//If movement stopped, decellerate.
		else
		{
			//Decellerate
			if(movSpeed > minSpeed)
			{
				movSpeed -= currDeccellerationRate;
				currDeccellerationRate += deccellerationRate;
			}
			//Floor for movement
			else if(movSpeed <= minSpeed)
			{
				movSpeed = minSpeed;

				currAccellerationRate = accellerationRate;
				currDeccellerationRate = deccellerationRate;
			}
		}
    }
}
