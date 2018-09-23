using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
	
	public bool followPlayer = true;
	[SerializeField] float cameraSmoothness;
	float slowestSpeed;
	Quaternion originalRotation;
	[SerializeField] Transform playerPosition;
	[SerializeField] Vector3 offsetVector;
	Vector3 targetPosition;
	Vector3 adjust;
	// Use this for initialization
	void Start () {
		originalRotation = transform.rotation;
		adjust = offsetVector;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			offsetVector += adjust;
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			offsetVector -= adjust;
		}
		if (followPlayer == true)
		{
			targetPosition = playerPosition.position + offsetVector;
			transform.Translate((targetPosition - transform.position) * Time.deltaTime * cameraSmoothness, Space.World);
		}
		// transform.rotation = originalRotation;
		// follow the player!?!?!??!?! but at a distance. I can set a position that is in the correct position and then have the.
		// let's figure out not stuck to the player first actually. This means we need a minimap first??? crap.
	}
}
