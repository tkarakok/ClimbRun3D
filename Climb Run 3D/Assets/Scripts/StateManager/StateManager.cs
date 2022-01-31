using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    MainMenu,
    InGame,
    EndGame,
    GameOver
}

public enum Status
{
    OnClimb,
    OffClimb
}

public class StateManager : Singleton<StateManager>
{
    public State state;
    public Status status;

    private void Start()
    {
        state = State.InGame;
        status = Status.OffClimb;
    }
}
