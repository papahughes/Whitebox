using UnityEngine;
using TMPro;

public class PlayerStateAir : PlayerState
{
    //Air State for the Player

    //Variable for air movement.
    private float max_speed = 10;
    private float accelleration = 2f;
    private float friction = 1.5f;
    private float gravity = -2f;
    


    public override void DebugInfo(TMP_Text debug_text, Player player)
    {
        //Output debug information for ground state.
        debug_text.text = "State: Air";
        debug_text.text += "\nInput: " + player.move_input_vec.ToString();
        debug_text.text += "\nVelocity: " + player.velocity.ToString();
        debug_text.text += "\nSpeed: " + new Vector2(player.velocity.x, player.velocity.z).magnitude;
    }

    public override void Enter(StateMachine state_machine, Player player)
    {
        //Debug.Log("Ground State Enter");
    }

    public override void Exit(StateMachine state_machine, Player player)
    {
        //Debug.Log("Ground State Exit");
    }

    public override void Update(StateMachine state_machine, Player player)
    {
        //Debug.Log("Ground State Update");

        //Rotate the player model.
        RotatePlayerModel(player);
    }

    public override void FixedUpdate(StateMachine state_machine, Player player)
    {
        //Debug.Log("Ground State FixedUpdate");

        //Movement in FixedUpdate to make it in sync with the Physics Step.

        //Accelerate the player.
        player.velocity += (player.model_transform.right * player.move_input_vec.x + player.model_transform.forward * player.move_input_vec.y) * accelleration;

        //Sepperate out the x and z velocity from the total velocity.
        Vector2 xz_vel = new Vector2(player.velocity.x, player.velocity.z);

        //Cap speed.
        xz_vel = Vector2.ClampMagnitude(xz_vel, max_speed);

        //Friction
        if (player.move_input_vec == Vector2.zero)
        {
            xz_vel = Vector2.MoveTowards(xz_vel, Vector2.zero, friction);
        }

        //Gravity
        player.velocity.y += gravity; //Add gravity to the y component of the velocity.

        //Reconstruct Velocity.
        player.velocity = new Vector3(xz_vel.x, player.velocity.y, xz_vel.y);

        //Move Player
        player.controller.Move(player.velocity * Time.fixedDeltaTime);

        //Change States
        if (player.controller.isGrounded)
        {
            state_machine.SwitchState(this, state_machine.ground_state);
        }
            
    }
}
