using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _menuHolder;

    private void Update()
    {
        
    }

    public void CloseGame()
    {
        Application.Quit(); 
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenPauseMenu()
    {
        _menuHolder.SetActive(true);
        Time.timeScale = 0;
    }

    public void ClosePauseMenu()
    {
        _menuHolder.SetActive(false);
        Time.timeScale = 1; 
    }

}
