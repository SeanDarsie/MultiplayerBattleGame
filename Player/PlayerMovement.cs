using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour, IStunnable {
	NavMeshAgent agent;
	Animator anim;
	PlayerController playerController;

	// BOOLS //
	[HideInInspector] public bool stunned = false; // if stunned is true
	
	// BOOLS //

	[SerializeField] float speed = 10f;
	[SerializeField] float acceleration = 100f;

	void Start () {
		agent 				= GetComponent<NavMeshAgent>();
		anim 				= GetComponent<Animator>();
		playerController	= GetComponent<PlayerController>();
		agent.speed 		= speed;
		agent.acceleration 	= acceleration;
		// Audiosource? nah
	}
	
	// Update is called once per frame
	void Update () {
		// Detect mouse click/hold or screen press/hold for movement. We won't care about attack move for now. 
		// We should not move if the player is trying to fire a skillshot or ability of any kind. 
		if (Input.GetMouseButton(1) 		&& 
		!stunned 							&& 
		!playerController.skillShotReady	&&
		!playerController.specialReady		&&
		!playerController.ultimateReady)	
		{
			// Raycast mouse screen to point thingy and see if it is a valid position on the navmesh. 
			RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) 
			{
				if (hit.collider.tag != "Enemy")
				{
					agent.SetDestination(hit.point);
					playerController.target = null;
					agent.stoppingDistance = 1.0f;
				}
				else
				{
					if (Vector3.Distance(transform.position, hit.collider.transform.position) > playerController.autoAttackRange)
						agent.SetDestination(hit.point);	
					playerController.target = hit.collider.gameObject;
					agent.stoppingDistance = playerController.autoAttackRange - 1f;
				}
				// NavMeshHit navMeshHit;
				// if (!NavMesh.Raycast(transform.position, hit.point, out navMeshHit, NavMesh.AllAreas))
				// {
				// 	// agent.destination = hit.point;
				// 	agent.SetDestination(hit.point);
				// }
			}
		}
	}

	public void GetStunned(float seconds) // IStunnable function
	{
		Debug.Log("GetStunned()");
		// TODO: assign a stunned animation to be used but the Animator anim.
		agent.isStopped = true;
		stunned = true;
		agent.isStopped = true;
		playerController.DeactivateSkillShotIndicator();
		playerController.DeactivateSpecialIndicator();
		playerController.DeactivateUltimateIndicator();
	}
	public void GetCleansed() // IStunnable function
	{
		Debug.Log("GetCleansed()");
		// TODO: setbool stunned in anim to false
		agent.isStopped = false;
		stunned = false;
	}
}
