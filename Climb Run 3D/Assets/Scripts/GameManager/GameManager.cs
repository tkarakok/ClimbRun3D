using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public GameObject plusPointPrefab;


    public IEnumerator PlusPoint(Transform target)
    {
        GameObject point = Instantiate(plusPointPrefab);
        point.transform.position = target.position;
        point.transform.DOMoveY(10, 6);
        yield return new WaitForSeconds(1);
        Destroy(point);
        
    }
}
