using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBasedPlayerController : MonoBehaviour
{
	[Header("Movement")]
	private float moveSpeed;
	public float walkSpeed;
	public float groundDrag;

	[Header("Slow Walking")]
	public float slowWalkSpeed;

	[Header("Sprinting")]
	private bool canSprint;
	public float sprintSpeed;
	public MovementState state;
	public float currSprintEnergy;
	public float maxSprintEnergy;
	private bool sprintCanRegen;
	public float sprintCoolDown;

	[Header("Jumping")]
	public float jumpForce;
	public float jumpCooldown;
	public float airMultiplier;
	public static bool readyToJump;
	public float jumpStamCost;

	[Header("Crouching")]
	public float crouchSpeed;
	public float crouchYScale;
	public float startYScale;

	[Header("Stealth")]
	public bool isHidden;
	public bool isQuiet;
	public bool isNoisy;
	public LayerMask whatisCover;

	[Header("KeyBinds")]
	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode sprintKey = KeyCode.LeftShift;
	public KeyCode crouchKey = KeyCode.LeftControl;
	public KeyCode slowWalkKey = KeyCode.LeftAlt;

	[Header("Ground Check")]
	public float playerHeight;
	public LayerMask whatIsGround;
	private bool grounded;
	private Vector2 boxSize;
	private float castDistance;

	float horizontalInput;
	float verticalInput;

	Rigidbody2D rb;

	[Header("UI Interaction")]
	public static bool canMove;

	public enum MovementState
	{
		slowWalking,
		walking,
		sprinting,
		crouching,
		air
	}

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		canMove = true;

		//Prepare for sprinting and jumping
		canSprint = true;
		currSprintEnergy = maxSprintEnergy;
		ResetJump();

		//Get Y scale for crouching
		startYScale = transform.localScale.y;

		//Setup Ground Detection
		boxSize = new Vector2(playerHeight * 0.5f, playerHeight);
		castDistance = (playerHeight * 0.5f) + 0.2f;

		//Start stealth
		isHidden = false;
		isQuiet = false;
	}

	private void Update()
	{
		grounded = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, whatIsGround);

		StateHandler();
		StealthHandler();
		HandleInput();
		SpeedControl();

		if (currSprintEnergy <= 0)
		{
			sprintCanRegen = false;
			canSprint = false;
		}

		if (grounded)
			rb.drag = groundDrag;
		else
			rb.drag = 0;
	}

	public void HideCharacter()
	{
		this.isHidden = true;
	}

	public void UnHideCharacter()
	{
		this.isHidden = false;
	}

	private void FixedUpdate()
	{
		if (canMove)
		{
			MovePlayer();
		}
	}

	private void HandleInput()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");

		if (Input.GetKey(jumpKey) && readyToJump && grounded && (currSprintEnergy - jumpStamCost) >= 0)
		{
			readyToJump = false;

			Jump();

			Invoke(nameof(ResetJump), jumpCooldown);
		}

		if (Input.GetKey(sprintKey) && canSprint)
		{
			currSprintEnergy -= Time.deltaTime;
		}
		if (Input.GetKeyUp(sprintKey))
		{
			Invoke(nameof(StartSprintRegen), sprintCoolDown);
		}

		if (Input.GetKeyDown(crouchKey))
		{
			transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
			rb.AddForce(Vector3.down * 5f, ForceMode2D.Impulse);
		}
		if (Input.GetKeyUp(crouchKey))
		{
			transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
		}
	}

	private void StateHandler()
	{
		if (Input.GetKey(crouchKey))
		{
			state = MovementState.crouching;
			moveSpeed = crouchSpeed;
		}
		else if (Input.GetKey(slowWalkKey))
		{
			state = MovementState.slowWalking;
			moveSpeed = slowWalkSpeed;
		}
		else if (grounded && Input.GetKey(sprintKey) && currSprintEnergy > 0 && canSprint)
		{
			state = MovementState.sprinting;
			moveSpeed = sprintSpeed;

		}
		else if (grounded)
		{
			state = MovementState.walking;
			moveSpeed = walkSpeed;

			if (currSprintEnergy < maxSprintEnergy && sprintCanRegen)
			{
				currSprintEnergy += Time.deltaTime;
			}
			if (currSprintEnergy >= maxSprintEnergy)
			{
				canSprint = true;
			}
		}
		else
		{
			state = MovementState.air;
		}

		if(state != MovementState.crouching && isHidden)
		{
			UnHideCharacter();
		}
	}

	public void StealthHandler()
	{
		switch (state)
		{
			case MovementState.slowWalking:
			case MovementState.crouching:
				isQuiet = true;
				isNoisy = false;
				break;
			case MovementState.walking:
				if(rb.velocity.x == 0)
                {
					isQuiet = true;
					isNoisy = false;
                }
                else
                {
					isQuiet = false;
					isNoisy = false;
                }
				break;
			case MovementState.air:
				isQuiet = false;
				isNoisy = false;
				break;
			case MovementState.sprinting:
				isQuiet = false;
				isNoisy = true;
				break;

		}
	}

	private void MovePlayer()
	{
		Vector2 moveDirection = new Vector2(horizontalInput, 0f);

		if (grounded)
			rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode2D.Force);
		else if (!grounded)
			rb.AddForce(moveDirection * moveSpeed * 10f * airMultiplier, ForceMode2D.Force);
	}

	private void SpeedControl()
	{
		Vector2 flatVel = new Vector2(rb.velocity.x, 0f);

		if (flatVel.magnitude > moveSpeed)
		{
			Vector2 limitedVel = flatVel.normalized * moveSpeed;
			rb.velocity = new Vector2(limitedVel.x, rb.velocity.y);
		}
	}

	private void Jump()
	{
		rb.velocity = new Vector3(rb.velocity.x, 0f, 0f);

		rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

		currSprintEnergy -= jumpStamCost;
	}

	private void ResetJump()
	{
		readyToJump = true;
	}

	private void StartSprintRegen()
	{
		sprintCanRegen = true;
	}
}
