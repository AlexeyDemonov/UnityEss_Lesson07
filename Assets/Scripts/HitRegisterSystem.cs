using UnityEngine;

public class HitRegisterSystem : MonoBehaviour
{
    private void Awake()
    {
        NPCController.HittedSomething += HandleHit;
    }

    private void OnDestroy()
    {
        NPCController.HittedSomething -= HandleHit;
    }

    void HandleHit(NPCController hitter, Collider collider, int damage)
    {
        //Check
        HitBoxController hitbox = collider.GetComponent(typeof(HitBoxController)) as HitBoxController;

        if (hitbox == null)
            return;

        //Apply damage
        //No friendly fire check though
        hitbox.TakeDamage(damage, out bool killed);

        if (killed)
            hitter.IncreaseFragCount();
    }
}