using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Used to show detain in a paper in world-map scene. 
public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private Level _assignedLevel;
    [SerializeField]
    private GameObject _levelDetailHolder ;

    [SerializeField]
    private TextMeshProUGUI _levelNOText; 
    private void Start()
    {
        _levelNOText.text = _assignedLevel.GetLevelData().Item2;
    }

    public void PointerEnter()
    {
        _levelDetailHolder.SetActive(true);
    }

    public void PointerExit()
    {
        _levelDetailHolder.SetActive(false);
    }

    public void OnClick_EnterSelectingStage()
    {
        FindObjectOfType<CharacterCardSelector>().EnterStage();
        LevelManager.instance.SetSelectedLevel(_assignedLevel);

    }
}
