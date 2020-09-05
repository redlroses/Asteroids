using UnityEngine;

public class Cleaner : MonoBehaviour
{
    [SerializeField] private Transform _asteroids;
    [SerializeField] private Transform _projectiles;
    [SerializeField] private Transform _bonuses;
    
    public void OnEndPlay()
    {
        CleanContainer(_asteroids);
        CleanContainer(_projectiles);
        CleanContainer(_bonuses);
    }

    private void CleanContainer(Transform list )
    {
        foreach (Transform item in list)
        {
            Destroy(item.gameObject);
        }
    }
}
