using UnityEngine;
using System.Collections;

public class PowerupScript : MonoBehaviour 
{
	public enum Type
	{
		NONE,
		EXTRA_BALL,
		BIGGER_PADDLE,
		SMALLER_PADDLE,
		SLOWER_BALL,
		LASERS,
		EXTRA_LIFE,
		POWER_BALL
	};

	private static Sprite extraBallSprite;
	private static Color extraBallColor;
	private static Sprite expandSprite;
	private static Color expandColor;
	private static Sprite shrinkSprite;
	private static Color shrinkColor;
	private static Sprite powerSprite;
	private static Color powerColor;
	private static Sprite oneupSprite;
	private static Color oneupColor;
	
	private Type _type = Type.NONE;
	public Type type
	{
		get { return _type; }
		set 
		{ 
			_type = value; 
			UpdateImage();
		}
	}

	private Rigidbody2D physics;

	private MainScript parent;

	// Use this for initialization
	void Start () 
	{
		if (extraBallSprite == null)
		{	
			Texture2D text = Resources.Load<Texture2D> ("png/powerup-B");
			extraBallSprite = Sprite.Create(text, new Rect(0.0f, 0.0f, text.width, text.height), Vector2.zero);
			extraBallColor = new Color(0.3f, 0.3f, 0.3f);

			text = Resources.Load <Texture2D>("png/powerup-Ex");
			expandSprite = Sprite.Create(text, new Rect(0.0f, 0.0f, text.width, text.height), Vector2.zero);
			expandColor = new Color(0.6f, 0.3f, 0.3f);

			text = Resources.Load <Texture2D>("png/powerup-Im");
			shrinkSprite = Sprite.Create (text, new Rect(0.0f, 0.0f, text.width, text.height), Vector2.zero);
			shrinkColor = new Color(0.3f, 0.6f, 0.6f);

			text = Resources.Load <Texture2D>("png/powerup-Boom");
			powerSprite = Sprite.Create (text, new Rect(0.0f, 0.0f, text.width, text.height), Vector2.zero);
			powerColor = new Color(0.3f, 0.3f, 0.6f);

			text = Resources.Load <Texture2D>("png/powerup-1up");
			oneupSprite = Sprite.Create (text, new Rect(0.0f, 0.0f, text.width, text.height), Vector2.zero);
			oneupColor = new Color(0.6f, 0.3f, 0.6f);
		}

		GameObject parentObj = GameObject.FindGameObjectWithTag ("GameController");
		parent = parentObj.GetComponent<MainScript> ();
		physics = GetComponent<Rigidbody2D> ();


		if (type == Type.NONE)
			type = Type.EXTRA_BALL;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (physics.position.y <= parent.bottom)
		{
			parent.PowerupOut(gameObject);
		}
	}

	private void UpdateImage()
	{
		SpriteRenderer r = GetComponent<SpriteRenderer> ();

		if (_type == Type.EXTRA_BALL)
		{
			r.sprite = extraBallSprite;
			r.color = extraBallColor;
		}
		else if (_type == Type.BIGGER_PADDLE)
		{
			r.sprite = expandSprite;
			r.color = expandColor;
		}
		else if (_type == Type.SMALLER_PADDLE)
		{
			r.sprite = shrinkSprite;
			r.color = shrinkColor;
		}
		else if (_type == Type.POWER_BALL)
		{
			r.sprite = powerSprite;
			r.color = powerColor;
		}
		else if (_type == Type.EXTRA_LIFE)
		{
			r.sprite = oneupSprite;
			r.color = oneupColor;
		}
	}
}
