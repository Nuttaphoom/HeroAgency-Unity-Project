using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
public class UI_GameOverAndWin : MonoBehaviour
{
	[SerializeField]
	GameObject menu;

    [SerializeField]
    private bool wonMenu = false;

    private bool _gameEnd = false; 

    private void Start()
	{
		EventManager.instance.OnGameOver += OnGameOver_PushMenu;
		EventManager.instance.OnGameWin += OnGameWin_PushMenu; 
	}

    private void Update()
    {
        if (_gameEnd)
            EventManager.instance.PlayOnPause(gameObject);
    }
    private void OnDestroy()
	{
		EventManager.instance.OnGameOver -= OnGameOver_PushMenu;
		EventManager.instance.OnGameWin -= OnGameWin_PushMenu;
	}

	private void OnGameOver_PushMenu(GameObject sender)
	{
        if (wonMenu)
            return;
        _gameEnd = true;
        menu.SetActive(true); 
	}

	private void OnGameWin_PushMenu(GameObject sender)
	{

        if (! wonMenu)
            return;
        _gameEnd = true;
        menu.SetActive(true);
	}

	public void EndLevel()
	{
 		SceneManager.LoadScene(0); 
	}
}
