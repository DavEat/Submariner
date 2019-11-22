using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region Vars
    public static Transform PlayerTransform;

    [SerializeField] LayerMask _layerMask = 0;

    [SerializeField] Movement _applyOn = null;

    [SerializeField] float _scanFrequency = 2f;
    float _nextScan = 0;

    int _lifePoint = 3;
    bool _dead { get { return _lifePoint <= 0; } }
    #endregion
    #region MonoFunctions
    void Awake()
    {
        PlayerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (_dead)
        {
            #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            #else       
            if (Input.GetMouseButtonDown(0))
            #endif
                GameManager.inst.Reload();

            return;
        }

        #if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            InputUp();
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
            InputStay(Input.GetTouch(0).position);
        #else
        if (Input.GetMouseButtonUp(0))
            InputUp();
        else if (Input.GetMouseButton(0))
            InputStay(Input.mousePosition);
        #endif
        if (_nextScan < Time.time)
        {
            _nextScan = Time.time + _scanFrequency;
            GameManager.sonarPing(PlayerTransform.position);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster"))
        {
            Debug.Log("lose one shield");
            collision.collider.enabled = false;
            //collision.collider.gameObject.SetActive(false);
            Destroy(collision.collider.gameObject, .7f);

            GameManager.inst.SetLife(--_lifePoint);
            if (_dead)
                Die();
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            Debug.Log("lose wall");
            GameManager.inst.SetLife(_lifePoint = 0);
            Die();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndSection"))
        {
            Debug.Log("spawn next");
            PathManager.inst.SpawnNext();

            GameManager.inst.UpdateScore((int)PlayerTransform.position.z * 10);
        }
    }
    #endregion
    #region Functions
    /// <summary>Action when the button is release</summary>
    void InputUp()
    {
        _applyOn.destination = _applyOn.transform.position;
    }
    /// <summary>Logic when the button stay press</summary>
    /// <param name="position">Screen position of the input</param>
    void InputStay(Vector3 position)
    {
        float w = Screen.width * .5f;
        float h = Screen.height * .5f;
        //apply a transformation to position to inverse the touch toward the center
        position.x = (position.x - w) * -1 + w;
        position.y = (position.y - h) * -1 + h;

        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, _layerMask))
        {
            _applyOn.destination = hit.point;
        }
    }
    /// <summary>After player die</summary>
    void Die()
    {
        InputUp();
    }
    #endregion
}
