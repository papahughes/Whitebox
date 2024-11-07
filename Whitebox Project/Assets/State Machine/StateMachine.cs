using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
* thinking i need an intermediate state for the player, something just called PlayerState 
*  -this is apparently called a super state.
* would just have a reference to the Player script, so things like input would be able to be used in each class.
* should be able to do this by just having the script, get the component of the Player that is on the game object in the start function.
*  -Problem is start is not being used so when do we get a reference to the function.
*  -State could inherit from monobehavior and then could have it.
*  ***Or not, could do input handling and all that things in here.
*  ***Would just be function that would be called in the update of the ground state.
*  
*  ***Look back in the programming patterns and see if i can try to handle input in the same way that it did.
*  ***End result probably doesnt matter.
*  
*  This is how it was done in GD script, but should ask austin about what he does.
*  -though i think he uses something he got off the asset store.
*/

//Pass in reference to player in each state.
//Set player in the start of the statemachine class.
//Then can just call fucntions from the player, like get input.
//For state example, the state class has a handle input function, this class would be overwritten by the children.

//Could also have the base state use Unity and inherit monodevlope.
//this would allow me to get input, however do not think this would be the best approach for using the input system.
//Could delegate a script for just getting input, but all of that could just be kept in the player.


public class StateMachine : MonoBehaviour
{
    //The current state the state machine is in.
    private PlayerState state;

    //States for the player.
    public PlayerStateGround ground_state = new PlayerStateGround();
    public PlayerStateAir air_state = new PlayerStateAir();

    //Reference to player.
    //Make sure each state has a reference to the player.
    //Should allow each state to affect the player, whether that be by getting input or causing the player to move.
    private Player player;

    //Debug
    //public TMP_Text debug_text;

    // Start is called before the first frame update
    void Start()
    {
        //Get Components
        player = GetComponent<Player>();
        
        //Set the starting state for the player.
        state = ground_state;

        //Enter the current state.
        state.Enter(this, player);
    }

    // Update is called once per frame
    void Update()
    {
        state.Update(this, player);

        //Tell the state to run it's debug function.
        //state.DebugInfo(debug_text, player);
    }

    private void FixedUpdate()
    {
        state.FixedUpdate(this, player);
    }

    public void SwitchState(PlayerState current_state, PlayerState new_state)
    {
        //Exit the current state.
        current_state.Exit(this, player);

        //Change what the current state is to the new state.
        state = new_state;

        //Enter the new state.
        state.Enter(this, player);
    }
}
