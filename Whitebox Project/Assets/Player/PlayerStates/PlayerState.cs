using UnityEngine;
using TMPro;

public class PlayerState : State
{
    /*
     * Refered to as a super state.
     * A state that all other player states would inherit from.
     * Useful for sharing functions/variables that all of the player states might need.
    */


    public virtual void DebugInfo(TMP_Text debug_text, Player player)
    {
        //Child states will override this function to display thier own debug information.
    }

    public void RotatePlayerModel(Player player)
    {
        //Make character face in the direction of the camera. Slerped for smooth rotation.
        player.model_transform.rotation = Quaternion.Slerp(player.model_transform.rotation, player.camera_rig_transform.rotation, 10 * Time.deltaTime);
    }
}
