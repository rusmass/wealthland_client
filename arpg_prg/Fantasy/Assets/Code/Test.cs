using UnityEngine;
using System.Collections;
using Net;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
	NetSession session;
	Button button;
	// Use this for initialization
	void Start ()
	{
		Console.Init ();
		Console.WriteLine ("");
		button = transform.GetComponentInChildren<Button> ();
		button.onClick.AddListener (DDD);
		session = new NetSession ();
		session.Connect ();

		new EnterRoomAction ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Console.Tick ();
		Core.Loom.Tick();
	}

	public void DDD()
	{
		session.DoAction<EnterRoomAction>();
	}

	void OnDisable()
	{
		ActionFactory.Instance.Clear ();
		session.DisConnect ();
		session = null;
	}
}

