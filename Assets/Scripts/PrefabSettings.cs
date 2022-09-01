// using System.Collections.Generic;
// using UnityEngine;
//
// public class PrefabSettings : MonoBehaviour
// {
//     [SerializeField] private RecordsContainer _recordsContainer;
//     [SerializeField] private GameObject _shipPrefab;
//     [SerializeField] private UIDisplay _currentWeaponTextShower;
//
//     private List<Weapon> _weapons = new List<Weapon>();
//
//     private void Awake()
//     {
//         foreach (var weapon in _shipPrefab.GetComponents<Weapon>())
//         {
//             _weapons.Add(weapon);
//         }
//     }
//
//     private void Start()
//     {
//         SetWeapon(_recordsContainer.WeaponClass);
//     }
//
//     public void SetWeapon(int index)
//     {
//         foreach (var weapon in _weapons)
//         {
//             if ((int) weapon.CurrentWeaponClass == index)
//             {
//                 weapon.enabled = true;
//                 _currentWeaponTextShower.DisplayText(weapon.GetType().ToString());
//                 _recordsContainer.WeaponClass = index;
//             }
//             else
//             {
//                 weapon.enabled = false;
//             }
//         }
//     }
// }