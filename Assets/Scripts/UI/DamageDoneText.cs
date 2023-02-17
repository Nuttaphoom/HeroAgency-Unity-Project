using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class DamageDoneText : MonoBehaviour
{
    private List<Vector2> _directionVectorList = new List<Vector2>();
    private Vector2 _moveDirection ;

    [SerializeField]
    private float _speed ;

    public void Init(Entity entity ,float damageDone )
    {
        if (entity.GetComponent<Enemy>())
        {
            _directionVectorList.Add(new Vector2(1, 1));
            _directionVectorList.Add(new Vector2(1, 0));
            _directionVectorList.Add(new Vector2(1, -1));
        } else
        {
            _directionVectorList.Add(new Vector2(-1, 1));
            _directionVectorList.Add(new Vector2(-1, 0));
            _directionVectorList.Add(new Vector2(-1, -1));

        }
        _moveDirection = _directionVectorList[Random.Range(0,_directionVectorList.Count)] ;

        damageDone = (int) damageDone ; 
        
         string damageDoneString = damageDone == 0 ? "MISS"  : damageDone.ToString() ;
        damageDoneString = damageDone < 0 ? "Blocked" : damageDoneString ;

        GetComponent<TextMeshProUGUI>().text = damageDoneString  ;
    }

    private void Update()
    {
        transform.Translate(_moveDirection * _speed  * Time.deltaTime);
        _speed -= 6.0f * Time.deltaTime;
        if (_speed <= 0)
            Destroy(gameObject);
    }
}
