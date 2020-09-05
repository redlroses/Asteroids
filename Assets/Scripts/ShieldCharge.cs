using UnityEngine;

public class ShieldCharge : SpawningObjects
{
    [SerializeField] private int _chargeCount = 1;

    protected override void ApplyBonus(Player player)
    {
        player.Shield.ShieldCharges += _chargeCount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Проверка на касание с игроком
        if (!collision.gameObject.TryGetComponent(out Shield shieldSystem))
        {
            return;
        }

        shieldSystem.ShieldCharges += _chargeCount;
        Destroy(gameObject);
    }
}
