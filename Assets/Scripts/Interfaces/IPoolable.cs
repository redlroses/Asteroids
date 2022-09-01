using System;
using UnityEngine;

public interface IPoolable<out T>
{
    event Action<T> OnDisabled;
    
    bool GetActiveSelf();
    
    GameObject GetGameObject();
}
