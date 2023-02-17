using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquido : ProHero
{
	public override void Attack(Entity target)
	{
		if (state == StateMachine.Attacking)
		{ //In delay time 
			delayBetweenAttackCounter += 1 * Time.deltaTime;
			if (delayBetweenAttackCounter >= delayBetweenAttack)
			{
				ChangeState(StateMachine.Idle);
				delayBetweenAttackCounter = 0;
			}
		}
		else if (state != StateMachine.Die)
		{
			attackDelay +=   SPEED * Time.deltaTime;
			attackDelaySlider.value = attackDelay;
			if (attackDelay >= 0.6)
			{
				int target1 = Random.Range(0, FindObjectOfType<CombatManager>().enemyEntities.Count);

				if (FindObjectOfType<CombatManager>().enemyEntities.Count > 1)
				{
					int target2 = Random.Range(0, FindObjectOfType<CombatManager>().enemyEntities.Count);
					do
					{
						target2 = Random.Range(0, FindObjectOfType<CombatManager>().enemyEntities.Count);
					} while (target2 == target1);
					DoDamage(FindObjectOfType<CombatManager>().enemyEntities[target2]);

				}
				DoDamage(FindObjectOfType<CombatManager>().enemyEntities[target1]);

				attackDelay = 0;
			}
		}
		


	}
}
