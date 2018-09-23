using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour {
	public Transform target;
	public int damage;
	public float speed=50;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate((target.position - transform.position).normalized * speed * Time.deltaTime, Space.World);
		Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == target.gameObject)
			Destroy(gameObject);
	}
}
