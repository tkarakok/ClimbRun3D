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
            AudioManager.Instance.PlaySound(AudioManager.Instance.collectClip);
            
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Climb"))
        {
            other.tag = "Untagged";
            Wall wall = other.GetComponent<Wall>();

            StateManager.Instance.status = Status.OnClimb;

            CollectManager.Instance.Climb(wall.climRotate, wall.climbTilt, wall.stairSize, wall.speed,wall.target);


        }
        else if (other.CompareTag("Finish"))
        {
            other.tag = "Untagged";
            AudioManager.Instance.PlaySound(AudioManager.Instance.finishClip);
            Finish finish = other.GetComponent<Finish>();
            StateManager.Instance.status = Status.OnClimb;
            CollectManager.Instance.Finish(finish.climRotate, finish.climbTilt, finish.finishPosition);
        }
        else if (other.CompareTag("Multiplier") && StateManager.Instance.state == State.InGame)
        {
            other.tag = "Untagged";
            AudioManager.Instance.PlaySound(AudioManager.Instance.confettiClip);
            AudioManager.Instance.gameMusicAudioSource.enabled = false;
            GameManager.Instance.Multiplier = other.GetComponent<Multiplier>().multiplierValue;
            StateManager.Instance.state = State.EndGame;
            UIManager.Instance.EndLevel();
        }
        else if (other.CompareTag("Minus"))
        {
            other.tag = "Untagged";
            CollectManager.Instance.ObstacleMinusStair(other.GetComponent<Multiplier>().multiplierValue,false);

        }
        else if (other.CompareTag("Plus"))
        {
            other.tag = "Untagged";
            for (int i = 0; i < other.GetComponent<Multiplier>().multiplierValue; i++)
            {
                CollectManager.Instance.InstantiateStair();
            }
        }
    }
}
