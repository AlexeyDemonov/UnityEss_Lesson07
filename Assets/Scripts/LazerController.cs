using UnityEngine;

public class LazerController : MonoBehaviour
{
    public LineRenderer LazerBeam;
    public Transform Origin;

    Vector3 _hitPoint = default(Vector3);
    Transform _target;

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            _hitPoint = _target.position;
        }

        if (LazerBeam.enabled && _hitPoint != default(Vector3))
        {
            LazerBeam.SetPosition(0, Origin.position);
            LazerBeam.SetPosition(1, _hitPoint);
        }
    }

    public void DrawLazer(Transform target)
    {
        _target = target;
        LazerBeam.enabled = true;
    }

    public void DrawLazer(Vector3 endPosition)
    {
        _hitPoint = endPosition;
        LazerBeam.enabled = true;
    }

    public void RemoveLazer()
    {
        LazerBeam.enabled = false;
        _target = null;
        _hitPoint = default(Vector3);
    }
}