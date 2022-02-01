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
            CollectManager.Instance.InstantiateStair();
            StartCoroutine(GameManager.Instance.PlusPoint(other.transform));
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Climb"))
        {
            other.tag = "Untagged";
            Wall wall = other.GetComponent<Wall>();

            StateManager.Instance.status = Status.OnClimb;

            CollectManager.Instance.Climb(wall.climRotate, wall.climbTilt, wall.stairCount, wall.target);

        }
        else if (other.CompareTag("Finish"))
        {
            other.tag = "Untagged";
            Finish finish = other.GetComponent<Finish>();
            StateManager.Instance.status = Status.OnClimb;
            CollectManager.Instance.Finish(finish.climRotate,finish.climbTilt,finish.finishPosition);
        }
        else if (other.CompareTag("Multiplier"))
        {
            Debug.Log(other.GetComponent<Multiplier>().multiplierValue);
        }
    }
}
