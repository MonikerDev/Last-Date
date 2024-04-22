using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaypointController : MonoBehaviour
{
    [Header("Location Details")]
    public Waypoint from;
    public Waypoint to;

    [Header("Collision")]
    Rigidbody2D rb;
    bool playerTouched;

    private void Awake()
    {
        if(this.from == GlobalVariableStorage.returnPoint)
        {
            MovePlayertoWaypoint();
        }
    }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PhysicsBasedPlayerController>().canCollide)
            {
                MovePlayertoWaypoint();
            }
        }
    }

    private void MovePlayertoWaypoint()
    {
        GlobalVariableStorage.instance.playerLoc = to.location;
        GlobalVariableStorage.returnPoint = this.from;

        GlobalVariableStorage.playerInstance.TriggerIFrames();

        SceneManager.LoadScene(to.scene);
    }
}
