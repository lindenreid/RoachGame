/*
 * File: SequenceController.cs
 * Created: 06/06/2026, 2:48:04 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameStateType
{
    Invalid, Action, Cinematic, Dialogue, Menu
}

public partial class SequenceController : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private Sequence _openGameSequence;
    [SerializeField] private Player _player;

    private GameState _activeState;

    private Dictionary<ClueData, Sequence> _sequencesByClueUnlock;
    private Sequence _activeSequence;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public static SequenceController _Instance { get; private set; }

    public GameStateType _ActiveStateType
    {
        get
        {
            if(_activeState == null) return GameStateType.Invalid;
            else if(_activeState is GameActionState) return GameStateType.Action;
            else if(_activeState is GameCinematicState) return GameStateType.Cinematic;
            else if(_activeState is GameDialogueState) return GameStateType.Dialogue;
            return GameStateType.Invalid;
        }
    }

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this);
            return;
        }

        _Instance = this;
    }

    // ------------------------------------------------------------------------
    private void Start ()
    {
        _sequencesByClueUnlock = new Dictionary<ClueData, Sequence>();
        foreach(Sequence sequence in gameObject.GetComponentsInChildren<Sequence>())
        {
            if(_sequencesByClueUnlock.Keys.Contains(sequence._TriggerClue))
            {
                Debug.LogError("sequences have idential start keys: " + sequence + " and " + _sequencesByClueUnlock[sequence._TriggerClue]);
                continue;
            }

            _sequencesByClueUnlock.Add(sequence._TriggerClue, sequence);
        }

        EventBus._Instance.ClueUnlocked += HandleClueUnlocked;
        EventBus._Instance.VisitDialogueNode += HandleVisitDialogueNode;

        ActivateSequence(_openGameSequence);
    }

    // ------------------------------------------------------------------------
    private void HandleClueUnlocked (ClueData clue)
    {
        if(_sequencesByClueUnlock.Keys.Contains(clue))
        {
            Sequence unlockedSeq = _sequencesByClueUnlock[clue];
            if(unlockedSeq != null)
            {
                ActivateSequence(unlockedSeq);
            }   
        }
    }

    // ------------------------------------------------------------------------
    private void ActivateSequence (Sequence sequence)
    {
        _activeSequence = sequence;
        _activeSequence.StartSequence();
        EnterState(_activeSequence._GameStateType);
        Debug.LogFormat("unlocked sequence: {0}", _activeSequence);
    }

    // ------------------------------------------------------------------------
    private void HandleVisitDialogueNode(DialogueNode node)
    {
        EnterState(GameStateType.Dialogue);
    }

    // ------------------------------------------------------------------------
    private void EnterState(GameStateType newState)
    {
        _activeState?.ExitState();

        switch(newState)
        {
            case GameStateType.Action: _activeState = new GameActionState(); break;
            case GameStateType.Cinematic: _activeState = new GameCinematicState(); break;
            case GameStateType.Dialogue: _activeState = new GameDialogueState(); break;
            case GameStateType.Menu: _activeState = new GameMenuState(); break;
            default: Debug.LogError("unhandled game state: " + newState); break;
        }
        Debug.LogFormat("{0} new state: {1}", gameObject.name, _activeState);
        _activeState.EnterState(this);
    }

    // ------------------------------------------------------------------------
    // timeline signal callback
    public void EndCurrentSequence ()
    {
        _activeSequence.EndSequence();
    }

    // ------------------------------------------------------------------------
    // button callback
    public void RestartActionSequence ()
    {
        if(_activeSequence == null || _activeSequence._GameStateType != GameStateType.Action)
        {
            Debug.LogError("No active sequence, or active sequence is not action.");
            return;
        }

        _activeSequence.RestartSequence();
    }
}
