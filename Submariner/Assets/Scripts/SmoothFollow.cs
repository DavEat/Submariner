using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField] Transform _target = null;
    //[SerializeField] float _speed = 1;
    Transform _transform;
    //Vector3 _velocity = Vector3.zero;

    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        _transform.position = _target.position;
        //_transform.position = Vector3.SmoothDamp(_transform.position, _target.position, ref _velocity, _speed);
    }
}
