using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    public AbstractTarget DependingTarget;
    [Range(1, 100)]
    public int DamageMultiplier = 1;

    public bool TakeDamage(int damage, out bool killHit)
    {
        if (DependingTarget.IsDead)
        {
            killHit = false;
            return false;
        }
        else
        {
            DependingTarget.TakeDamage(damage * DamageMultiplier);

            if/*now*/(DependingTarget.IsDead)
                killHit = true;
            else
                killHit = false;

            return true;
        }
    }
}