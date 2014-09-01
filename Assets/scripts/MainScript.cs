using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Main controller to rule them all
 */
public class MainScript : MonoBehaviour 
{
	private GUIText scoreLbl; //global score label
	private GUIText livesLbl;
	private GUIText multiplierLbl;

	private static int lives;
	private static int score;
	private static int multiplier;

	static MainScript()
	{
		lives = 3;
		score = 0;
		multiplier = 0;
	}

	private GameObject ballPrefab;

	private GameObject playerPaddle; //player paddle
	private PaddleScript playerPaddleCtrl;

	private List<GameObject> bricks;
	private GameObject powerupPrefab;

	private List<GameObject> balls;

	private int playerScore;
	private float _bottom = -5.3f;

	public float bottom
	{
		get { return _bottom; }
	}

	// Use this for initialization
	void Start () 
	{
		playerScore = 0;
		multiplier = 1;

		ballPrefab = (GameObject) Resources.Load ("ball");

		scoreLbl = GameObject.FindGameObjectWithTag ("score").guiText;
		livesLbl = GameObject.FindGameObjectWithTag ("lives").guiText;
		multiplierLbl = GameObject.FindGameObjectWithTag ("multiplier").guiText;

		paused = false;

		//setup the ai and controls
		GameObject[] paddles = GameObject.FindGameObjectsWithTag ("paddle");
		playerPaddle = paddles [0];
		playerPaddleCtrl = playerPaddle.GetComponent<PaddleScript> ();
		playerPaddleCtrl.ailevel = 0;

		bricks = new List<GameObject> ();
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("brick");
		foreach (GameObject o in objs)
		{
			bricks.Add(o);
		}

		balls = new List<GameObject> ();
		objs = GameObject.FindGameObjectsWithTag ("ball");
		foreach (GameObject o in objs)
		{
			bricks.Add(o);
		}

		powerupPrefab = Resources.Load<GameObject>("powerup");

		//start by putting the ball in play
		Reset ();
	}
	
	// Update is called once per frame
	public void Update () 
	{
		scoreLbl.text = "" + score;
		livesLbl.text = "x" + lives;
		multiplierLbl.text = "x" + multiplier;

		if (bricks.Count == 0)
		{
			LoadNextLevel();
		}
	}

	private bool _paused;

	public bool paused
	{
		get { return _paused;}
		set 
		{
			_paused = value; 
			Time.timeScale = (_paused) ? 0.0f : 1.0f;
		}
	}

	public void OnGUI ()
	{
		if (GUI.Button(new Rect(Screen.width - 160, 10, 140, 80), "Menu"))
		{
			paused = !paused;
			OnGUI ();
		}

		if (paused)
		{
			string msg = "Paused";

			if (lives == 0)
				msg = "Game Over";

			float x = (Screen.width - 200.0f) / 2.0f;
			float y = (Screen.height - 240.0f) / 2.0f;
			GUI.Box (new Rect(x, y, 200, 240), msg);

			if (GUI.Button (new Rect(x + 10, y + 40, 180, 80), "Restart"))
			{
				Application.LoadLevel(0);

				lives = 3;
				score = 0;
				multiplier = 1;

				Reset ();
				paused = false;
				OnGUI ();
			}

			if (GUI.Button (new Rect(x + 10, y + 130, 180, 80), "Quit"))
			{
				Application.Quit();
			}
		}
	}

	private void LoadNextLevel()
	{
		if (Application.loadedLevel < Application.levelCount - 1)
			Application.LoadLevel (Application.loadedLevel + 1);
	}

	private float powerupProbability = 0.3f;
	private bool brickHit = false;

	public void BrickHit(GameObject brickObj)
	{
		bricks.Remove (brickObj);
		Brickscript brick = brickObj.GetComponent<Brickscript> ();

		if (Random.Range(0.0f, 1.0f) <= 0.2f)//<= powerupProbability)
		{
			Vector2 pos = brickObj.transform.position;
			BoxCollider2D collider = (BoxCollider2D) brickObj.collider2D;
			pos -= collider.size / 2.0f;

			GameObject powerupObj = (GameObject) Instantiate(powerupPrefab, pos, Quaternion.identity);
			PowerupScript powerup = powerupObj.GetComponent<PowerupScript>();

			float rand = Random.Range(0.0f, 5.0f);
			int pick = (int) rand;

			switch(pick)
			{
			case 1:
				powerup.type = PowerupScript.Type.EXTRA_BALL;
				break;
			case 2:
				powerup.type = PowerupScript.Type.SMALLER_PADDLE;
				break;
			case 3:
				powerup.type = PowerupScript.Type.POWER_BALL;
				break;
			case 4:
				powerup.type = PowerupScript.Type.EXTRA_LIFE;
				break;
			default:
				powerup.type = PowerupScript.Type.BIGGER_PADDLE;
				break;
			}

		}

		brick.Kill ();
		brickHit = true;
		score += 100 * multiplier;
	}

	public void PaddleHit(GameObject paddleObj)
	{
		if (brickHit == true)
			multiplier++;
		else
			multiplier = 1;

		brickHit = false;
	}

	public void AddBall()
	{
		GameObject ballObj = (GameObject) Instantiate (ballPrefab, Vector3.zero, Quaternion.identity);
		BallScript ball = ballObj.GetComponent<BallScript>();

		balls.Add (ballObj);
	}

	public void AddLife()
	{
		lives++;
	}

	public void PowerupBalls(PowerupScript powerup)
	{
		foreach (GameObject ballobj in balls)
		{
			BallScript ball = ballobj.GetComponent<BallScript> ();
			ball.addPowerup(powerup);
		}
	}

	public void BallOut(GameObject ballObj)
	{
		balls.Remove (ballObj);
		Destroy (ballObj);

		if (balls.Count == 0)
		{
			lives--;
			multiplier = 1;

			if (lives > 0)
				AddBall ();
			else
			{
				paused = true;	
				OnGUI ();
			}
		}
	}

	public void PowerupOut(GameObject powerup)
	{
		Destroy (powerup);
	}

	private void Reset()
	{
		foreach (GameObject ballObj in balls)
		{
			Destroy (ballObj);
		}

		AddBall ();
	}
}
