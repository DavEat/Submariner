using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Vars
    [SerializeField] float _movSpeed = 1;
    [SerializeField] float _movSpeedAttenuationByRot = .5f;
    [SerializeField] float _rotSpeed = 1;

    [HideInInspector] public Vector3 destination;
    Vector3 _lastDir;

    protected Transform _transform;
    #endregion
    #region MonoFunctions
    protected virtual void Start()
    {
        if (_transform == null)
            _transform = GetComponent<Transform>();
        destination = _transform.position;
    }
    protected virtual void FixedUpdate()
    {
        MoveThrough();
    }
    #endregion
    #region Functions
    /// <summary>Move straight to destination with a little rotation to go in the good direction</summary>
    /// <returns>return true if the object is close enough from the destination</returns>
    protected virtual bool MoveThrough(float speedMul = 1)
    {
        Vector3 dir = destination - _transform.position;

        bool arrived = (dir).sqrMagnitude < .2f;
        if (!arrived)
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
            transform.position += transform.forward * movSpeed * speedMul  * Time.fixedDeltaTime;
        }
        return arrived;
    }
    #endregion
}
