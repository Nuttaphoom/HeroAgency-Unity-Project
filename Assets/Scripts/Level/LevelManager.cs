using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement; 
public class LevelManager : SavableObject
{ 

    public static LevelManager instance;
    public List<Level> LevelList;

    public Level selectedLevel = null; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetSelectedLevel(Level level)
    {
        selectedLevel = level; 
    }

    public void LoadNextScene()
    {
        if (selectedLevel != null)
        {
             EventManager.instance.PlayOnEnterLevel(selectedLevel) ;
             SceneManager.LoadScene(1);
        }
    }

    public override object CaptureState()
    {
        List<object> s = new List<object>() ;
        foreach (Level level in LevelList)
        {
            s.Add(level.CaptureState());
        }

        return s;
    }

    public override void RestoreState(object state)
    {
        List<object> s = (List<object>) state;
        int i = 0;
        foreach(Level level in LevelList)
        {
            level.RestoreState(s[i]);
            i++; 
        }
    }

     

   
}
