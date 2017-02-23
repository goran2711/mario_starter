using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBlockScript : MonoBehaviour {
	// Use this for initialization
	private bool isPlayerTouchingBottom;

    public GameObject playerObject;
    private Player m_player;

    void Start ()
    {
		isPlayerTouchingBottom = false;

        m_player = playerObject.GetComponent<Player>();
    }

    void Reset()
    {
        isPlayerTouchingBottom = false;

        m_player = playerObject.GetComponent<Player>();
    }

    void OnEnable()
    {
        Reset();
    }
	
	// Update is called once per frame
	void Update () {

		isPlayerTouchingBottom = false;

		for (int i = 0; i < 4; ++i)
		{
			Ray ray = new Ray (transform.position - new Vector3 ((transform.localScale.x / 2.0f) -(float)i * (transform.localScale.x / 3.0f), (transform.localScale.y / 2.0f), 0.0f), new Vector3(0.0f, -0.2f, 0.0f));
			RaycastHit hit = new RaycastHit();

			if (Physics.Raycast (ray, out hit, 1.0f))
			{
				if (hit.collider.CompareTag("Player"))
				{
					isPlayerTouchingBottom = true;
                }
			}

			Debug.DrawRay(ray.origin, ray.direction, Color.red);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			if(isPlayerTouchingBottom)
			{
                m_player.DropPlayer();
                gameObject.SetActive(false);
				//Destroy (gameObject);
			}
		}
	}
}
