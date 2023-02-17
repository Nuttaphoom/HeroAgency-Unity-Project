using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDoneTextFactory : MonoBehaviour
{
    [SerializeField]
    private DamageDoneText _damageDoneObjTemplate;

    public DamageDoneText CreateDamageDoneText(Entity damagedEntity,float damageDoneAmount)
    {
        DamageDoneText dt =  Instantiate(_damageDoneObjTemplate,transform)  ;
        dt.transform.localScale =  _damageDoneObjTemplate.transform.localScale;
        dt.transform.position = damagedEntity.GetComponent<Entity>().combatObj.transform.position;
        dt.gameObject.SetActive(true);
        dt.Init(damagedEntity, (int) damageDoneAmount);
        return dt; 
    }
}
