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

    [Header("Player Detection")]
    public float visionRadius;
    public float hearingRadius;

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
        if(state == EnemyState.idle)
        {
            moveSpeed = 0;

            if (idleTimer <= 0)
            {
                this.transform.localScale = new Vector3(patrolDir, this.transform.localScale.y, this.transform.localScale.z);

                switch (prevState)
                {
                    case EnemyState.patrolling:
                        state = EnemyState.returning;
                        break;
                    case EnemyState.returning:
                        state = EnemyState.patrolling;
                        break;
                    default:
                        state = EnemyState.patrolling;
                        break;
                }

                idleTimer = idleSeconds;
            }
            else
            {
                idleTimer -= Time.deltaTime;
            }
        }
        else if(state == EnemyState.patrolling)
        {
            moveSpeed = walkSpeed;

            if(transform.position.x - roamingPoint.x >= roamDistance)
            {
                patrolDir *= -1;
                //state = EnemyState.returning;

                prevState = EnemyState.patrolling;
                state = EnemyState.idle;
            }
        }
        else if(state == EnemyState.returning)
        {
            moveSpeed = walkSpeed;

            if (transform.position.x + roamingPoint.x <= (-1 * roamDistance))
            {
                patrolDir *= -1;
                //state = EnemyState.patrolling;

                prevState = EnemyState.returning;
                state = EnemyState.idle;
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
}
