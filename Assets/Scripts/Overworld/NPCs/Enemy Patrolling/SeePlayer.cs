using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePlayer : MonoBehaviour
{
    //Thing Seeing Player
    public BasicEnemyPatrolBehaviour host;

    //Collider
    BoxCollider2D bc;

    //Range
    private float radius;

    //Tweaking
    public float Xoffset;
    public float Yoffset;

    // Start is called before the first frame update
    void Start()
    {
        radius = host.visionRadius;

        bc = this.GetComponent<BoxCollider2D>();

        bc.size = new Vector3(radius * 2.5f, radius / 2, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = host.transform.position;
        bc.offset = new Vector2(radius - Xoffset, Yoffset);
    }

    //If brief glimpse, should be different on hold
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //Spot player and start chasing even if they leave vision radius
            host.SpotPlayer(collision.transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //If they stay within continue to update their position
            host.SpotPlayer(collision.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //If lost player continue to last place they
            //were seen and look around
            host.LostPlayer();
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawCube(new Vector3(transform.position.x + radius, transform.position.y, transform.position.z), new Vector3(2 * radius, radius, 0f));
    //}
}
