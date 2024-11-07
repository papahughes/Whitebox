using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRig : MonoBehaviour
{
    //***Still dont know why but then the player is selected the camera will move faster in play mode.
    //***Need to ask austin about this.


    //Do rotation with out using accumation angles.
    //Problem was clamping angles, since can't directly access the rotation angle.
    //Need to also find out how the Rotate funtions works.
        //Does it use radians or degrees.

    //Child Objects
        //***Might be better to get the whole game object, but trnasforms are good for now.
    private Transform camera_pivot_transform;
    private Transform camera_transform;

    //Camera Position
    public Transform player_transform;

    //Camera Variables
    [Range(1, 20)]public float distance_from_player = 8f;
    public LayerMask collision_layers; //The layers that the sphere cast can collide with.
    public float camera_speed = 250f; //Amount to rotate the camera in radians.
    [Range(1, 20)] public float camera_follow_speed = 12f;
    private Vector2 camera_input = Vector2.zero;
    private float sum_x_angle = 0f; //Used to accumalte the angle of rotation for the X axis.
    private float sum_y_angle = 0f; //The same except for the y angle.

    //Collision
    private RaycastHit hit;
    Vector3 col_point = Vector3.zero;

    void Start()
    {
        //Hide Mouse and Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Get Child Objects
        camera_pivot_transform = transform.GetChild(0).transform;
        camera_transform = camera_pivot_transform.GetChild(0);

        //Position camera to be at the player's position.
        transform.position = player_transform.position;

        //Position Camera
        camera_transform.transform.localPosition = new Vector3(0, 0, -distance_from_player);
    }

    // Update is called once per frame
    void Update()
    {
        //***Move Camera***
        //If camera is not moving, just return since nothing needs to be calculated.
        if (camera_input == Vector2.zero)
            return;

        //Get the direction that the mouse has moved.
        Vector2 dir = camera_input - Vector2.zero; //Not normalized.

        //If I remember correctly, this is to simualte stick on a controller.
        //Length is the distance that the stick is currently being tilted.
        //Max_Length is the furthest distance the stick can tilt.
        float length = dir.magnitude;
        float max_length = 25;
        if (length > max_length)
            length = max_length;

        //Percent is used to calcualte the speed of the camera.
        //Basically this mimics the how far the player tilted the stick on a controller.
        //If the player tilted the stick 60%, then the speed of the camera should be 60%.
        float percent = length / max_length;

        //Normalize direction.
        dir = dir.normalized;

        //Camera Rig Rotation. (Left/Right)
        float angle = dir.x * (camera_speed * percent) * Time.deltaTime; //Find angle to rotate.
        sum_y_angle += angle; //Add to the sum angle.
        transform.localRotation = Quaternion.Euler(0, sum_y_angle, 0); //Rotate.

        //Camera Pivot Rotation. (Up/Down)
        angle = dir.y * (camera_speed * percent) * Time.deltaTime;
        sum_x_angle -= angle;
        sum_x_angle = Mathf.Clamp(sum_x_angle, -90, 90); //Clamp the angle so can't look below -90 or above 90 degrees.
        camera_pivot_transform.localRotation = Quaternion.Euler(sum_x_angle, 0, 0);
    }

    private void FixedUpdate()
    {
        //We move the camera in fixed update since the player moves in fixed update...
        //And becasue collisions should be done in fixed update.

        //***Camera Position and Collision***
        //***For objects to collide with the camera they must be set to a layer the camera can collide with!***

        //Make camera lag slightly behind the player.
        transform.position = Vector3.Lerp(transform.position, player_transform.position, camera_follow_speed * Time.fixedDeltaTime);

        //Do a raycast to see if colliding...
        Ray ray = new Ray(camera_pivot_transform.position, -camera_pivot_transform.forward);
        //0.5f is the radius used for the sphere cast. It is the player's character controller's radius. 
        //This just ensures that it is small enough to detect a wall even if the player is right up against it.
        //Might want to grab a reference to the player rather than hard code this in but this is good for now.
        //Also need to keep in mind the palyer's skin_width is 0.08, this is the extra distance that keeps the player away from the wall...
        //Making it so the sphere cast will detect the wall. 
        //NOTE: A shape cast will not detect a collider if the shape starts out in that collider.
        if (Physics.SphereCast(ray, 0.5f, out hit, distance_from_player, collision_layers)) 
        {
            //So, if something was detected, just move the position of the camera to the distance of the point of contact, in the direction of the ray.
            camera_transform.position = camera_pivot_transform.position + (hit.distance * -camera_pivot_transform.forward);
            return; //Then return to stop anything else from happening.
        }

        //Otherwise position the camera at the proper distance away from the camera.
        camera_transform.position = camera_pivot_transform.position + (distance_from_player * -camera_pivot_transform.forward);
    }

    private void OnDrawGizmos()
    {
        //NOTE: Use this functions to draw thing in the future. 
        //Way more useful the Debug funtions.

        /*Gizmos.color = Color.red;
        Gizmos.DrawRay(camera_pivot.position, -camera_pivot.forward * distance_from_player);

        Gizmos.DrawSphere(col_point, 0.05f);

        //Visualize Sphere Cast

        //Line shpere cast travels down.
        /*Gizmos.color = Color.blue;
        Gizmos.DrawRay(camera_pivot.position, -camera_pivot.forward * distance_from_player);

        //Sphere starting position.
        Gizmos.DrawWireSphere(camera_pivot.position, sphere_radius);

        //Sphere ending position.
        Gizmos.DrawWireSphere(camera_pivot.position + (distance_from_player * -camera_pivot.forward), sphere_radius);
        
        //Where cast would be if a collision occured.
        if (collided)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.05f); //Show the position at where the sphere collided.
        }*/
    }

    public void CaptureCameraInput(InputAction.CallbackContext context)
    {
        camera_input = context.ReadValue<Vector2>();
    }

    //***Getters***
    public Vector2 GetCameraInput()
    {
        return camera_input;
    }

}
