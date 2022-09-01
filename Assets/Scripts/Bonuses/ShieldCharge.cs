using UnityEngine;

public class ShieldCharge : Bonus
{
    [SerializeField] private int _chargeCount = 1;

    protected override void TryApplyBonus(Collider2D collision)
    {
        if (collision.TryGetComponent(out Shield shield))
        {
            shield.AddCharge(_chargeCount);
        }
    }
}
