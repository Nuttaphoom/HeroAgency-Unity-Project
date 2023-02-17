using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class SpeedManager : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> _speedSprites;

    [SerializeField]
    private GameObject _speedImage;

    private int _speedMultipler = 1;


    private void Start()
    {
        EventManager.instance.OnStopPause += OnStopPause_ChangeSprite;
        EventManager.instance.OnPause += OnPause_ChangeSprite;
    }

    private void OnDestroy()
    {
        EventManager.instance.OnStopPause -= OnStopPause_ChangeSprite;
        EventManager.instance.OnPause -= OnPause_ChangeSprite;
    }

    private void OnStopPause_ChangeSprite(GameObject sender)
    {
        if (CombatManager.instance.GetState() != CombatManager.StateMachine.Idle)
            return;
 

        _speedImage.GetComponent<Image>().sprite = _speedSprites[GetSpeedMultipler()];
         
    }

    private void OnPause_ChangeSprite(GameObject sender)
    {
        _speedImage.GetComponent<Image>().sprite = _speedSprites[0];
    }


    public void SetSpeed(int time) {
        EventManager.instance.PlayOnStopPause(gameObject) ;
        _speedImage.GetComponent<Image>().sprite = _speedSprites[time];
        _speedMultipler = time;
    }

    public int GetSpeedMultipler()
    {
        return _speedMultipler  ; 
    }

    public void OnMouseDown_IncreaseSpeed()
    {
        if (CombatManager.instance.GetState() != CombatManager.StateMachine.Idle)
            return;

        EventManager.instance.PlayOnStopPause(gameObject);
        _speedMultipler = (_speedMultipler + 1) % _speedSprites.Count;
        _speedImage.GetComponent<Image>().sprite = _speedSprites[_speedMultipler];

        if (_speedMultipler == 0)
            EventManager.instance.PlayOnStopPause(gameObject); 
    }
}
