using System;
using UnityEngine;

public class baseTest : MonoBehaviour
{
	public NavMeshAgent man;
	public Transform target;

	void Start()
	{
		man = gameObject.GetComponent<NavMeshAgent> ();
		if (null == man) 
		{
			man = gameObject.AddComponent<NavMeshAgent> ();
		}
	}

	void Update()
	{
		man.SetDestination (target.position);
	}

}