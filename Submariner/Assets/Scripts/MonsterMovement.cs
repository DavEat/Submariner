using UnityEngine;

public class MonsterMovement : Movement
{
    #region Vars
    [SerializeField] float _turnAroundSpeed = 2;
    [SerializeField] Vector2 _turnPivot = Vector2.one;

    [SerializeField] float _pingMoveDst = 10;
    [SerializeField] float _pingActionDst = 10;

    Vector3 _pivotAbsPosition = Vector3.zero;

    bool _moveToPing = false;
    bool _inCharge = false;
    [SerializeField] float _viewRadius = 2.5f;

    float _startChasingTime;
    [SerializeField] float _maxChasingTime = 8;
    #endregion
    #region MonoFunctions
    protected override void Start()
    {
        base.Start();
        CalculatePivotAbsPosition();
        // add ping to the delegate SonarPing to be call at each ping
        GameManager.sonarPing += Ping;
    }
    protected override void FixedUpdate()
    {
        if (_inCharge)
            Charge();
        else
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

            CheckEye();
        }
    }
    void OnDisable()
    {
        //Clear the delegate SonarPing for this instance before disable or destroy
        GameManager.sonarPing -= Ping;
    }
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (_moveToPing)
            return;

        Gizmos.color = Color.red;
        Vector3 center = Vector3.zero;
        if (Application.isPlaying)
            center = _pivotAbsPosition;
        else center = transform.position + transform.rotation * new Vector3(_turnPivot.x, 0, _turnPivot.y);

        Gizmos.DrawSphere(center, .1f);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(center, Vector3.up, _turnPivot.magnitude);
    }
    #endif
    #endregion
    #region Functions
    /// <summary>Init var from this class</summary>
    /// <param name="_turnPivot">the ponster will tiurn around this pivot</param>
    public void Init(Vector2 turnPivot)
    {
        _turnPivot = turnPivot;
        CalculatePivotAbsPosition();
    }
    /// <summary>Calculate the pivot to rotate around</summary>
    void CalculatePivotAbsPosition()
    {
        if (_transform == null)
            _transform = GetComponent<Transform>();

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
        if (dir.sqrMagnitude < _pingActionDst * _pingActionDst)
        {
            destination = Vector3.Lerp(_transform.position, pingSource, _pingMoveDst / dir.magnitude);
            _moveToPing = true;
        }
    }
    /// <summary>Check if the monster can see the player with his eyes</summary>
    void CheckEye()
    {
        if ((_transform.GetChild(0).position - PlayerInput.PlayerTransform.position).sqrMagnitude < _viewRadius * _viewRadius)
        {
            Debug.Log("charge mode");
            _inCharge = true;
            _startChasingTime = Time.time + _maxChasingTime;
        }
    }
    /// <summary>Movement logic of the monster when he is in charge mode</summary>
    void Charge()
    {
        destination = PlayerInput.PlayerTransform.position;
        MoveThrough(4f);

        if (_startChasingTime < Time.time && (_transform.position - PlayerInput.PlayerTransform.position).sqrMagnitude < _viewRadius * _viewRadius)
            Destroy(gameObject);
    }
    #endregion
}
