﻿using UnityEngine;
using System.Collections;

public class Cyclops_Follow : MonoBehaviour {

	bool attackSleep = true;
	bool hitting = true;

	// Function to chase player if in range 
	public void FollowThem (Animator animate, NavMeshAgent agent, Vector3 playerLocation, Cylops_AniControl cyclopsControl, Cyclops_Combat combatScript,Player playerScript) {
		Vector3 direction = playerLocation - this.transform.position;
		bool isCurrentAttack = animate.GetCurrentAnimatorStateInfo (0).IsName ("Attack");
		if (Vector3.Distance (playerLocation, this.transform.position) < 300){
			direction.y = 0;
			if (!isCurrentAttack) { 
				this.transform.rotation = (Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.1f));
			}

			if (direction.magnitude > 5) {
				if (!isCurrentAttack) { 
					agent.Resume ();
					agent.SetDestination (playerLocation);
					agent.speed = 7;
					agent.acceleration = 8;
				}
				cyclopsControl.setRun (animate);	// outside of loop to tell attack to stop
			} 

			else{
				cyclopsControl.setAttack (animate);
				if (hitting) {
					hitting = false;
					StartCoroutine (SleepForAttack());
					combatScript.Hit (playerLocation,playerScript);
				}
				agent.SetDestination (this.transform.position);
				agent.velocity = Vector3.zero;
				agent.Stop ();

				// then make it so he can turn at the end
				if (attackSleep){
					attackSleep = false;
					this.transform.rotation = (Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.1f));
					attackSleep = true;
				}
			}
		} 
	}

	IEnumerator SleepForAttack(){
		yield return new WaitForSecondsRealtime(2.7f);
		hitting = true;
	}
}
