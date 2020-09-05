using System;
using UnityEngine;

public class Laser : Weapon
{
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask mask;
    [SerializeField] private SpriteRenderer _laserBeam;
    [SerializeField] private SpriteRenderer _laserSpark;
    [SerializeField] private Transform _spawnPoint;

    private LineRenderer _lineRenderer;
    private float _laserLenght;
    private Vector2 _defaultLaserBeamSize;
    private Animation _laserBeamAnimation;
    private State _currentLaserState = State.Deactive;

    private const string LaserBeamWarmingUp = "LaserBeamWarmingUp";
    private const string LaserBeamCooling = "LaserBeamCooling";

    private enum State
    {
        Deactive,
        Active
    }

    protected override void Shoot()
    {
        //.DrawRay(transform.position, Vector2.up * _distance, Color.blue);
        var hit = Physics2D.Raycast(_spawnPoint.position, Vector2.up, _distance, mask);

        if (hit)
        {
            hit.transform.GetComponent<Asteroid>().TakeDamage(Damage);
            //Debug.DrawLine(transform.position, hit.point, Color.red);
            //Debug.Log(hit.collider.ToString());
        }
    }

    protected override void UpgradeWeapon()
    {
        weaponLevel++;

        if (weaponLevel == 2)
        {
            MaxLevel();
        }

        _distance += 1.5f;
    }

    protected override void Initialize()
    {
        _defaultLaserBeamSize = new Vector2(_distance, 0.25f);
        _laserBeamAnimation = _laserBeam.GetComponent<Animation>();
    }

    private void LateUpdate()
    {
        VisualizeLaser();
    }

    private void VisualizeLaser()
    {
        var hit = Physics2D.Raycast(_spawnPoint.position, Vector2.up, _distance, mask);

        if (hit)
        {
            _laserBeam.size = new Vector2(hit.distance, 0.25f);
            _laserSpark.enabled = true;
            _laserSpark.transform.position = hit.point;
            PlayAnimation(LaserBeamWarmingUp, State.Active);
        }
        else
        {
            //_laserBeam.size = _defaultLaserBeamSize;
            _laserSpark.enabled = false;
            PlayAnimation(LaserBeamCooling, State.Deactive);
        }
    }

    private void PlayAnimation(string name, State newState)
    {
        if (newState == _currentLaserState) return;

        _laserBeamAnimation.Play(name);
        _currentLaserState = newState;
    }
}