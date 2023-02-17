using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CombatManager.instance.GetState() != CombatManager.StateMachine.Idle)
                return;

            if (!FindObjectOfType<PlayerController>().Pause)
                EventManager.instance.PlayOnPause(this.gameObject);
            else
                EventManager.instance.PlayOnStopPause(this.gameObject);

        }

        //Speed control
        if (CombatManager.instance.GetState() == CombatManager.StateMachine.Idle)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                FindObjectOfType<SpeedManager>().SetSpeed(1);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                FindObjectOfType<SpeedManager>().SetSpeed(2);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                FindObjectOfType<SpeedManager>().SetSpeed(3);
        }
        if (Input.GetMouseButtonDown(0))
		{
			FindObjectOfType<Player>().MouseDown();  		 
		}
		 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<PauseMenu>().OpenPauseMenu(); 
        }

        //Cheat Code 
        if (Input.GetKeyDown(KeyCode.D))
        {
            FindObjectOfType<Deck>().DrawNewCardIntoDeck();
        }
    }

	 
}
