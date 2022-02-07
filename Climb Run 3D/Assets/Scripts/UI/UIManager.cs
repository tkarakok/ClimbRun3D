using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    public GameObject canvas;
    public GameObject coinPanel;
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject inGamePanel;
    public GameObject gameOverPanel;
    public GameObject endGamePanel;
    public GameObject shopPanel;
    [Header("Main Menu")]
    public Text mainMenuTotalCoinText;
    public Text mainMenuLevelText;
    [Header("In Game")]
    public Text inGameCoinText;
    public Text inGameBonusText;
    public Text inGameCurrentLevelText;
    public Text inGameNextLevelText;
    public Slider slider;
    [Header("End Game")]
    public Text endGameBonusText;
    public Text endGameCurrentCoinText;
    public Text endGameMultiplierText;
    public Text endGameTotalCoinText;
    [Header("Shop")]
    public Button shopButton;
    [Header("Settings")]
    public GameObject subSettingsPanel;
    

    [Header("Animator")]
    public Animator animator;

    private GameObject _currentPanel;
    private Vector3 _firstPosition;
    private void Start()

    {
        _currentPanel = mainMenuPanel;
        _firstPosition = shopPanel.transform.position;
    }
    
    #region Buttons
    public void StartGame()
    {
        EventManager.Instance.InGame();
        PanelChange(inGamePanel);
       
    }
    public void RestartGame()
    {
        LevelManager.Instance.ChangeLevel("LEVEL " + LevelManager.Instance.CurrentLevel);
        AudioManager.Instance.PlaySound(AudioManager.Instance.uiClickClip);
    }

    public void NextLevelButton()
    {
        LevelManager.Instance.ChangeLevel("LEVEL " + LevelManager.Instance.CurrentLevel);
        AudioManager.Instance.PlaySound(AudioManager.Instance.uiClickClip);
        
    }
    #endregion

    #region Panel
    public void PanelChange(GameObject openPanel)
    {
        _currentPanel.SetActive(false);
        openPanel.SetActive(true);
        _currentPanel = openPanel;
    }
    #endregion

    #region UI UPDATE
    // main menu UI
    public void MainMenuUIUpdate()
    {
        mainMenuTotalCoinText.text = PlayerPrefs.GetInt("Total").ToString();
        mainMenuLevelText.text = "LEVEL " + (LevelManager.Instance.CurrentLevel + 1).ToString();
    }
    // in game UI
    public void InGameCoinTextUpdate()
    {
        inGameCoinText.text = GameManager.Instance.CurrentCoin.ToString();
    }
    public void InGameBonusTextUpdate()
    {
        inGameBonusText.text = GameManager.Instance.Bonus.ToString();
    }
    public void InGameLevelTextUIUpdate()
    {
        inGameCurrentLevelText.text = (LevelManager.Instance.CurrentLevel + 1).ToString();
        inGameNextLevelText.text = (LevelManager.Instance.CurrentLevel + 2).ToString();
    }
    // end game UI
    public void EndGameUIUpdate()
    {
        
        endGameCurrentCoinText.text = inGameCoinText.text;
        endGameBonusText.text =inGameBonusText.text;
        endGameMultiplierText.text = GameManager.Instance.Multiplier.ToString();
        endGameTotalCoinText.text = GameManager.Instance.CurrentCoin.ToString();
    }

    // shop 
    public void ShopUIUpdate()
    {
        if (PlayerPrefs.GetInt("Total") < 2000 || ShopManager.Instance.UnlockRandomItem())
        {
            shopButton.interactable = false;
        }
        else
        {
            shopButton.interactable = true;
        }
        mainMenuTotalCoinText.text = PlayerPrefs.GetInt("Total").ToString();
    }
    #endregion

    #region GameOver
    public void GameOver()
    {
        PanelChange(gameOverPanel);
        StateManager.Instance.state = State.GameOver;

    }
    #endregion

    #region End Level
    public void EndLevel()
    {
        GameManager.Instance.SetTotalPoint();
        PanelChange(endGamePanel);
        EndGameUIUpdate();
    }
    #endregion

    #region Shop
    public void OpenShop()
    {
        ShopUIUpdate();
        shopPanel.SetActive(true);
        shopPanel.transform.DOMove(mainMenuPanel.transform.position,.35f).OnComplete(()=> coinPanel.transform.SetParent(shopPanel.transform));
        
        AudioManager.Instance.PlaySound(AudioManager.Instance.uiClickClip);
    }
    public void CloseShop()
    {
        shopPanel.transform.DOMove(_firstPosition, .35f).OnComplete(() => shopPanel.SetActive(false));
        coinPanel.transform.SetParent(mainMenuPanel.transform);
        AudioManager.Instance.PlaySound(AudioManager.Instance.uiClickClip);
    }

    #endregion

    #region Settings
    public void SettingsButton()
    {
        animator.SetBool("Settings", true);
        if (subSettingsPanel.activeInHierarchy)
        {
            subSettingsPanel.SetActive(false);
            animator.SetBool("Settings", false);
        }
        else
        {
            subSettingsPanel.SetActive(true);
            StartCoroutine(CloseSettingsPanel());
        }
        
    }

    IEnumerator CloseSettingsPanel()
    {
        yield return new WaitForSeconds(3);
        subSettingsPanel.SetActive(false);
        animator.SetBool("Settings",false);
    }

    #endregion

   
}
