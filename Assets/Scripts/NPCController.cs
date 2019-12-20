using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LazerController))]
[RequireComponent(typeof(IKAnimationController))]
public class NPCController : MonoBehaviour
{
    [Header("Target register system")]
    public TargetRegisterSystem RegisterSystem;
    [Header("Target search")]
    public Transform Seeker;
    public float FieldOfView;
    [Header("Shooting")]
    public float CoroutinesWait;
    public float MaxShootRandomAngle;
    public int MinDamagePerAction;
    public int MaxDamagePerAction;

    WaitForSeconds _actionWait;
    NavMeshAgent _agent;
    LazerController _lazer;
    IKAnimationController _IKanimator;
    AbstractTarget _currentTarget;

    public static event Action</*who*/NPCController,/*what*/Collider,/*damage*/int> HittedSomething;
    public event Action KilledSomeone;

    // Start is called before the first frame update
    void Start()
    {
        _actionWait = new WaitForSeconds(CoroutinesWait);
        _agent = GetComponent<NavMeshAgent>();
        _lazer = GetComponent<LazerController>();
        _IKanimator = GetComponent<IKAnimationController>();
        _currentTarget = null;

        StartCoroutine(SeekAndDestroy());
    }

    public void DisableAI()
    {
        StopAllCoroutines();
        _agent.velocity = Vector3.zero;
        _agent.isStopped = true;
        _lazer.RemoveLazer();
        _IKanimator.Target = null;
        _currentTarget = null;
    }

    public void IncreaseFragCount()
    {
        KilledSomeone?.Invoke();
    }

    bool TargetValid
    {
        get => _currentTarget != null && _currentTarget.IsDead == false;
    }

    bool TargetVisible
    {
        get
        {
            Seeker.transform.LookAt(_currentTarget.AimSpot.transform.position);

            if (Vector3.Angle(Seeker.transform.forward, this.transform.forward) > FieldOfView)
                return false;

            if (Physics.Raycast(Seeker.transform.position, Seeker.transform.forward, out RaycastHit hitInfo))
            {
                AbstractTarget raycastedTarget = hitInfo.collider.gameObject.GetComponentInParent(typeof(AbstractTarget)) as AbstractTarget;

                return raycastedTarget != null && raycastedTarget == _currentTarget;
            }
            else
                return false;
        }
    }

    IEnumerator SeekAndDestroy()
    {
        while (true)
        {
            yield return _actionWait;

            AbstractTarget newTarget = RegisterSystem.ProvideTarget();

            if (newTarget != null)
            {
                _currentTarget = newTarget;
                break;
            }
        }

        StartCoroutine(RunToTarget());
    }

    IEnumerator RunToTarget()
    {
        if (_agent.isStopped)
            _agent.isStopped = false;

        Vector3 targetPosition = _currentTarget.transform.position;
        _agent.SetDestination(targetPosition);

        while (TargetValid && !TargetVisible)
        {
            yield return _actionWait;

            if (_currentTarget.transform.position != targetPosition)
            {
                targetPosition = _currentTarget.transform.position;
                _agent.SetDestination(targetPosition);
            }
        }

        _agent.velocity = Vector3.zero;
        _agent.isStopped = true;

        if (TargetValid)
            StartCoroutine(AttackTheTarget());
        else//Someone else destroyed it
            StartCoroutine(SeekAndDestroy());
    }

    IEnumerator AttackTheTarget()
    {
        //Look at target
        _IKanimator.Target = _currentTarget.AimSpot;

        while (TargetValid && TargetVisible)
        {
            //Aim to target
            //Seeker.LookAt(_currentTarget.AimSpot); //Already done in TargetVisible property

            //Add little randomization
            float rotateAngle = UnityEngine.Random.Range(-MaxShootRandomAngle, MaxShootRandomAngle);
            Seeker.Rotate(rotateAngle, rotateAngle, 0f);

            //Shoot
            Vector3 hitPoint;

            if (Physics.Raycast(Seeker.position, Seeker.forward, out RaycastHit hitInfo))
            {
                hitPoint = hitInfo.point;
                HittedSomething?.Invoke(this, hitInfo.collider, UnityEngine.Random.Range(MinDamagePerAction, MaxDamagePerAction));
            }
            else
                hitPoint = Seeker.position + Seeker.forward * 100f;

            _lazer.DrawLazer(hitPoint);
            yield return _actionWait;

            //Stop shooting
            _lazer.RemoveLazer();
            yield return _actionWait;
        }

        _IKanimator.Target = null;

        if (!TargetValid)//Target destroyed
        {
            _currentTarget = null;
            StartCoroutine(SeekAndDestroy());
        }
        else//Lost target's sight
        {
            StartCoroutine(RunToTarget());
        }
    }
}