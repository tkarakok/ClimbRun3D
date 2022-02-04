using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public delegate void StateActions();
    public StateActions MainMenu;
    public StateActions InGame;
    public StateActions EndGame;

    private void Awake()
    {
        MainMenu += SubscribeAllEvent;
        MainMenu += UIManager.Instance.MainMenuUIUpdate;
        MainMenu();
    }

    void SubscribeAllEvent()
    {
        // in game
        #region In game
        InGame += () => StateManager.Instance.state = State.InGame;
        InGame += () => MovementController.Instance.animator.SetBool("Run", true);
        InGame += () => AudioManager.Instance.PlaySound(AudioManager.Instance.uiClickClip);
        InGame += () => AudioManager.Instance.gameMusicAudioSource.enabled = true;
        InGame += UIManager.Instance.InGameBonusTextUpdate;
        InGame += UIManager.Instance.InGameCoinTextUpdate;
        InGame += UIManager.Instance.InGameLevelTextUIUpdate;
        #endregion

        //end game 
        #region End Game
        EndGame += AdManager.Instance.InterstitialAdShow;
        EndGame += LevelManager.Instance.SetLevel;
        EndGame += UIManager.Instance.EndLevel;
        EndGame += () => AudioManager.Instance.PlaySound(AudioManager.Instance.confettiClip);
        EndGame += () => AudioManager.Instance.gameMusicAudioSource.enabled = false; 
        EndGame += () => StateManager.Instance.state = State.EndGame;
        #endregion

    }
}
