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
    public void InstantiateStair(GameObject gameObject)
    {
        StairCounter++;
        GameManager.Instance.EarnPoint(1);
        UIManager.Instance.InGameCoinTextUpdate();
        Destroy(gameObject.GetComponent<Rotator>());
        gameObject.transform.eulerAngles = new Vector3(0,90,0);
        _stairs.Add(gameObject);
        if (StairCounter != 1)
        {
            gameObject.transform.position = spawnPoint.position + new Vector3(0, (StairCounter - 1) * .4f,0);
        }
        else
        {
            gameObject.transform.position = spawnPoint.position;
        }
        gameObject.transform.SetParent(stairParent);
    }
    public void InstantiateStair()
    {
        StairCounter++;
        GameManager.Instance.EarnPoint(1);
        UIManager.Instance.InGameCoinTextUpdate();
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
            if (stairCounter < 0)
            {
                UIManager.Instance.GameOver();
            }
            
            GameObject stair = _stairs[_stairs.Count - 1];
            _stairs.Remove(stair);
            stair.gameObject.SetActive(false);
            stair.transform.SetParent(null);
            
        }
        if (bonus)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.bonusClip);
            GameManager.Instance.EarnBonus(value * 2);
            UIManager.Instance.InGameBonusTextUpdate();
            GameManager.Instance.PlusBonus((value *2).ToString());
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
            UIManager.Instance.GameOver();
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
