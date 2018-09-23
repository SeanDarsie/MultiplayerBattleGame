using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This class will handle health and attack at least and maybe movement? 
// I think movement might be better handled elsewhere. 
// This class looks like it will end up being really big if i continue down the path that i am currently using. I think i will 
// start separating things now. 

public class PlayerController : MonoBehaviour, IHittable{


	NavMeshAgent agent; // not sure if i need this. 
	AudioSource audioSource;
	PlayerMovement playerMovement;
	Animator anim;

	// KDA //
	[HideInInspector] public int 	kills;
	[HideInInspector] public int 	deaths;
	[HideInInspector] public int 	assists;
	// KDA //


	// STATS //
	[HideInInspector] 	public int		health;
	[HideInInspector] 	public int		armor;
						public int		damage;
						public float	autoAttackRange;
	[HideInInspector] 	public int 		abilityDamage;
	[HideInInspector] 	public int 		deathTimer;
	// STATS //


	// BOOLEANS // 
	bool  							attackMove		= false;
	bool  							dead			= false;
	[HideInInspector] public bool 	skillShotReady	= false;
	[HideInInspector] public bool 	specialReady 	= false;
	[HideInInspector] public bool 	ultimateReady 	= false;
	// BOOLEANS //


	// GameObjects //
	[HideInInspector] public GameObject target; // the target of aa. I will have a separate script that will handle input for clicking enemies
	[SerializeField] GameObject skillShotIndicator;
	[SerializeField] GameObject specialAbilityIndicator;
	[SerializeField] GameObject ultimateAbilityIndicator;
	[SerializeField] GameObject projectile;
	[SerializeField] Transform lookAtMousePosition;
	[SerializeField] Transform firePos;
	// GameObjects //

	// ABILITY COOLDOWNS //
	[SerializeField] float autoAttackCooldown;
	float autoAttackCountdown;
	[SerializeField] float skillShotCooldown;
	float skillShotCountdown;
	[SerializeField] float specialCooldown;
	float specialCountdown;
	[SerializeField] float ultimateCooldown;
	float ultimateCountdown;
	
	// ABILITY COOLDOWNS // 
	void Start () {
		playerMovement 		= GetComponent<PlayerMovement>();
		audioSource 		= GetComponent<AudioSource>();
		agent 				= GetComponent<NavMeshAgent>();
		skillShotCountdown 	= Time.time + skillShotCooldown;
		specialCountdown 	= Time.time + specialCooldown;
		ultimateCountdown 	= Time.time + ultimateCooldown;
		// autoAttackRange = 10f;
		// damage = 1;
		// anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		// if an enemy is within attack range and the navmeshagent doesn't have a destination and attackMove is not true. 
		if (!playerMovement.stunned)
		{
			if (agent.isStopped == true || agent.velocity == Vector3.zero && target == null)
			{
				float distToEnemy = 100000f;
				foreach(GameObject x in FindObjectOfType<TargetFinder>().GetEnemiesList())
				{
					if (Vector3.Distance(transform.position, x.transform.position) < distToEnemy)
					{
						distToEnemy = Vector3.Distance(transform.position, x.transform.position);
						target = x;
					}
				}
			}
			if (Time.time > autoAttackCountdown) // attack targeted enemy. targeting will be it's own challenge. for now just attack the current target if there is one.
			{
				if (target != null && Vector3.Distance(transform.position, target.transform.position) <= autoAttackRange)
					AutoAttack(); 
			}
			if (skillShotReady == true)
			{
				if (Input.GetButtonUp("SkillShot")) // not going to allow any changing of this or now. 
					SkillShot();
			}
			if (specialReady == true)
			{
				if (Input.GetButtonUp("SpecialAbility"))
					SpecialAbility();	
			}
			if (ultimateReady == true)
			{
				if (Input.GetButtonUp("UltimateAbility"))
					UltimateAbility();
			}
		}
	}
	public void Die()
	{
		// start the death timer. and increment it. 
	}
	public void TakeDamage(int damage)
	{
		health -= damage;
	}
	void AutoAttack() // attack the current target. 
	{
		// TODO: This function must start an animation that fires an event that calls a function to fire some projectile. 
		agent.SetDestination(transform.position); // could also set the destination to current position as well. instead of turning it off. 
		GameObject bullet = Instantiate(projectile, firePos.position, firePos.rotation);
		AutoAttack autoAttack = bullet.GetComponent<AutoAttack>();
		autoAttack.damage = damage;
		autoAttack.target = target.transform;
		autoAttackCountdown = Time.time + autoAttackCooldown;
	}	
	public void SkillShot() // straight line skill shot on a low cooldown (7 - 13 sec)
	{
		// TODO: Play an animation that has an event which fires off a projectile. 
		skillShotCountdown = Time.time + skillShotCooldown;
		skillShotReady = false;
		DeactivateSkillShotIndicator();
	}
	public void SpecialAbility() // Maybe an aoe of some kind. Or something that provides zone control. CD ( 5 min or so )
	{
		specialCountdown = Time.time + specialCooldown;
		specialReady = false;
		DeactivateSpecialIndicator();
	}
	public void UltimateAbility() // Could be something that has to be picked up or earned in some way
	{
		ultimateCountdown = Time.time + ultimateCooldown;
		ultimateReady = false;
		DeactivateUltimateIndicator();
	}

	public void ActivateSkillShotIndicator()
	{
		// conditions are required to allow this
		if (Time.time < skillShotCountdown || playerMovement.stunned)
			return;
		specialAbilityIndicator.SetActive(false);
		ultimateAbilityIndicator.SetActive(false);
		skillShotIndicator.SetActive(true);
		skillShotReady = true;
	}
	public void ActivateSpecialIndicator()
	{
		if (Time.time < specialCountdown || playerMovement.stunned)
			return;
		skillShotIndicator.SetActive(false);
		ultimateAbilityIndicator.SetActive(false);
		specialAbilityIndicator.SetActive(true);
		specialReady = true;
	}
	public void ActivateUltimateIndicator()
	{
		if (Time.time < ultimateCountdown || playerMovement.stunned)
			return;
		skillShotIndicator.SetActive(false);
		specialAbilityIndicator.SetActive(false);
		ultimateAbilityIndicator.SetActive(true);
		ultimateReady = true;
	}
	public void DeactivateSkillShotIndicator()
	{
		skillShotIndicator.SetActive(false);
		skillShotReady = false;
	}
	public void DeactivateSpecialIndicator()
	{
		specialAbilityIndicator.SetActive(false);
		specialReady = false;
	}
	public void DeactivateUltimateIndicator()
	{
		ultimateAbilityIndicator.SetActive(false);
		ultimateReady = false;
	}
	
}
