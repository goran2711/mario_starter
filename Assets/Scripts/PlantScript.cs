using UnityEngine;
using System.Collections;

public class PlantScript : MonoBehaviour
{

    // variables taken from CharacterController.Move example script
    // https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
    public float speed = 6.0F;
      
    
    GameObject playerGameObject; // this is a reference to the player game object
       

    public Transform farEnd;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private float secondsForOneLength = 3f;

    void Start()
    {
        // find the player game object in the scene
        playerGameObject = GameObject.FindGameObjectWithTag("Player");


        startPoint = transform.position;
        endPoint = farEnd.position;
    }

    public void Reset()
    {
        // reset the enemy position to the start position
        transform.position = startPoint;

        // reset the movement direction
        //direction = start_direction;
    }

    void Update()
    {
        // get the character controller attached to the enemy game object
        CharacterController controller = GetComponent<CharacterController>();


        transform.position = Vector3.Lerp(startPoint, endPoint,
         Mathf.SmoothStep(0f, 1f,
          Mathf.PingPong(Time.time / secondsForOneLength, 1f)
        ));
    }

    //
    // This function is called when a CharacterController moves into an object
    //
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Player"))
        {
            // we've hit the player

            // get player script component
            Player playerComponent = playerGameObject.GetComponent<Player>();

            // remove a life from the player
            playerComponent.Lives = playerComponent.Lives - 1;

            // reset the player
            playerComponent.Reset();

            // reset the enemy
            Reset();
        }
    }
}