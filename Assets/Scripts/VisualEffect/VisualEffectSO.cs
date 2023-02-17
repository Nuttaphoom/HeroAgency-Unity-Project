using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/VisualEffects/VisualEffectSO")]
public class VisualEffectSO : ScriptableObject
{
	public enum VisualEffectType
	{
		NONE, 
		Projectile,
		InstantEffect 
	}

	public VisualEffectType visualEffectType;

	public float howLongInSec = 0 ;

	public GameObject animationObject ;

	public float projectileSpeed = 0; 


}
