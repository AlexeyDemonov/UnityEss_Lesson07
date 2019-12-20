using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NPCHealthBarHolder))]
[RequireComponent(typeof(NPCScoreHolder))]
[RequireComponent(typeof(NPCController))]
[RequireComponent(typeof(NavMeshAgentToAnimationConverter))]
[RequireComponent(typeof(Animator))]
public class NPCToTargetConverter : AbstractTarget
{
    public float DestroyDelay;

    NPCHealthBarHolder _healthBar;
    WaitForSeconds _delay;

    // Start is called before the first frame update
    void Start()
    {
        _healthBar = GetComponent<NPCHealthBarHolder>();
        _delay = new WaitForSeconds(DestroyDelay);
    }

    public override void TakeDamage(int damage)
    {
        if (!base.IsDead)
        {
            base.Health -= damage;
            _healthBar.SetHealth(base.Health);

            if/*now*/(base.IsDead)
            {
                base.RaiseUnregister(this);

                _healthBar.HideHealthBar();
                GetComponent<NPCScoreHolder>().HideScore();
                GetComponent<NPCController>().DisableAI();
                GetComponent<NavMeshAgentToAnimationConverter>().enabled = false;
                GetComponent<Animator>().SetTrigger("Die");
                //Destroy(this.gameObject, DestroyDelay);
                StartCoroutine(DisableAfterDelay());
            }
        }
    }

    IEnumerator DisableAfterDelay()
    {
        yield return _delay;

        this.gameObject.SetActive(false);
    }
}