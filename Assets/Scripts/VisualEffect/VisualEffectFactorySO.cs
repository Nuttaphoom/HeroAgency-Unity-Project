using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectFactorySO : MonoBehaviour 
{
	public GameObject CreateEffect(VisualEffectSO so, Entity I , Entity Target ) 
	{
		GameObject ve = Instantiate(so.animationObject) ;
		ve.GetComponent<VisualEffect>().Init(so, I, Target) ; 
		return ve; 
	}
}
