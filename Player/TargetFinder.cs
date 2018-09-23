using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour {

	Transform target;
	[SerializeField] GameObject[] players;
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		players = GameObject.FindGameObjectsWithTag("Enemy");
	}
	// Update is called once per frame
	void Update () {
		
	}
	public GameObject[] GetEnemiesList() { return players; }
}
