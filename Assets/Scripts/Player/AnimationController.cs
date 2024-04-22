using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator anim;
    PhysicsBasedPlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        player = this.GetComponent<PhysicsBasedPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.state)
        {
            case (PhysicsBasedPlayerController.MovementState.still):
                anim.ResetTrigger("isWalking");
                anim.ResetTrigger("isRunning");
                anim.ResetTrigger("isCrouching");
                anim.ResetTrigger("isCrouchWalking");
                anim.ResetTrigger("isSlowWalking");
                anim.SetTrigger("isStill");
                break;
            case (PhysicsBasedPlayerController.MovementState.walking):
                anim.ResetTrigger("isStill");
                anim.ResetTrigger("isRunning");
                anim.ResetTrigger("isCrouching");
                anim.ResetTrigger("isCrouchWalking");
                anim.ResetTrigger("isSlowWalking");
                anim.SetTrigger("isWalking");
                break;
            case (PhysicsBasedPlayerController.MovementState.sprinting):
                anim.ResetTrigger("isWalking");
                anim.ResetTrigger("isStill");
                anim.ResetTrigger("isCrouching");
                anim.ResetTrigger("isCrouchWalking");
                anim.SetTrigger("isRunning");
                break;
            case (PhysicsBasedPlayerController.MovementState.crouching):
                anim.ResetTrigger("isWalking");
                anim.ResetTrigger("isRunning");
                anim.ResetTrigger("isStill");
                anim.ResetTrigger("isCrouchWalking");
                anim.ResetTrigger("isSlowWalking");
                anim.SetTrigger("isCrouching");
                break;
            case (PhysicsBasedPlayerController.MovementState.crouchWalking):
                anim.ResetTrigger("isWalking");
                anim.ResetTrigger("isRunning");
                anim.ResetTrigger("isStill");
                anim.ResetTrigger("isCrouching");
                anim.ResetTrigger("isSlowWalking");
                anim.SetTrigger("isCrouchWalking");
                break;
            case (PhysicsBasedPlayerController.MovementState.slowWalking):
                anim.ResetTrigger("isWalking");
                anim.ResetTrigger("isRunning");
                anim.ResetTrigger("isStill");
                anim.ResetTrigger("isCrouching");
                anim.ResetTrigger("isCrouchWalking");
                anim.SetTrigger("isSlowWalking");
                break;
        }
    }
}
