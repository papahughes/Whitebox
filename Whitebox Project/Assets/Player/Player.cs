using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /*
       This script i think is going to act like a utlilty for the player.
           A swiss army knife script.
        I think it is just going to store references to things that the player needs.
            Which makes sense.
        Things like the input that the player is receiving.
        The model that the player has.
        The animation controller.
        Things of that nature.
        It should be things that can be accessed by anything that has a reference to the player.
    */

    //***Bet I could make something like this but in thirdperson.
    //***https://www.youtube.com/watch?v=TpF82Il-kXI&t=312s
    //***

    //Input
    [HideInInspector] public Vector2 move_input_vec = Vector2.zero;

    //Moving
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Vector3 velocity;

    //Jumping
    [HideInInspector] public bool jump_button_pressed = false;

    //Camera
    public Transform camera_rig_transform;

    //Model
    public Transform model_transform;

    void Start()
    {
        //Get components.
        controller = GetComponent<CharacterController>();
    }

    public void CaptureMoveInput(InputAction.CallbackContext context)
    {
        move_input_vec = context.ReadValue<Vector2>();
    }

    public void CaptureJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) jump_button_pressed = true;
        if (context.phase == InputActionPhase.Canceled) jump_button_pressed = false;
    }
}
