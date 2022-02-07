using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Transform _target;
    private Vector3 _offsetZ;
    private float _offsetY;


    private void Start()
    {
        _offsetZ = gameObject.transform.position - _target.position;
        //_offsetY = gameObject.transform.position.y - _target.position.y;
    }

    private void LateUpdate()
    {
        if (StateManager.Instance.state == State.InGame)
        {

            Vector3 targetPosition = _target.position + _offsetZ;
           // targetPosition += new Vector3(0,_offsetY,0);
            transform.position = targetPosition;
        }

    }
}