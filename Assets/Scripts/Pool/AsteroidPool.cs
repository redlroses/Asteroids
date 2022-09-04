using System;

public class AsteroidPool : ObjectPool<Asteroid>
{
    public event Action<Asteroid> AsteroidDestroyed;
    
    protected override void DisableCopy(Asteroid copy)
    {
        if (copy.IsDestroyedByPlayer)
        {
            AsteroidDestroyed?.Invoke(copy);
        }
        
        base.DisableCopy(copy);
    }
}
