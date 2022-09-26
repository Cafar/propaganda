using UnityEngine;
using System.Collections;
using DG.Tweening;


public class MenuController : MonoBehaviour {

	public SpriteRenderer credits,title;
	void Start () 
	{
		if(credits != null && title != null)
		{
			credits.DOFade(1,2).OnComplete(()=>{
				credits.DOFade(0,2).OnComplete(()=>{
					title.DOFade(1,2);
				}
				);
			}
			);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
		    StartCoroutine(Blackboard.Instance.ChangeLevel("Intro"));
		}
	}
}
