using UnityEngine;
using System.Collections;

[System.Serializable]
public class KeyInputState  
{
	public bool isActive = false;
	public float lastTimeStamp = 0f;
	public KeyCode key;
	float timeToDeactivate = 0.5f;

	public KeyInputState(KeyCode k)
	{
		key = k;
	}

	public void CheckState()
	{
		if(isActive)
		{
			if(Input.GetKeyUp(key))
			{
				isActive = false;
			}else if(Time.time > lastTimeStamp + timeToDeactivate)
			{
				isActive=false;
			}
		}
		else
		{
			if(Input.GetKeyDown(key))
			{
				lastTimeStamp = Time.time;
				isActive= true;
			}
		}

	}

	public void Reset()
	{
		isActive=false;
	}
}
