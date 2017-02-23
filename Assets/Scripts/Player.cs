using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	// variables taken from CharacterController.Move example script
	// https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;

	public int Lives = 3; // number of lives the player hs

    public bool Touched { get; set; }

    public List<GameObject> disabledObjects;

	Vector3 start_position; // start position of the player


	void Start()
	{
		// record the start position of the player
		start_position = transform.position;

        Touched = false;
	}

	public void Reset()
	{
		// reset the player position to the start position
		transform.position = start_position;

        foreach (GameObject g in disabledObjects)
            g.SetActive(true);

	}
    public void Reset(GameObject go)
    {
        Reset();
        if (go.CompareTag("Goomba"))
            go.GetComponent<Enemy>().Reset();
        if (go.CompareTag("Piranha"))
            go.GetComponent<PlantScript>().Reset();
    }

	void Update()
	{
		// get the character controller attached to the player game object
		CharacterController controller = GetComponent<CharacterController>();


        if (transform.position.z != 0)
        {
            Vector3 pos;
            pos = new Vector3(transform.position.x, transform.position.y, 0);
            transform.position = pos;
        }

		// check to see if the player is on the ground
		if (controller.isGrounded) 
		{
            if (Touched)
                Touched = false;

			// set the movement direction based on user input and the desired speed
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;

            // check to see if the player should jump
            if (Input.GetButton("Jump"))
                JumpPlayer();
		}

		// apply gravity to movement direction
		moveDirection.y -= gravity * Time.deltaTime;

		// make the call to move the character controller
		controller.Move(moveDirection * Time.deltaTime);
    }


    public void JumpPlayer()
    {
        moveDirection.y = jumpSpeed;
    }

    public void DropPlayer()
    {
        moveDirection.y = 0;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Block"))
        {
            HitBlock(hit);
        }
        else if (hit.collider.gameObject.CompareTag("Pipe"))
        {
            HitBlock(hit);
        }
        else if (hit.collider.gameObject.CompareTag("Goomba"))
        {
            Attacked(hit.collider.gameObject);
        }
        else if (hit.collider.gameObject.CompareTag("Piranha"))
        {
            Attacked(hit.collider.gameObject);
        }
    }

    private void HitBlock(ControllerColliderHit hit)
    {
        if (hit.point.y > gameObject.transform.position.y)
        {
            if (Touched == false)
            {
                DropPlayer();
                Touched = true;
            }
        }
    }

    void Attacked(GameObject go)
    {
        Reset(go);
        
        Lives--;
    }
}