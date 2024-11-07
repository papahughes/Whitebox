public abstract class State
{
    /*
     * Since not including unity engine, cannot use cannot use any of the classes.
     * Since not inheriting from Monobehvaior cant use print.
     * However since not iheriting can call fucntions Update and FixedUpdate.
    */

    //Method for when the state machine transitions to a new state.
    public virtual void Enter(StateMachine state_machine, Player player)
    {

    }

    //Method for when the state machine exits the current state.
    public virtual void Exit(StateMachine state_machine, Player player)
    {

    }

    //Runs during the state machine's update method.
    public virtual void Update(StateMachine state_machine, Player player)
    {

    }

    //Runs during the state machine's fixed update method.
    public virtual void FixedUpdate(StateMachine state_machine, Player player)
    {
        
    }

    //Might also need to include methods for Collisions.
}
