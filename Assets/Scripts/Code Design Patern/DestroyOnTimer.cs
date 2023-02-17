using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTimer : MonoBehaviour
{
	[SerializeField]
	private float maxTime = 0; 
	float currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        if (currentTime >= maxTime)
		{
			Destroy(gameObject);
		}

		currentTime += 1 * Time.deltaTime;
    }

	private void OnDisable()
	{
		Destroy(gameObject);
	}
}
