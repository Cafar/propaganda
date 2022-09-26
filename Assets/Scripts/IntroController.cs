using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;


public class IntroController : MonoBehaviour {

	private int num;
	public Text text, text2;
	public Image anim;

	void Start()
	{
		num = 0;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(num == 0)
			{
				text.DOFade(0,1).OnComplete(()=>{
					text2.DOFade(1,1);
					}
					);
			}
			if(num ==1)
			{
				StartCoroutine(Blackboard.Instance.ChangeLevel("Main"));
			}
			num++;
		}
	}

	void Change()
	{
		StartCoroutine(Blackboard.Instance.ChangeLevel("Main"));
	}
}
