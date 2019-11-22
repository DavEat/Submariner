using UnityEngine;

public class MonsterMovement : Movement
{
    #region Vars
    [SerializeField] float _turnAroundSpeed = 2;
    [SerializeField] Vector2 _turnPivot = Vector2.one;

    [SerializeField] float _pingMoveDst = 10;

    Vector3 _pivotAbsPosition = Vector3.zero;

    bool _moveToPing = false;
    #endregion
    #region MonoFunctions
    protected override void Start()
    {
        base.Start();
        CalculatePivotAbsPosition();
        // add ping to the delegate SonarPing to be call at each ping
        
    }
    protected override void FixedUpdate()
    {
        if (!_moveToPing) //turn around the pivot unless the monster is moving toward the ping source
            TurnAround();
        else
        {
            _moveToPing = !MoveThrough();
            if (!_moveToPing)
            {
                //recalculate the pivot to turn around
                CalculatePivotAbsPosition();
            }
        }
    }
    void OnDisable()
    {
        //Clear the delegate SonarPing for this instance before disable or destroy
        GameManager.sonarPing -= Ping;
    }
    void OnDrawGizmos()
    {
        if (_moveToPing)
            return;

        Gizmos.color = Color.red;
        if (Application.isPlaying)
            Gizmos.DrawSphere(_pivotAbsPosition, .2f);
        else Gizmos.DrawSphere(transform.position + transform.rotation * new Vector3(_turnPivot.x, 0, _turnPivot.y), .2f);
    }
    #endregion
    #region Functions
    /// <summary>Calculate the pivot to rotate around</summary>
    void CalculatePivotAbsPosition()
    {
        _pivotAbsPosition = _transform.position + _transform.rotation * new Vector3(_turnPivot.x, 0, _turnPivot.y);
    }
    /// <summary>Make the transform to around the pivot: _pivotAbsPosition</summary>
    void TurnAround()
    {
        //_transform.RotateAround(_pivotAbsPosition, Vector3.up, _turnAroundSpeed * (Mathf.Sin(Time.time*10) * .2f + 1f));
        float speedMul = 5; //to compensate the Sin and keep roughly the same speed
        _transform.RotateAround(_pivotAbsPosition, Vector3.up, _turnAroundSpeed * speedMul * Time.deltaTime);
    }
    /// <summary>Calculate the destination to move toward when a ping is made</summary>
    /// <param name="pingSource">the ping source point</param>
    void Ping(Vector3 pingSource)
    {
        Vector3 dir = pingSource - _transform.position;
        destination = Vector3.Lerp(_transform.position, dir, _pingMoveDst / dir.magnitude);
        _moveToPing = true;
    }
    #endregion
}
