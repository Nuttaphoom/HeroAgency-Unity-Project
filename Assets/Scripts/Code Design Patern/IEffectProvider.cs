using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectProvider 
{
	void TakeCombatEffect(GameObject effectTile);
	void TakeAreaEffect(GameObject effectTile);
	void TakeEffect_BeforeSpawn(GameObject effectTile);
	void TakeEffect_OnHeadQuarterEnter(GameObject effectTile);
	void RemoveEffect(GameObject effectTile);
 }

 
