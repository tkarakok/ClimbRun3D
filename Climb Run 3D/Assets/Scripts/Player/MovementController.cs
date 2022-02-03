using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementController : Singleton<MovementController>
{
    public Animator animator;

    [SerializeField] private float _limitX = 2;
    [SerializeField] private float _xSpeed = 25;
    [SerializeField] private float _forwardSpeed = 2;
    private float _lastTouchedX;
    void Update()
    {
        if (StateManager.Instance.state == State.InGame && StateManager.Instance.status == Status.OffClimb)
        {
            float _touchXDelta = 0;
            float _newX = 0;
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    _lastTouchedX = Input.GetTouch(0).position.x;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    _touchXDelta = 5 * (Input.GetTouch(0).position.x - _lastTouchedX) / Screen.width;
                    _lastTouchedX = Input.GetTouch(0).position.x;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                _touchXDelta = Input.GetAxis("Mouse X");
            }
            _newX = transform.position.x + _xSpeed * _touchXDelta * Time.deltaTime;
            _newX = Mathf.Clamp(_newX, -_limitX, _limitX);



            Vector3 newPosition = new Vector3(_newX, transform.position.y, transform.position.z + _forwardSpeed * Time.deltaTime);
            transform.position = newPosition;

        }
    }

    
}
