using System;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public Weapon Weapon
    {
        get
        {
            foreach (var component in GetComponents<Weapon>())
            {
                if (component.isActiveAndEnabled)
                {
                    return component;
                }
            }
            throw new NullReferenceException("Отсутствует включенный компонент оружия");
        }
    }

    public Shield Shield => GetComponent<Shield>();
    public Mover Mover => GetComponent<Mover>();
}
