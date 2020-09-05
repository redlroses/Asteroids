using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShipSetup : MonoBehaviour
{
    [SerializeField] private GameObject _playerShipPrefab;

    private static PlayerShipSetup _instance;

    private void Awake()
    {
        // Инициализация singleton
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    // Вызывается при загрузке главной игровой сцены
    private void SceneManagerOnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (String.Equals(scene.name, "Main"))
        {
            SpawnPlayerShip();
        }
    }
    
    public void SpawnPlayerShip()
    {
        Instantiate(_playerShipPrefab, new Vector3(0f, -3f, 0f), Quaternion.identity);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
    }
}