using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CollectManager : Singleton<CollectManager>
{
    public GameObject stairPrefab,stairParentPrefab;
    public Transform player,stairParent,parentsParent,spawnPoint;
    public List<GameObject> stairs;

    private int stairCounter=0;

    public int StairCounter { get => stairCounter; set => stairCounter = value; }

    public void InstantiateStair()
    {
        StairCounter++;
        GameObject stair = Instantiate(stairPrefab);
        stairs.Add(stair);
        if (StairCounter !=1)
        {
            stair.transform.position = spawnPoint.position + new Vector3(0, (StairCounter - 1) * .4f, 0);
        }
        else
        {
            stair.transform.position = spawnPoint.position;
        }
        stair.transform.SetParent(stairParent);
    }

    public void ObstacleMinusStair(int value)
    {
        for (int i = 0; i < value; i++)
        {
            StairCounter--;
            GameObject stair = stairs[stairs.Count - 1];
            stairs.Remove(stair);
            Destroy(stair);
        }
        
    }

    public void Climb(Vector3 climbRotate,Vector3 climbTilt,Transform target)
    {
        
        stairParent.parent = null;
        stairParent.DOMove(new Vector3(player.position.x,stairParent.transform.position.y,player.position.z + 1),2);
        stairParent.DORotate(climbRotate, 1).OnComplete(() => stairParent.DORotate(climbTilt, .5f));
        StartCoroutine(PlayerMove(target));
        
    }


    IEnumerator PlayerMove(Transform target)
    {
        yield return new WaitForSeconds(2.5f);
        player.DOMove(stairParent.GetChild(stairParent.childCount - 1).gameObject.transform.position + new Vector3(0,1,0), 1).OnComplete(() => player.DOMoveZ(target.position.z, 1));
        StartCoroutine(ResetStair());
    }

    IEnumerator PlayerMove()
    {
        yield return new WaitForSeconds(2.5f);
        player.DOMove(stairParent.GetChild(stairParent.childCount - 1).gameObject.transform.position + new Vector3(0, 1, 0), 1).OnComplete(() => player.DOMoveZ(player.position.z + 1, 1));
        
    }

    IEnumerator ResetStair()
    {
        yield return new WaitForSeconds(2);
        GameObject newStairParent = Instantiate(stairParentPrefab);
        newStairParent.transform.position = parentsParent.position;
        newStairParent.transform.SetParent(parentsParent);
        stairParent = newStairParent.transform;
        StateManager.Instance.status = Status.OffClimb;
        StairCounter = 0;
    }


    public void Finish(Vector3 climbRotate,Vector3 climbTilt,Transform finishPosition)
    {
        stairParent.parent = null;
        stairParent.DOMove(new Vector3(player.position.x, stairParent.transform.position.y, finishPosition.position.z), 2);
        stairParent.DORotate(climbRotate, 1).OnComplete(() => stairParent.DORotate(climbTilt, .5f));
        StartCoroutine(PlayerMove());
    }

    
    
}
