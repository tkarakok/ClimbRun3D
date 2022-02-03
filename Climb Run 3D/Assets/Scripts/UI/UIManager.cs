
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject canvas;
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject inGamePanel;
    public GameObject gameOverPanel;
    public GameObject endGamePanel;
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


    private GameObject _currentPanel;

    private void Start()
    {
        _currentPanel = mainMenuPanel;
        
        MainMenuUIUpdate();
        
    }
    
    #region Buttons
    public void StartGame()
    {
        StateManager.Instance.state = State.InGame;
        PanelChange(inGamePanel);
        InGameBonusTextUpdate();
        InGameCoinTextUpdate();
        InGameLevelTextUIUpdate();
        MovementController.Instance.animator.SetBool("Run", true);
    }
    public void RestartGame()
    {
        LevelManager.Instance.ChangeLevel("LEVEL " + LevelManager.Instance.CurrentLevel);
    }

    public void NextLevelButton()
    {
        PlayerPrefs.SetInt("Level", LevelManager.Instance.CurrentLevel++);
        LevelManager.Instance.ChangeLevel("LEVEL " + PlayerPrefs.GetInt("Level"));
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
        mainMenuLevelText.text = "LEVEL " +(LevelManager.Instance.CurrentLevel + 1).ToString();
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
    #endregion

    #region GameOver
    public void GameOver()
    {
        PanelChange(gameOverPanel);

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

}
