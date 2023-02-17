using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fei : ProHero
{
	public override void PrepareForCombat()
	{
        base.PrepareForCombat();

        if (CombatManager.instance.CanAddMoreHero())
        {

            for (int i = 0; i < FindObjectOfType<HeroStock>().heroLists.Count; i++)
            {
                bool _containH = false;

                GameObject h = FindObjectOfType<HeroStock>().heroLists[i]; // ;
                if (FindObjectOfType<HeroStock>().heroLists[i].GetComponent<Fei>())
                {
                    for (int j = 0; j < FindObjectOfType<CombatManager>().heroEntities.Count; j++)
                    {
                        if (FindObjectOfType<CombatManager>().heroEntities.Contains(h))
                            _containH = true;
                    }
                }

                if (FindObjectOfType<HeroStock>().heroLists[i].GetComponent<Entity>().HP <= 0)
                    continue;

                if (!_containH)
                {
                    FindObjectOfType<CombatManager>().heroEntities.Add(h);
                    break;
                }
            }
        }
	}
}
