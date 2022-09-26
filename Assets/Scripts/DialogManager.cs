using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class Combination
{
	public KeyCode firstKey;
	public KeyCode secondKey;
	public Color32	colorToChange;
	public Combination(KeyCode a, KeyCode b, Color32 color)
	{
		firstKey =a;
		secondKey = b;
		colorToChange = color;
	}
}

public class DialogManager : MonoBehaviour {

	public delegate void HandleKeyDown(List<KeyCode> _keyStep);
	public event HandleKeyDown OnKeyDown;
	public Dictionary<KeyCode, KeyInputState> keyStates;
	public List<Combination> selectedCombinations;
	public List<KeyCode> keysSelected;
	public List<Combination> currentCompas, auxCompas;
	public AudioSource music;
	public AudioSource fx;

	public bool lose;

	public AudioClip goodPeople, regularPeople, badPeople;

	public int currentIndexCompas;
	#region FSM
	
	public FSM	Fsm{get;set;}
	
	public Step1 			step1;
	public PlayStep 		playStep;
	public Step2 			step2;
	public Step3 			step3;
	public Step4 			step4;
	public Step5 			step5;
	public Step6			step6;
//	public Step3 		step3;
//	public Step4 		step4;
//	public Step5 		step5;
//	public Step6 		step6;
//	public Step7 		step7;
//	public Step8 		step8;
	#endregion

	public Button 		btnOption1, btnOption2, btnOption3;
	public Text			textPapel;
	[HideInInspector]
	public Text			mainText, backText;

	public Google2u.Datos data;

	public MuletillaController muletilla;

	public float letterPause;

	public int currentStep;

	public Transform paper;

	[HideInInspector]
	public List<KeyCode> keyA,keyB,keyC;

	public void Awake()
	{
		InitializeKeyStates();

		mainText = muletilla.mainText;
		backText = muletilla.backText;
		currentStep = 0;
		mainText.enabled = false;
		backText.enabled = false;
		lose = false;

		Fsm = FSM.CreateFSM(this);
		Fsm.AddFSMState (step1, step2, step3, step4, step5, step6, playStep);

		Fsm.ChangeState(step1);
		selectedCombinations = new List<Combination>();
		currentCompas = new List<Combination>();
		auxCompas = new List<Combination>();
	}

	void InitializeKeyStates()
	{
		keyStates =  new Dictionary<KeyCode, KeyInputState>();
		keyStates.Add(KeyCode.Q,new KeyInputState(KeyCode.Q));
		keyStates.Add(KeyCode.W,new KeyInputState(KeyCode.W));
		keyStates.Add(KeyCode.E,new KeyInputState(KeyCode.E));
		keyStates.Add(KeyCode.R,new KeyInputState(KeyCode.R));
		keyStates.Add(KeyCode.T,new KeyInputState(KeyCode.T));
		keyStates.Add(KeyCode.Y,new KeyInputState(KeyCode.Y));
		keyStates.Add(KeyCode.U,new KeyInputState(KeyCode.U));
		keyStates.Add(KeyCode.I,new KeyInputState(KeyCode.I));
		keyStates.Add(KeyCode.O,new KeyInputState(KeyCode.O));
		keyStates.Add(KeyCode.P,new KeyInputState(KeyCode.P));
		keyStates.Add(KeyCode.A,new KeyInputState(KeyCode.A));
		keyStates.Add(KeyCode.S,new KeyInputState(KeyCode.S));
		keyStates.Add(KeyCode.D,new KeyInputState(KeyCode.D));
		keyStates.Add(KeyCode.F,new KeyInputState(KeyCode.F));
		keyStates.Add(KeyCode.G,new KeyInputState(KeyCode.G));
		keyStates.Add(KeyCode.H,new KeyInputState(KeyCode.H));
		keyStates.Add(KeyCode.J,new KeyInputState(KeyCode.J));
		keyStates.Add(KeyCode.K,new KeyInputState(KeyCode.K));
		keyStates.Add(KeyCode.L,new KeyInputState(KeyCode.L));
		keyStates.Add(KeyCode.Z,new KeyInputState(KeyCode.Z));
		keyStates.Add(KeyCode.X,new KeyInputState(KeyCode.X));
		keyStates.Add(KeyCode.C,new KeyInputState(KeyCode.C));
		keyStates.Add(KeyCode.V,new KeyInputState(KeyCode.V));
		keyStates.Add(KeyCode.B,new KeyInputState(KeyCode.B));
		keyStates.Add(KeyCode.N,new KeyInputState(KeyCode.N));
		keyStates.Add(KeyCode.M,new KeyInputState(KeyCode.M));
	}

	void InitializeKeyButtons()
	{
		keyA = new List<KeyCode>();
		keyB = new List<KeyCode>();
		keyC = new List<KeyCode>();
		currentIndexCompas = 0;
	}

	void CheckAllKeyStates()
	{
		foreach(KeyCode k in keyStates.Keys)
		{
			keyStates[k].CheckState();
		}
	}

	public Combination GetRandomComb()
	{
		return selectedCombinations[Random.Range(0,selectedCombinations.Count)];
	}

	void GetNextCompas()
	{
		currentCompas = new List<Combination>();
		currentCompas.Add(GetRandomComb());
		currentCompas.Add(GetRandomComb());
		currentCompas.Add(GetRandomComb());
		currentCompas.Add(GetRandomComb());
		//Borro 
		auxCompas = new List<Combination>();
		//añado
		foreach(Combination co in currentCompas)
		{
			auxCompas.Add(co);
		}
	}

	bool IsKeyValid(KeyCode k)
	{
		return keyStates[k].isActive;
	}

	bool IsCombinationValid(KeyCode a, KeyCode b)
	{
		return IsKeyValid(a) && IsKeyValid(b);
	}

	void CheckIsCorrect ()
	{
		if(muletilla.IsCorrect())
		{
			currentCompas.Remove(currentCompas[0]);
			//Si te pasas la fase
			if(currentCompas.Count==0)
			{
				fx.Play();
				StartCoroutine("MyChangeState",1);
			}
		}
		else
		{
			//Borro 
			currentCompas = new List<Combination>();
			//añado
			foreach(Combination co in auxCompas)
			{
				currentCompas.Add(co);
			}
			Debug.Log("ERROR: "+currentCompas.Count);
		}
	}
	IEnumerator MyChangeState(float _time)
	{
		fx.Play();
		mainText.enabled = false;
		backText.enabled = false;
		yield return new WaitForSeconds(_time);
		if(!lose)
		{
			currentStep++;
			if(currentStep == 5)
			{
				StartCoroutine(Blackboard.Instance.ChangeLevel("Final"));
			}
			Fsm.ChangeState(Fsm.GetStateFromIndex( currentStep));
		}
		else
		{
			Invoke("ChangeScene",2);
		    
		}
	}

	void ChangeScene()
	{
		StartCoroutine(Blackboard.Instance.ChangeLevel("Intro"));
	}

	public void Update()
	{
		CheckAllKeyStates();

		if(muletilla.currentTime == 0)
		{
			//Borro 
			currentCompas = new List<Combination>();
			//añado
			foreach(Combination co in auxCompas)
			{
				currentCompas.Add(co);
			}
		}
	}

	#region states

	[System.Serializable]
	public class Step1 : FSM.FSMState
	{
		private DialogManager myOwner;

		private bool activate;

		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as DialogManager;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.InitializeKeyButtons();

			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.A_1)._Key[0]));
			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.A_1)._Key[1]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.A_2)._Key[0]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.A_2)._Key[1]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.A_3)._Key[0]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.A_3)._Key[1]));

			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).text = myOwner.keyA[0].ToString() + "+" + myOwner.keyA[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).text = myOwner.keyB[0].ToString() + "+" + myOwner.keyB[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).text = myOwner.keyC[0].ToString() + "+" + myOwner.keyC[1].ToString();

			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.A_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.A_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.A_1)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.A_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.A_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.A_2)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.A_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.A_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.A_3)._Color.b, 255);

			myOwner.textPapel.text = "Economy";

			myOwner.paper.DOLocalMoveY(-5,1f).OnComplete(()=>{
				activate = true;
			}
			);
		}
		
		public override void Update ()
		{
			base.Update ();
			if(activate)
			{
				if(myOwner.IsCombinationValid(myOwner.keyA[0], myOwner.keyA[1]))
				{
					myOwner.keyStates[myOwner.keyA[0]].Reset();
					myOwner.keyStates[myOwner.keyA[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.A_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.A_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.A_1)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.A_1)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyA[0],myOwner.keyA[1],myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.badPeople;
					myOwner.lose = true;
					ChangeState(myOwner.playStep);
				}

				if(myOwner.IsCombinationValid(myOwner.keyB[0], myOwner.keyB[1]))
				{
					myOwner.keyStates[myOwner.keyB[0]].Reset();
					myOwner.keyStates[myOwner.keyB[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.A_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.A_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.A_2)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.A_2)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyB[0],myOwner.keyB[1],myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.regularPeople;
					ChangeState(myOwner.playStep);
				}

				if(myOwner.IsCombinationValid(myOwner.keyC[0], myOwner.keyC[1]))
				{
					myOwner.keyStates[myOwner.keyC[0]].Reset();
					myOwner.keyStates[myOwner.keyC[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.A_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.A_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.A_3)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.A_3)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyC[0],myOwner.keyC[1],myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.goodPeople;
					ChangeState(myOwner.playStep);
				}
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			myOwner.paper.DOLocalMoveY(-155,1f).OnComplete(()=>{
				myOwner.mainText.enabled = true;
				myOwner.backText.enabled = true;
				myOwner.music.Play();
			}
			);

		}
		
	}

	[System.Serializable]
	public class PlayStep : FSM.FSMState
	{
		private DialogManager myOwner;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as DialogManager;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			myOwner.GetNextCompas();

		}
		
		public override void Update ()
		{
			base.Update ();



			if(myOwner.currentCompas.Count > 0)
			{
				myOwner.muletilla.colorToChange = myOwner.currentCompas[0].colorToChange; 
				if(myOwner.IsCombinationValid(myOwner.currentCompas[0].firstKey,myOwner.currentCompas[0].secondKey))
				{
					myOwner.keyStates[myOwner.currentCompas[0].firstKey].Reset();
					myOwner.keyStates[myOwner.currentCompas[0].secondKey].Reset();
					myOwner.CheckIsCorrect();
				}
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
		}
		
	}
	[System.Serializable]
	public class Step2 : FSM.FSMState
	{
		private DialogManager myOwner;
		
		private bool activate;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as DialogManager;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);

			myOwner.InitializeKeyButtons();

			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.B_1)._Key[0]));
			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.B_1)._Key[1]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.B_2)._Key[0]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.B_2)._Key[1]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.B_3)._Key[0]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.B_3)._Key[1]));
			
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).text = myOwner.keyA[0].ToString() + "+" + myOwner.keyA[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).text = myOwner.keyB[0].ToString() + "+" + myOwner.keyB[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).text = myOwner.keyC[0].ToString() + "+" + myOwner.keyC[1].ToString();
			
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.B_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.B_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.B_1)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.B_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.B_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.B_2)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.B_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.B_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.B_3)._Color.b, 255);

			myOwner.textPapel.text = "Healthcare";

			myOwner.mainText.enabled = false;
			myOwner.backText.enabled = false;
			
			myOwner.paper.DOLocalMoveY(-5f,1f).OnComplete(()=>{
				activate = true;
			}
			);
		}
		
		public override void Update ()
		{
			base.Update ();
			if(activate)
			{
				if(myOwner.IsCombinationValid(myOwner.keyA[0], myOwner.keyA[1]))
				{
					myOwner.keyStates[myOwner.keyA[0]].Reset();
					myOwner.keyStates[myOwner.keyA[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.B_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.B_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.B_1)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.B_1)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyA[0],myOwner.keyA[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.regularPeople;
					ChangeState(myOwner.playStep);
				}
				
				if(myOwner.IsCombinationValid(myOwner.keyB[0], myOwner.keyB[1]))
				{
					myOwner.keyStates[myOwner.keyB[0]].Reset();
					myOwner.keyStates[myOwner.keyB[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.B_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.B_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.B_2)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.B_2)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyB[0],myOwner.keyB[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.goodPeople;
					ChangeState(myOwner.playStep);
				}
				
				if(myOwner.IsCombinationValid(myOwner.keyC[0], myOwner.keyC[1]))
				{
					myOwner.keyStates[myOwner.keyC[0]].Reset();
					myOwner.keyStates[myOwner.keyC[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.B_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.B_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.B_3)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.B_3)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyC[0],myOwner.keyC[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.badPeople;
					myOwner.lose = true;
					ChangeState(myOwner.playStep);
				}
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			myOwner.paper.DOLocalMoveY(-155,1f).OnComplete(()=>{
				myOwner.mainText.enabled = true;
				myOwner.backText.enabled = true;

			}
			);
		}
		
	}

	[System.Serializable]
	public class Step3 : FSM.FSMState
	{
		private DialogManager myOwner;
		
		private bool activate;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as DialogManager;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.InitializeKeyButtons();
			
			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.C_1)._Key[0]));
			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.C_1)._Key[1]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.C_2)._Key[0]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.C_2)._Key[1]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.C_3)._Key[0]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.C_3)._Key[1]));
			
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).text = myOwner.keyA[0].ToString() + "+" + myOwner.keyA[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).text = myOwner.keyB[0].ToString() + "+" + myOwner.keyB[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).text = myOwner.keyC[0].ToString() + "+" + myOwner.keyC[1].ToString();
			
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.C_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.C_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.C_1)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.C_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.C_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.C_2)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.C_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.C_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.C_3)._Color.b, 255);

			myOwner.textPapel.text = "Education";

			myOwner.mainText.enabled = false;
			myOwner.backText.enabled = false;
			
			myOwner.paper.DOLocalMoveY(-5,1f).OnComplete(()=>{
				activate = true;
			}
			);
		}
		
		public override void Update ()
		{
			base.Update ();
			if(activate)
			{
				if(myOwner.IsCombinationValid(myOwner.keyA[0], myOwner.keyA[1]))
				{
					myOwner.keyStates[myOwner.keyA[0]].Reset();
					myOwner.keyStates[myOwner.keyA[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.C_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.C_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.C_1)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.C_1)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyA[0],myOwner.keyA[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.goodPeople;
					ChangeState(myOwner.playStep);
				}
				
				if(myOwner.IsCombinationValid(myOwner.keyB[0], myOwner.keyB[1]))
				{
					myOwner.keyStates[myOwner.keyB[0]].Reset();
					myOwner.keyStates[myOwner.keyB[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.C_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.C_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.C_2)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.C_2)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyB[0],myOwner.keyB[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.regularPeople;
					ChangeState(myOwner.playStep);
				}
				
				if(myOwner.IsCombinationValid(myOwner.keyC[0], myOwner.keyC[1]))
				{
					myOwner.keyStates[myOwner.keyC[0]].Reset();
					myOwner.keyStates[myOwner.keyC[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.C_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.C_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.C_3)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.C_3)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyC[0],myOwner.keyC[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.badPeople;
					myOwner.lose = true;
					ChangeState(myOwner.playStep);
				}
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			myOwner.paper.DOLocalMoveY(-155,1f).OnComplete(()=>{
				myOwner.mainText.enabled = true;
				myOwner.backText.enabled = true;

			}
			);
		}
		
	}

	[System.Serializable]
	public class Step4 : FSM.FSMState
	{
		private DialogManager myOwner;
		
		private bool activate;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as DialogManager;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.InitializeKeyButtons();
			
			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.D_1)._Key[0]));
			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.D_1)._Key[1]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.D_2)._Key[0]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.D_2)._Key[1]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.D_3)._Key[0]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.D_3)._Key[1]));
			
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).text = myOwner.keyA[0].ToString() + "+" + myOwner.keyA[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).text = myOwner.keyB[0].ToString() + "+" + myOwner.keyB[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).text = myOwner.keyC[0].ToString() + "+" + myOwner.keyC[1].ToString();

			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.D_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.D_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.D_1)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.D_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.D_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.D_2)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.D_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.D_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.D_3)._Color.b, 255);

			myOwner.textPapel.text = "National Security";

			myOwner.mainText.enabled = false;
			myOwner.backText.enabled = false;
			
			myOwner.paper.DOLocalMoveY(-5,1f).OnComplete(()=>{
				activate = true;
			}
			);
		}
		
		public override void Update ()
		{
			base.Update ();
			if(activate)
			{
				if(myOwner.IsCombinationValid(myOwner.keyA[0], myOwner.keyA[1]))
				{
					myOwner.keyStates[myOwner.keyA[0]].Reset();
					myOwner.keyStates[myOwner.keyA[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.D_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.D_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.D_1)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.D_1)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyA[0],myOwner.keyA[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.goodPeople;
					ChangeState(myOwner.playStep);
				}
				
				if(myOwner.IsCombinationValid(myOwner.keyB[0], myOwner.keyB[1]))
				{
					myOwner.keyStates[myOwner.keyB[0]].Reset();
					myOwner.keyStates[myOwner.keyB[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.D_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.D_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.D_2)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.D_2)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyB[0],myOwner.keyB[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.badPeople;
					myOwner.lose = true;
					ChangeState(myOwner.playStep);
				}
				
				if(myOwner.IsCombinationValid(myOwner.keyC[0], myOwner.keyC[1]))
				{
					myOwner.keyStates[myOwner.keyC[0]].Reset();
					myOwner.keyStates[myOwner.keyC[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.D_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.D_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.D_3)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.D_3)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyC[0],myOwner.keyC[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.regularPeople;
					ChangeState(myOwner.playStep);
				}
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			myOwner.paper.DOLocalMoveY(-155,1f).OnComplete(()=>{
				myOwner.mainText.enabled = true;
				myOwner.backText.enabled = true;

			}
			);
		}
		
	}

	[System.Serializable]
	public class Step5 : FSM.FSMState
	{
		private DialogManager myOwner;
		
		private bool activate;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as DialogManager;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.InitializeKeyButtons();
			
			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.E_1)._Key[0]));
			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.E_1)._Key[1]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.E_2)._Key[0]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.E_2)._Key[1]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.E_3)._Key[0]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.E_3)._Key[1]));
			
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).text = myOwner.keyA[0].ToString() + "+" + myOwner.keyA[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).text = myOwner.keyB[0].ToString() + "+" + myOwner.keyB[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).text = myOwner.keyC[0].ToString() + "+" + myOwner.keyC[1].ToString();
			
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.E_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.E_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.E_1)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.E_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.E_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.E_2)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.E_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.E_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.E_3)._Color.b, 255);

			myOwner.textPapel.text = "Religion";

			myOwner.mainText.enabled = false;
			myOwner.backText.enabled = false;
			
			myOwner.paper.DOLocalMoveY(-5,1f).OnComplete(()=>{
				activate = true;
			}
			);
		}
		
		public override void Update ()
		{
			base.Update ();
			if(activate)
			{
				if(myOwner.IsCombinationValid(myOwner.keyA[0], myOwner.keyA[1]))
				{
					myOwner.keyStates[myOwner.keyA[0]].Reset();
					myOwner.keyStates[myOwner.keyA[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.E_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.E_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.E_1)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.E_1)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyA[0],myOwner.keyA[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.goodPeople;
					ChangeState(myOwner.playStep);
				}
				
				if(myOwner.IsCombinationValid(myOwner.keyB[0], myOwner.keyB[1]))
				{
					myOwner.keyStates[myOwner.keyB[0]].Reset();
					myOwner.keyStates[myOwner.keyB[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.E_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.E_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.E_2)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.E_2)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyB[0],myOwner.keyB[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.badPeople;
					myOwner.lose = true;
					ChangeState(myOwner.playStep);
				}
				
				if(myOwner.IsCombinationValid(myOwner.keyC[0], myOwner.keyC[1]))
				{
					myOwner.keyStates[myOwner.keyC[0]].Reset();
					myOwner.keyStates[myOwner.keyC[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.E_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.E_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.E_3)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.E_3)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyC[0],myOwner.keyC[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.regularPeople;
					ChangeState(myOwner.playStep);
				}
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			myOwner.paper.DOLocalMoveY(-155,1f).OnComplete(()=>{
				myOwner.mainText.enabled = true;
				myOwner.backText.enabled = true;

			}
			);
		}
		
	}

	[System.Serializable]
	public class Step6 : FSM.FSMState
	{
		private DialogManager myOwner;
		
		private bool activate;
		
		public override void Init ()
		{
			base.Init ();
			
			myOwner = owner as DialogManager;
			
		}
		
		public override void Enter (Hashtable _parameters)
		{
			base.Enter (_parameters);
			
			myOwner.InitializeKeyButtons();
			
			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.F_1)._Key[0]));
			myOwner.keyA.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.F_1)._Key[1]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.F_2)._Key[0]));
			myOwner.keyB.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.F_2)._Key[1]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.F_3)._Key[0]));
			myOwner.keyC.Add((KeyCode) System.Enum.Parse(typeof(KeyCode), myOwner.data.GetRow(Google2u.Datos.rowIds.F_3)._Key[1]));
			
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).text = myOwner.keyA[0].ToString() + "+" + myOwner.keyA[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).text = myOwner.keyB[0].ToString() + "+" + myOwner.keyB[1].ToString();
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).text = myOwner.keyC[0].ToString() + "+" + myOwner.keyC[1].ToString();
			
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption1).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.F_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.F_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.F_1)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption2).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.F_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.F_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.F_2)._Color.b, 255);
			Blackboard.Instance.GetTextFromButton(myOwner.btnOption3).color = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.F_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.F_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.F_3)._Color.b, 255);

			myOwner.textPapel.text = "Domestic Policy";

			myOwner.mainText.enabled = false;
			myOwner.backText.enabled = false;
			
			myOwner.paper.DOLocalMoveY(-5,1f).OnComplete(()=>{
				activate = true;
			}
			);
		}
		
		public override void Update ()
		{
			base.Update ();
			if(activate)
			{
				if(myOwner.IsCombinationValid(myOwner.keyA[0], myOwner.keyA[1]))
				{
					myOwner.keyStates[myOwner.keyA[0]].Reset();
					myOwner.keyStates[myOwner.keyA[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.F_1)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.F_1)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.F_1)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.F_1)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyA[0],myOwner.keyA[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.goodPeople;
					ChangeState(myOwner.playStep);
				}
				
				if(myOwner.IsCombinationValid(myOwner.keyB[0], myOwner.keyB[1]))
				{
					myOwner.keyStates[myOwner.keyB[0]].Reset();
					myOwner.keyStates[myOwner.keyB[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.F_2)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.F_2)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.F_2)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.F_2)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyB[0],myOwner.keyB[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.badPeople;
					myOwner.lose = true;
					ChangeState(myOwner.playStep);
				}
				
				if(myOwner.IsCombinationValid(myOwner.keyC[0], myOwner.keyC[1]))
				{
					myOwner.keyStates[myOwner.keyC[0]].Reset();
					myOwner.keyStates[myOwner.keyC[1]].Reset();
					myOwner.muletilla.colorToChange = new Color32(myOwner.data.GetRow(Google2u.Datos.rowIds.F_3)._Color.r, myOwner.data.GetRow(Google2u.Datos.rowIds.F_3)._Color.g, myOwner.data.GetRow(Google2u.Datos.rowIds.F_3)._Color.b, 255);
					myOwner.mainText.text = myOwner.data.GetRow(Google2u.Datos.rowIds.F_3)._Textos;
					myOwner.selectedCombinations.Add(new Combination(myOwner.keyC[0],myOwner.keyC[1], myOwner.muletilla.colorToChange));
					myOwner.fx.clip = myOwner.regularPeople;
					ChangeState(myOwner.playStep);
				}
			}
		}
		
		public override void Exit (FSM.FSMState _nextState)
		{
			myOwner.paper.DOLocalMoveY(-155,1f).OnComplete(()=>{
				myOwner.mainText.enabled = true;
				myOwner.backText.enabled = true;

			}
			);
		}
	}
	#endregion
}
