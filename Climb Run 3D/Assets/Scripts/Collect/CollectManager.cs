using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CollectManager : Singleton<CollectManager>
{
    public GameObject stairPrefab,stairParentPrefab;
    public Transform player,stairParent,parentsParent,spawnPoint;
    public List<GameObject> _stairs;

    private int stairCounter=0;

    public int StairCounter { get => stairCounter; set => stairCounter = value; }


    #region Stair Instantiate and Destroy
    public void InstantiateStair()
    {
        StairCounter++;
        GameObject stair = Instantiate(stairPrefab);
        _stairs.Add(stair);
        if (StairCounter != 1)
        {
            stair.transform.position = spawnPoint.position + new Vector3(0, (StairCounter - 1) * .4f, 0);
        }
        else
        {
            stair.transform.position = spawnPoint.position;
        }
        stair.transform.SetParent(stairParent);
    }

    public void ObstacleMinusStair(int value,bool bonus)
    {
        for (int i = 0; i < value; i++)
        {
            StairCounter--;
            GameObject stair = _stairs[_stairs.Count - 1];
            _stairs.Remove(stair);
            Destroy(stair);
            if (bonus)
            {
                StartCoroutine(GameManager.Instance.PlusPoint(stair.transform));
            }
            
        }

    }
    #endregion


    #region Player Climb To Stair
    public void Climb(Vector3 climbRotate, Vector3 climbTilt,int stairSize , float speed,Transform target)
    {
        MovementController.Instance.animator.SetBool("Run",false);
        stairParent.parent = null;
        stairParent.DOMove(new Vector3(player.position.x, stairParent.transform.position.y, player.position.z + 1), 2);
        stairParent.DORotate(climbRotate, 1).OnComplete(() => stairParent.DORotate(climbTilt, .5f));
        if (stairSize > StairCounter)
        {
            // game over
            Debug.Log("Game Over");
        }
        else
        {
            int bonus = StairCounter - stairSize;
            ObstacleMinusStair(bonus,true);
            
            StartCoroutine(PlayerMove(target,speed));
        }
        

    }


    IEnumerator PlayerMove(Transform target,float speed)
    {
        yield return new WaitForSeconds(2.5f);
        MovementController.Instance.animator.SetBool("Climb", true);
        player.DOMoveZ(target.position.z, 1);
        player.DOMove(stairParent.GetChild(stairParent.childCount - 1).gameObject.transform.position,2 ).OnComplete(() => player.DOMoveZ(target.position.z, 1)) ;
        StartCoroutine(ResetStair());
    }

    IEnumerator PlayerMove()
    {
        yield return new WaitForSeconds(2.5f);
        player.DOMove(stairParent.GetChild(stairParent.childCount - 1).gameObject.transform.position , 3).OnComplete(FinishMove);

    }

    IEnumerator ResetStair()
    {
        yield return new WaitForSeconds(2);
        MovementController.Instance.animator.SetBool("Run",true);
        MovementController.Instance.animator.SetBool("Climb",false);
        GameObject newStairParent = Instantiate(stairParentPrefab);
        newStairParent.transform.position = parentsParent.position;
        newStairParent.transform.SetParent(parentsParent);
        stairParent = newStairParent.transform;
        StateManager.Instance.status = Status.OffClimb;
        StairCounter = 0;
    }

    void FinishMove()
    {
        player.DOMoveZ(player.position.z + 1, 1);
        MovementController.Instance.animator.SetBool("Win", true);
    }

    IEnumerator FinishAnimation()
    {
        yield return new WaitForSeconds(2);
        MovementController.Instance.animator.SetBool("Climb", true);
    }

    public void Finish(Vector3 climbRotate, Vector3 climbTilt, Transform finishPosition)
    {
        MovementController.Instance.animator.SetBool("Run", false);
        StartCoroutine(FinishAnimation());
        stairParent.parent = null;
        stairParent.DOMove(new Vector3(player.position.x, stairParent.transform.position.y, finishPosition.position.z), 2);
        stairParent.DORotate(climbRotate, 1).OnComplete(() => stairParent.DORotate(climbTilt, .5f));
        StartCoroutine(PlayerMove());
    }
    #endregion



}
