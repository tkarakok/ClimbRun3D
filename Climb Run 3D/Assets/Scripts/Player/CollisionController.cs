using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stair"))
        {
            other.tag = "Untagged";
            Destroy(other.gameObject);
            CollectManager.Instance.InstantiateStair();
        }
        else if (other.CompareTag("Climb"))
        {
            other.tag = "Untagged";
            StateManager.Instance.status = Status.OnClimb;
            Wall wall = other.GetComponent<Wall>();
            CollectManager.Instance.Climb(wall.climRotate,wall.climbTilt,wall.stairCount,wall.target);
            
        }
    }
}
