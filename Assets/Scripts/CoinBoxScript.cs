using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBoxScript : MonoBehaviour {

    public int maxCoinAmount;
	public int amtOfCoins;
	private bool isHit;
	public float coolDownTime;
	public float bounceAmt;
	private float coolDownTimer;
	private Vector3 originalPos;
	public GameObject coinPrefab;
	private GameObject currentCoin;
	private bool isPlayerTouchingBottom;

    public GameObject playerObject;
    private Player m_player;

	// Use this for initialization
	void Start () {
		isHit = false;
		isPlayerTouchingBottom = false;
		coolDownTimer = 0.0f;
		originalPos = transform.position;

        amtOfCoins = maxCoinAmount;

        m_player = playerObject.GetComponent<Player>();
    }

    void Reset()
    {
        isHit = false;
        isPlayerTouchingBottom = false;
        coolDownTimer = 0.0f;
        originalPos = transform.position;

        amtOfCoins = maxCoinAmount;
    }

    void OnEnable()
    {
        Reset();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (isHit)
        {
			transform.position = originalPos + new Vector3 (0.0f, Mathf.Sin ((coolDownTimer / coolDownTime) * Mathf.PI) * bounceAmt, 0.0f);

			coolDownTimer -= Time.deltaTime;
			if (coolDownTimer <= 0.0f)
{
				coolDownTimer = 0.0f;
				isHit = false;
				transform.position = originalPos;
				Destroy (currentCoin);

                if (amtOfCoins == 0)
                    gameObject.SetActive(false);
            }
		}
        else
		{
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
                        if (m_player.Touched)
                        {
                            m_player.DropPlayer();
                            m_player.Touched = false;
                        }
                    }
				}

				Debug.DrawRay(ray.origin, ray.direction, Color.red);
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (amtOfCoins > 0)
		{
			if (isHit == false)
			{
				if(isPlayerTouchingBottom)
				{
					isHit = true;
					isPlayerTouchingBottom = false;
					coolDownTimer = coolDownTime;

					currentCoin = Instantiate(coinPrefab, transform.position, coinPrefab.transform.rotation, transform);
					currentCoin.transform.Translate (new Vector3 (0.0f, 1.0f, 0.0f), Space.World);

					amtOfCoins--;
				}
			}
		}
	}
}
