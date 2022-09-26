using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class MuletillaController : MonoBehaviour {

	public float 	maxTime;
	public float 	currentTime;
	public int		steps;
	public float 	errorRangePercent;
	public Text		backText;
	public Text		mainText;
	public AudioSource music, fx;
	public Animator anim;

	public Color32 	colorToChange;

	[HideInInspector]
	public float currentValue;

	private float timeToFill;

	private Image myBar;

	private Color startColorText;

	private int seg;

	private int currentGesto;

	private float timePushed;

	private float timeToPerfect;

	private bool play;

	void Start () 
	{
		myBar				= GetComponent<Image>();
		currentTime 		= 0;
		timeToPerfect		= maxTime/(float)steps;	
		startColorText		= backText.color;
		currentGesto		= 1;
		seg = 0;
	}

	void Reset()
	{
		currentTime 	= 0;
		currentGesto	= 1;
		currentValue   	= 0;
		seg				= 0;
		timeToPerfect	= maxTime/steps;
		backText.color		= startColorText;
		myBar.fillAmount	= 0;
		play = false;
	}
	// Update is called once per frame
	void LateUpdate () 
	{
		if(music.isPlaying)
		{
			seg = (int)music.time;
			timePushed += Time.deltaTime;
			if(seg % 2 == 1)
			{
				if(music.time >= seg && !play && timePushed >= 1)
				{
					play = true;
					Debug.Log("Play");
				}
			}

			if(play)
			{
				currentTime 	+= 	Time.deltaTime;
				currentValue	=	currentTime/maxTime;

				if(currentTime > timeToPerfect - errorRangePercent)
				{
					backText.color = colorToChange;
				}
				//Debug.Log((int)music.time +"--"+ music.time);

				if(currentTime >= maxTime || currentTime > timeToPerfect + errorRangePercent)
				{
					Reset();
				}
				myBar.fillAmount	 	= currentValue;
			}
			backText.text			= mainText.text;
		}
	}

	public bool IsCorrect()
	{
		bool correct = false;

		if(currentTime <= timeToPerfect + errorRangePercent && currentTime >= timeToPerfect - errorRangePercent)
		{
			correct = true;
			timeToPerfect++;
			backText.color = startColorText;
			anim.SetTrigger("Gesto"+ currentGesto);
			currentGesto++;
			if(currentGesto == 4)
				currentGesto = 1;
		}
		else
		{
			anim.SetTrigger("Toser");
			Reset();
			timePushed = 0;
		}
		return correct;
	}
}
