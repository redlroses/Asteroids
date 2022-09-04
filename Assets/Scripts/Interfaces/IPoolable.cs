using System;
using UnityEngine;

public interface IPoolable<out T> where T : Component
{
    event Action<T> Disabled;
}
