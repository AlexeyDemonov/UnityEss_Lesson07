using UnityEngine;

[RequireComponent(typeof(TargetRegisterSystem))]
public class TargetAutoRegister : MonoBehaviour
{
    private void Awake()
    {
        AbstractTarget[] targets = GetComponentsInChildren<AbstractTarget>();

        if (targets != null && targets.Length > 0)
            GetComponent<TargetRegisterSystem>().RegisterTargets(targets);
    }
}