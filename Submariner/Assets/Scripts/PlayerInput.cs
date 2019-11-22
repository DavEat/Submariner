using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask = 0;

    [SerializeField] Movement _applyOn = null;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, _layerMask))
            {
                _applyOn.destination = hit.point;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.sonarPing(_applyOn.transform.position);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

    }
}
