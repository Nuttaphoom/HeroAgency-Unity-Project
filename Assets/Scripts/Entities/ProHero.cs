using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System ;


public abstract class ProHero : Hero
{
    public enum WeaponType
	{
		None,Electric, Fire , Water,Wind
	}

	public WeaponType weapon;

}
