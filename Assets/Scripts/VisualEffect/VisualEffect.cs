using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffect : MonoBehaviour
{
	private Entity launcher;
	private Entity target;
    private Vector3 targetPos;

	VisualEffectSO visualSO;

	private float lifeTime = 0;

    private float speed = 0 ; 

	public void Init(VisualEffectSO so , Entity launcher , Entity target)
	{
		this.launcher = launcher; this.target = target; this.targetPos = target.combatObj.transform.position;
		visualSO = so ;

        if (visualSO.visualEffectType == VisualEffectSO.VisualEffectType.InstantEffect)
        {
            transform.SetParent(target.combatObj.transform);
            transform.position = target.combatObj.transform.position;
        }
        else
        {
            transform.position = launcher.combatObj.transform.position;
            Vector3 s =  (target.combatObj.transform.position - transform.position   );
            if (s.x < 0)
                transform.localScale = new Vector3(-1, -1, 1);
            speed =   (Mathf.Abs( s.x) + Mathf.Abs(s.y)) / so.projectileSpeed    ;
         }
    }

	private void Update()
	{ 		
		if (visualSO != null)
		{
            if (visualSO.visualEffectType == VisualEffectSO.VisualEffectType.InstantEffect)
            {
                lifeTime += 1 * Time.deltaTime;

                if (lifeTime >= visualSO.howLongInSec)
                {
                    Destroy(gameObject);
                }
            }
            else if (visualSO.visualEffectType == VisualEffectSO.VisualEffectType.Projectile)
            {
                Vector3 v3 = this.targetPos - transform.position;
                v3.Normalize();
                transform.Translate(v3 * Time.deltaTime * speed);


                     
                if (Mathf.Abs(targetPos.x - transform.position.x) <= 0.02f  )
                {
                    lifeTime += 1 * Time.deltaTime;
                    if (lifeTime >= visualSO.howLongInSec)
                        Destroy(gameObject);
                }
            }



		}
	}
}


