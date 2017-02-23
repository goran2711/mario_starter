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

    [Header("Debug")]
    public Vector3 _plantMoveVector;
    public Vector3 _startPointVector;
    public Vector3 _endPointVector;


    void Start()
    {
        // find the player game object in the scene
        playerGameObject = GameObject.FindGameObjectWithTag("Player");


        _startPointVector = startPoint = transform.position;
        _endPointVector = endPoint = farEnd.position;


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

        Vector3 moveVector;
        moveVector = _plantMoveVector = Vector3.Lerp(startPoint - transform.position, endPoint - transform.position,
         Mathf.SmoothStep(0f, 1f,
          Mathf.PingPong(Time.time / secondsForOneLength, 1f)
        ));

        controller.Move(moveVector * Time.deltaTime);
    }
}