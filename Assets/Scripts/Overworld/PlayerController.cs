using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movSpeed = 5;
    public static bool canMove = true;

    // Update is called once per frame
    void Update()
    {
		if (canMove)
		{
            transform.Translate(Time.deltaTime * movSpeed * Input.GetAxis("Horizontal"), 0, 0);

            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.Translate(0, 0.5f * Time.deltaTime * movSpeed * Input.GetAxis("Vertical"), 0);
            }
            else
            {
                transform.Translate(0, Time.deltaTime * movSpeed * Input.GetAxis("Vertical"), 0);
            }
        }
    }
}
