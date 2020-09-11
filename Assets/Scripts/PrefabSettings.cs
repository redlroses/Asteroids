using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSettings : MonoBehaviour
{
    [SerializeField] private GameObject _shipPrefab;
    [SerializeField] private UiTMProText _currentWeaponText;
    
    private List<Weapon> _weapons = new List<Weapon>();

    private void Awake()
    {
        foreach (var weapon in _shipPrefab.GetComponents<Weapon>())
        {
           _weapons.Add(weapon);
        }
    }

    private void Start()
    {
        SetWeapon(2);
    }

    public void SetWeapon(int index)
    {
        foreach (var weapon in _weapons)
        {
            if ((int) weapon.CurrentWeaponClass == index)
            {
                weapon.enabled = true;
                _currentWeaponText.SetText(weapon.GetType().ToString());
            }
            else
            {
                weapon.enabled = false;
            }
        }
    }
}
