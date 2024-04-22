using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWanderBehaviour : MonoBehaviour
{
    [Header("Wandering Entity")]
    public GameObject npc;

    [Header("Boundaries")]
    public float wanderRadius;
    public Vector2 patrolPoint;

    [Header("Movement")]
    public float moveSpeed;
    private float movDir = 1;

    Rigidbody2D rb;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        patrolPoint = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessDir();
    }

    private void FixedUpdate()
    {
        MoveNPC();
    }

    //Apply force
    void MoveNPC()
    {
        Vector2 moveDir = new Vector2(movDir, 0);
        rb.AddForce(moveDir * moveSpeed * 10f, ForceMode2D.Force);
    }

    //If leaving bounds, flip
    void ProcessDir()
    {
        if ((this.transform.position.x > (patrolPoint.x + wanderRadius)) && movDir == 1 
            || (this.transform.position.x < (patrolPoint.x - wanderRadius)) && movDir == -1)
        {
            movDir *= -1;
            this.transform.localScale = new Vector3(movDir * this.transform.localScale.x, 
                this.transform.localScale.y, this.transform.localScale.z);
        }
    }
}
