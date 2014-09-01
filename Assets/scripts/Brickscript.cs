using UnityEngine;
using System.Collections;

public class Brickscript : MonoBehaviour 
{
	private GameObject[] bricks;
	private MainScript parent;

	private float deathTime;
	private float deathLength;

	// Use this for initialization
	public void Start () 
	{
		deathTime = -1.0f;
		deathLength = 0.2f;
		GameObject parentObj = GameObject.FindGameObjectWithTag ("GameController");
		parent = parentObj.GetComponent<MainScript> ();
	}

	public void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "ball")
		{
			if (deathTime == -1.0f)
			{
				parent.BrickHit (gameObject);
			}
		}
	}

	public void Kill()
	{
		Destroy(gameObject);
		//rigidbody2D.isKinematic = true;
		//deathTime = Time.realtimeSinceStartup + deathLength;
	}

	// Update is called once per frame
	public void Update () 
	{
		if (deathTime > -1.0f)
		{
			SpriteRenderer r = GetComponent<SpriteRenderer> ();
			float transparency = 0.5f * ((deathTime - Time.realtimeSinceStartup) / deathLength);
			r.color = new Color(r.color.r, r.color.g, r.color.b, transparency);

			if (Time.realtimeSinceStartup > deathTime)
				Destroy(gameObject);
		}
	}
}
