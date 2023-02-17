using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HeroInventoryManager : MonoBehaviour
{
    private Hero _owner ;
    private List<Equipment> _equiments;

    bool init = false ;

    private void Start()
    {
        Init();
    } 

    private void Init()
    {
        if (init)
            return;

        init = true;
        _equiments = new List<Equipment>(); 
    }

    public HeroInventoryManager(Hero owner)
    {
        this._owner = owner;
    }

    public void EquipItem(Equipment equipment)
    {
        Init();

        _equiments.Add(equipment);
        Debug.Log("equip item no." + _equiments.Count);
        equipment.OnEquip(_owner);
        _owner.AddSkill(equipment);
    }

    public List<Equipment> GetItemInInventory()
    {
        Init();
        return _equiments; 
    }

    public int GetItemCount()
    {
        Init();
        return _equiments.Count; 
    }
}
