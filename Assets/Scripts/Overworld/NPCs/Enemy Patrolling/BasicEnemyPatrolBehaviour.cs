using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyPatrolBehaviour : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed; //Current speed
    public float walkSpeed;  //Base move speed
    public float groundDrag; //Friction
    public float patrolDir;

    [Header("Slow Walking")]
    public float slowWalkSpeed; //Speed when searching
                                //Requires player to have been spotted

    [Header("Sprinting")]
    public float sprintSpeed; //Speed when chasing
    public EnemyState state; // Current movement state
    private EnemyState prevState;
    public float idleTimer;
    public float idleSeconds;

    [Header("Patrolling")]
    public Vector3 roamingPoint; //Center point for roaming
    public float roamDistance; // How far to go from point

    [Header("Vision")]
    public float visionRadius;
    public bool seesPlayer = false;
    public Vector3 playerLoc;

    //Enemy saves heard position
    //And roams near it
    [Header("Hearing")]
    public float hearingRadius;
    public bool hearsPlayer = false;
    public Vector3 heardPlace;
    public Vector3 placeHolder; //hehe puns
    
    [Header("Searching")]
    private float currSearchTime;
    public float maxSearchTime;

    Rigidbody2D rb;

    public enum EnemyState
    {
        idle, //Default state
        patrolling, //Roam within a certain distance of a point
        returning, //Return to patrol point
        chasing, //Spotted player and give chase
        searching //Implement this last as it is advancing on the others
    }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        
        if(roamingPoint == null)
        {
            roamingPoint = this.transform.position;
        }

        placeHolder = roamingPoint;

        patrolDir = 1;

        rb.drag = groundDrag;
    }

    private void Update()
    {
        StateHandler();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        Patrol();
    }

    private void StateHandler()
    {
        //There may be a more efficient
        //Place for this
        Flip();

        if(state == EnemyState.idle)
        {
            moveSpeed = 0;

            if (idleTimer <= 0)
            {
                switch (prevState)
                {
                    case EnemyState.patrolling:
                        state = EnemyState.returning;
                        break;
                    case EnemyState.returning:
                        state = EnemyState.patrolling;
                        break;
                    case EnemyState.chasing:
                        state = EnemyState.searching;
                        break;
                    default:
                        state = EnemyState.patrolling;
                        break;
                }

                idleTimer = idleSeconds;
            }
            else if(seesPlayer || hearsPlayer)
            {
                state = EnemyState.chasing;
            }
            else
            {
                idleTimer -= Time.deltaTime;
            }
        }
        else if(state == EnemyState.patrolling)
        {
            moveSpeed = walkSpeed;
            patrolDir = 1;

            if (transform.position.x - roamingPoint.x >= roamDistance)
            {
                prevState = EnemyState.patrolling;
                state = EnemyState.idle;
            }
            else if(this.seesPlayer)
            {
                state = EnemyState.chasing;
            }
            else if (this.hearsPlayer)
            {
                state = EnemyState.searching;
            }
        }
        else if(state == EnemyState.returning)
        {
            moveSpeed = walkSpeed;
            patrolDir = -1;

            if (transform.position.x + roamingPoint.x <= (-1 * roamDistance))
            {
                prevState = EnemyState.returning;
                state = EnemyState.idle;
            }
            else if (this.seesPlayer)
            {
                state = EnemyState.chasing;
            }
            else if (this.hearsPlayer)
            {
                state = EnemyState.searching;
            }
        }
        else if(state == EnemyState.chasing)
        {
            moveSpeed = sprintSpeed;
            
            if(Vector3.Distance(playerLoc, this.transform.position) < 1f 
                && !seesPlayer)
            {
                this.state = EnemyState.idle;
                this.prevState = EnemyState.chasing;
            }
        }
        else if(state == EnemyState.searching)
        {
            //this DOES NOT WORK
            placeHolder = roamingPoint;
            roamingPoint = heardPlace;
            state = EnemyState.patrolling;
        }

        if(roamingPoint != placeHolder)
        {
            currSearchTime -= Time.deltaTime;

            if(currSearchTime <= 0)
            {
                roamingPoint = placeHolder;
                currSearchTime = maxSearchTime;
            }
        }
    }

    private void Patrol()
    {
        Vector2 moveDirection = new Vector2(patrolDir, 0f);
        rb.AddForce(moveDirection * moveSpeed * 10f, ForceMode2D.Force);
    }

    private void SpeedControl()
    {
        Vector2 flatVel = new Vector2(rb.velocity.x, 0f);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector2 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector2(limitedVel.x, rb.velocity.y);
        }
    }

    //Was overthinking this
    public void SpotPlayer(Vector3 location)
    {
        this.seesPlayer = true;
        playerLoc = location;
    }

    //Was overthinking this
    public  void HearPlayer(Vector3 location)
    {
        this.hearsPlayer = true;
        heardPlace = location;
    }

    //Helps for readability :)
    public void LostPlayer()
    {
        this.seesPlayer = false;
        this.hearsPlayer = false;
    }

    private void Flip()
    {
        this.transform.localScale = new Vector3(patrolDir, 
            this.transform.localScale.y, this.transform.localScale.z);
    }
}
