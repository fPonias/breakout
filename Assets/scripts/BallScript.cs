using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Ball controller Script.
 * handles ball behaviour on collision and provides a reset for the main controller.
 */
public class BallScript : MonoBehaviour 
{
	private MainScript parent;

	private Rigidbody2D _physics;
	public Rigidbody2D physics
	{
		get {return _physics;}
	}
	private CircleCollider2D collider;
	private float rotation;
	private const float MAX_ROTATION = 70.0f;
	private Vector2 defaultDir = new Vector2(0, 7f);
	
	private List<PowerupScript> powerups;
	private List<float> powerupEnd;

	// Use this for initialization
	void Start () 
	{
		powerups = new List<PowerupScript> ();
		powerupEnd = new List<float> ();

		hasPowerball = false;
		isColliding = false;

		GameObject parentObj = GameObject.FindGameObjectWithTag ("GameController");
		parent = parentObj.GetComponent<MainScript> ();

		_physics = GetComponent<Rigidbody2D> ();
		this.collider = (CircleCollider2D) base.collider2D;

		Reset ();
	}

	//move the ball back to the center and pick a direction to fling it in
	public void Reset()
	{
		float offY = 0.0f;

		rotation = Random.Range (-30.0f, 30.0f);
		offY = -4f;

		_physics.velocity = Quaternion.AngleAxis(rotation, new Vector3(0, 0, -1.0f)) * defaultDir;
		_physics.MovePosition (new Vector2(0, offY));
		_physics.isKinematic = false;
	}

	private bool isColliding = false;

	public void OnCollisionEnter2D(Collision2D coll)
	{
		isColliding = true;
		coll.gameObject.audio.Play ();
		_lastHit = coll.gameObject.tag;
	}

	public void OnCollisionExit2D(Collision2D coll)
	{
		isColliding = false;
	}

	private Vector2 _lastVelocity;
	public Vector2 lastVelocity
	{
		get {return _lastVelocity;}
	}

	private string _lastHit;

	public void addPowerup(PowerupScript powerup)
	{
		powerups.Add (powerup);
		powerupEnd.Add (Time.timeSinceLevelLoad + 5.0f);
		
		calculatePowerups ();
	}

	private bool hasPowerball;

	private void calculatePowerups()
	{
		hasPowerball = false;

		int sz = powerups.Count;
		for (int i = 0; i < sz; i++)
		{
			PowerupScript powerup = powerups[i];
			if (powerup != null && powerup.type == PowerupScript.Type.POWER_BALL)
				hasPowerball = true;
		}
	}


	public void Update()
	{
		if (!isColliding)
			_lastVelocity = _physics.velocity;

		//check if the ball went out of bounds, update the score and reset the ball position
		if (physics.position.y < parent.bottom)
		{
			parent.BallOut(gameObject);
		}

		//see if any powerups have died
		int sz = powerups.Count;
		for (int i = 0; i < sz; i++)
		{
			float death = powerupEnd[i];
			
			if (death < Time.timeSinceLevelLoad)
			{
				powerups.RemoveAt(i);
				powerupEnd.RemoveAt(i);
				
				calculatePowerups();
			}
		}

		if (hasPowerball)
		{
			float dot = Vector2.Dot (_lastVelocity, physics.velocity);
			if (_lastHit == "brick" && _lastVelocity != physics.velocity)
			{
				physics.velocity = _lastVelocity;
			}
		}
	}
}
