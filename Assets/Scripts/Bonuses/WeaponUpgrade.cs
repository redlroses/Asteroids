using UnityEngine;

public class WeaponUpgrade : Bonus
{
    protected override void TryApply(Collider2D collision)
    {
        if (collision.TryGetComponent(out Shooter shooter))
        {
            shooter.Weapon.UpLevel();
        }
    }
}