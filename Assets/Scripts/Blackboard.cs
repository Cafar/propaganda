using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour {
	
	public static int points;

	private static Blackboard instance;
	public static Blackboard Instance 
	{
		get
		{
			if(instance == null)
			{
				string resourcesPrefabPath = "Blackboard";
				// Search in resources folder for this GameObject
				Blackboard managerPrefab = Resources.Load<Blackboard>(resourcesPrefabPath);
				
				if(managerPrefab == null)
				{
					Debug.LogError("[ERROR] Prefab "+resourcesPrefabPath+" not found in Resources directory");
					return null;
				}
				
				Instance = Instantiate(managerPrefab) as Blackboard;
			}
			
			return instance;
		}
		
		private set{
			instance = value;
		}
	}

	void Awake()
	{

	}

	public IEnumerator ChangeLevel(string _nameLevel)
	{
		float fadeTime = Camera.main.GetComponent<Fading>().BeginFade(1); 
		yield return new WaitForSeconds(.8f);
		Application.LoadLevel(_nameLevel);
	}

	/// <summary>
	/// Esto lo hago por que no funciona el WaitfOrseconds cuando hago un time.timescale a 0
	/// </summary>
	/// <returns>The for real seconds.</returns>
	/// <param name="delay">Delay.</param>
	public static IEnumerator WaitForRealSeconds( float delay )
	{
		float start = Time.realtimeSinceStartup;
		while( Time.realtimeSinceStartup < start + delay )
		{
			yield return null;
		}
	}

	public Text GetTextFromButton(Button _btn)
	{
		Text text = null;
		if(_btn.transform.FindChild("Text") != null)
		{
			text = _btn.transform.GetComponentInChildren<Text>();
		}
		else
		{
			Debug.LogError("El btn no tiene como hijo ninguno text");
		}
		return text;
	}
}
