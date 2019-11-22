using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarEffect : MonoBehaviour
{
    #region Vars
    [SerializeField] Transform _emitter = null;

    public Material Effect_Cave;
    public Material Effect_Monster;

    bool _scanning;
    float _scanDistance;
    [SerializeField] float _maxScanDistance = 350;
    #endregion
    void Start()
    {
        GameManager.sonarPing += Ping;
    }
    void Update()
    {
        if (_scanning)
        {
            _scanDistance += Time.deltaTime * 5;
            Effect_Cave.SetFloat("_ScanDistance", _scanDistance);
            Effect_Monster.SetFloat("_ScanDistance", _scanDistance);

            if (_scanDistance > _maxScanDistance)
                _scanning = false;
        }

        Effect_Cave.SetVector("_WorldSpaceScannerPos", _emitter.position);
        Effect_Monster.SetVector("_WorldSpaceScannerPos", _emitter.position);
    }
    /// <summary>Start sonar shader</summary>
    /// <param name="pingSource">the ping source point</param>
    void Ping(Vector3 pingSource)
    {
        _scanDistance = 0;
        _scanning = true;

        Effect_Cave.SetVector("_WorldSpaceScannerPos", pingSource);
        Effect_Monster.SetVector("_WorldSpaceScannerPos", pingSource);
    }
}
