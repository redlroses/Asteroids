using UnityEngine;

public class Laser : Weapon
{
    private const string LaserBeamWarmingUp = "LaserBeamWarmingUp";
    private const string LaserBeamCooling = "LaserBeamCooling";

    private readonly float _laserBeamSizeY = 0.25f;
    
    [SerializeField] private float _distance = 2.5f;
    [SerializeField] private float _distanceUpgrade = 1.5f;
    [SerializeField] private LayerMask _hitMask;
    [SerializeField] private SpriteRenderer _laserBeam;
    [SerializeField] private SpriteRenderer _laserSpark;
    [SerializeField] private Transform _spawnPoint;

    private LineRenderer _lineRenderer;
    private Animation _laserBeamAnimation;
    private GameObject _lastDamageableAsteroid;
    private Asteroid _lastDamageableAsteroidComponent;
    private float _laserLength;
    private bool _isActive;

    private void LateUpdate()
    {
        VisualizeLaser();
    }

    public override void Shoot()
    {
        var hit = Physics2D.Raycast(_spawnPoint.position, Vector2.up, _distance, _hitMask);

        if (hit == false)
        {
            return;
        }
        
        var currentDamageableAsteroid = hit.transform.gameObject;
        
        if (currentDamageableAsteroid.Equals(_lastDamageableAsteroid) == false)
        {
            _lastDamageableAsteroid = currentDamageableAsteroid;
            _lastDamageableAsteroidComponent = currentDamageableAsteroid.GetComponent<Asteroid>();
        }
            
        _lastDamageableAsteroidComponent.TakeDamage(Damage);
    }

    public override void Initialize()
    {
        InitializeParameters();
        _laserBeamAnimation = _laserBeam.GetComponent<Animation>();
    }

    protected override void Upgrade()
    {
        _distance += _distanceUpgrade;
    }

    private void VisualizeLaser()
    {
        var hit = Physics2D.Raycast(_spawnPoint.position, Vector2.up, _distance, _hitMask);

        if (hit)
        {
            _laserBeam.size = new Vector2(hit.distance, _laserBeamSizeY);
            _laserSpark.enabled = true;
            _laserSpark.transform.position = hit.point;
            PlayAnimation(LaserBeamWarmingUp, true);
        }
        else
        {
            _laserSpark.enabled = false;
            PlayAnimation(LaserBeamCooling, false);
        }
    }

    private void PlayAnimation(string animationName, bool newState)
    {
        if (newState == _isActive) return;

        _laserBeamAnimation.Play(animationName);
        _isActive = newState;
    }
}