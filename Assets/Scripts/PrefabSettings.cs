
using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSettings : MonoBehaviour
{
    [SerializeField] private GameObject _shipPrefab;
    
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
        ResetSettings();
    }

    private void ResetSettings()
    {
        foreach (var weapon in _weapons)
        {
            weapon.enabled = false;
        }

        _weapons[0].enabled = true;
    }
    
    public void SetWeapon(int index)
    {
        foreach (var weapon in _weapons)
        {
            if ((int) weapon.CurrentWeaponClass == index)
            {
                weapon.enabled = true;
                Debug.Log($"Оружие установлено: {(Weapon.WeaponClass) index} {weapon.enabled}");
            }
            else
            {
                weapon.enabled = false;
            }
        }
    }
}
