using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetRegisterSystem : MonoBehaviour
{
    List<AbstractTarget> _targets = new List<AbstractTarget>();

    public event Action RanOutOfTargets;

    private void Awake()
    {
        AbstractTarget.Unregister += UnregisterTarget;
    }

    private void OnDestroy()
    {
        AbstractTarget.Unregister -= UnregisterTarget;
    }

    void UnregisterTarget(AbstractTarget targetToUnregister)
    {
        //if(!_targets.Contains(targetToUnregister)) //Not needed, Remove checks it anyway
        //    return;

        if(_targets.Remove(targetToUnregister) == true)//Successfully removed, i.e.: target is really from this instance's collection
        {
            if (_targets.Count == 0)
                RanOutOfTargets?.Invoke();
        }
    }

    public AbstractTarget ProvideTarget()
    {
        if (_targets.Count > 0)
            return _targets[UnityEngine.Random.Range(0, _targets.Count)];
        else
            return null;
    }

    public void RegisterTargets(AbstractTarget[] targets)
    {
        _targets.AddRange(targets);
    }
}