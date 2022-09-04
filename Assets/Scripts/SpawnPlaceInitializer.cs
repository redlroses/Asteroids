using System.Collections.Generic;
using UnityEngine;

public class SpawnPlaceInitializer : MonoBehaviour
{
    private readonly List<Transform> _spawns= new List<Transform>();
    private readonly List<Transform> _targets = new List<Transform>();

    [SerializeField] private Vector2 _spawnPointsOffset;
    [SerializeField] private Vector2 _targetPointsOffset;
    [SerializeField] private Transform _spawnsParent;
    [SerializeField] private Transform _targetsParent;

    private Vector2 _playableField;

    private void Start()
    {
        _playableField = PlayableFieldLimiter.GetPlayableField(false);
        FillSpawnPoints();
        FillTargetPoints();
        ArrangePoints();
    }

    private void FillSpawnPoints()
    {
        foreach (Transform item in _spawnsParent)
        {
            _spawns.Add(item);
        }
    }
    
    private void FillTargetPoints()
    {
        foreach (Transform item in _targetsParent)
        {
            _targets.Add(item);
        }
    }

    private void ArrangePoints()
    {
        float width = (_playableField.x + _spawnPointsOffset.x) * 2;
        Vector2 distanceBetween = new Vector2(width / (_spawns.Count - 1),0f);
        Vector2 spawnPosition = new Vector2(-(_playableField.x + _spawnPointsOffset.x), _playableField.y + _spawnPointsOffset.y);
        Vector2 targetPosition = new Vector2(-(_playableField.x + _targetPointsOffset.x), -(_playableField.y + _targetPointsOffset.y));
        
        for (int i = 0; i < _spawns.Count; i++)
        {
            _spawns[i].transform.position = spawnPosition + distanceBetween * i;
        }
        
        width = (_playableField.x + _targetPointsOffset.x) * 2;
        distanceBetween = new Vector2(width / (_targets.Count - 1),0f);
        
        for (int i = 0; i < _targets.Count; i++)
        {
            _targets[i].transform.position = targetPosition + distanceBetween * i;
        }
    }
}
