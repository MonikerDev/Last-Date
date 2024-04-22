using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearPlayer : MonoBehaviour
{
    //Thing Hearing Player
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
        radius = host.hearingRadius;

        bc = this.GetComponent<BoxCollider2D>();

        bc.size = new Vector3(radius * 3, radius / 2, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = host.transform.position;
        bc.offset = new Vector2(Xoffset, Yoffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!GlobalVariableStorage.playerInstance.GetComponent<PhysicsBasedPlayerController>().isQuiet)
            {
                host.HearPlayer(collision.transform.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            host.LostPlayer();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position, new Vector3(2 * radius, radius, 0f));
    //}
}
