using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {

	public MuletillaController 	sliderMuletilla;
	public Text					pointsText;
	public DialogManager		dialogManager;

	void Start () 
	{

	}

	void Update () 
	{
		//pointsText.text 	= Blackboard.points.ToString();
	}

	public void ClickMuletilla()
	{
		if(sliderMuletilla.IsCorrect())
		{
			Blackboard.points++;
		}
		else
		{
			Blackboard.points--;	
		}
	}
}
