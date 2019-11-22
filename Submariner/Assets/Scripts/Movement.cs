using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float _movSpeed = 1;
    [SerializeField] float _movSpeedAttenuationByRot = .5f;
    [SerializeField] float _rotSpeed = 1;

    [HideInInspector] public Vector3 destination;
    Vector3 _lastDir;

    Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
        destination = _transform.position;
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        Vector3 dir = destination - _transform.position;
        if ((dir).sqrMagnitude > .2f)
        {

            Vector3 targetDir = destination - transform.position;

            float step = _rotSpeed * Time.fixedDeltaTime;
            //Find the rotation vector
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, .0f);
            newDir.y = 0;

            // Rotate the object by apply the rotation vector
            transform.rotation = Quaternion.LookRotation(newDir);

            float movSpeed = _movSpeed;
            if (_lastDir != newDir) //check if the object is rotating
            {
                _lastDir = newDir;
                movSpeed *= _movSpeedAttenuationByRot; //reduce speed if rotating
            }

            //move the object
            transform.position += transform.forward * movSpeed * Time.fixedDeltaTime;
        }
    }
}
