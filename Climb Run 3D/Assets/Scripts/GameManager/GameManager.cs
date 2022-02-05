using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public GameObject plusPointPrefab;
    public GameObject minusPointPrefab;
    public GameObject plusBonusPrefab;
    public GameObject finishLine;
    public Slider levelProgressBar;

    private int _currentCoin;
    private int _totalCoin;
    private int _bonus;
    private int _multiplier;
    private float _maxDistance;
    
    public int Bonus { get => _bonus; set => _bonus = value; }
    public int Multiplier { get => _multiplier; set => _multiplier = value; }
    public int CurrentCoin { get => _currentCoin; set => _currentCoin = value; }
    public int TotalCoin { get => _totalCoin; set => _totalCoin = value; }

    private void Start()
    {
        _maxDistance = finishLine.transform.position.z - MovementController.Instance.transform.position.z;
        CurrentCoin = 0;
        Bonus = 0;
        TotalCoin = PlayerPrefs.GetInt("Total");
        
    }

    private void Update()
    {
        float distance = finishLine.transform.position.z - MovementController.Instance.transform.position.z;
        levelProgressBar.value = 1 - (distance / _maxDistance);
    }

    public void EarnPoint(int value)
    {
        CurrentCoin += value;
    }

    public void EarnBonus(int value)
    {
        Bonus += value;
    }

    public void SetTotalPoint()
    {
        Bonus = Bonus * Multiplier;
        CurrentCoin += Bonus;
        TotalCoin += CurrentCoin;
        PlayerPrefs.SetInt("Total",TotalCoin);
    }

    public IEnumerator PlusPoint(Transform target)
    {
        GameObject point = Instantiate(plusPointPrefab);
        point.transform.position = target.position;
        point.transform.DOMoveY(10, 6);
        yield return new WaitForSeconds(.5f);
        Destroy(point);
    }
    public IEnumerator MinusPoint(Transform target)
    {
        GameObject point = Instantiate(minusPointPrefab);
        point.transform.position = target.position;
        point.transform.DOMoveY(10, 6);
        yield return new WaitForSeconds(.5f);
        Destroy(point);
    }

    public void PlusBonus(string value)
    {
        GameObject bonusPoint = Instantiate(plusBonusPrefab,UIManager.Instance.canvas.transform);
        bonusPoint.GetComponent<Text>().text = "+" + value;
        bonusPoint.transform.DOMove(UIManager.Instance.inGameBonusText.transform.position,1).OnComplete(()=> Destroy(bonusPoint));
    }

    

   
}
