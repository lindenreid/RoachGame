/*
 * File: Sequence.cs
 * Created: 06/06/2026, 2:47:17 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public class Sequence : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Variables
    // ------------------------------------------------------------------------
    [SerializeField] private ClueData _triggerClue;
    [SerializeField] private ClueData _finishClue;
    [SerializeField] private GameObject _objects;
    [SerializeField] private GameStateType _gameStateType;
    [SerializeField] private AudioClip _music;
    [SerializeField] private Transform _playerStartPos;
    [SerializeField] private bool _onlySetPlayerLocOnRestart;

    // stuff for action sequences to keep track of for restarting
    private Roach[] _roaches;
    private Vector3[] _roachOriginalPositions;

    // ------------------------------------------------------------------------
    // Properties
    // ------------------------------------------------------------------------
    public ClueData _TriggerClue => _triggerClue;
    public GameStateType _GameStateType => _gameStateType;
    public AudioClip _Music => _music;

    // ------------------------------------------------------------------------
    // Methods
    // ------------------------------------------------------------------------
    public void StartSequence ()
    {
        EventBus._Instance.InvokeSequenceStarted(this);

        SetupPlayer(!_onlySetPlayerLocOnRestart);

        if(_objects != null)
        {
            _objects.SetActive(true);

            _roaches = _objects.GetComponentsInChildren<Roach>();
            _roachOriginalPositions = new Vector3[_roaches.Length];
            for(int i = 0; i < _roaches.Length; i++)
            {
                _roachOriginalPositions[i] = _roaches[i].transform.position;
            }
        }
    }

    // ------------------------------------------------------------------------
    public void EndSequence ()
    {
        if(_finishClue != null)
        {
            EventBus._Instance.InvokeClueUnlocked(_finishClue);
        }
    }

    // ------------------------------------------------------------------------
    public void RestartSequence()
    {
        if(_gameStateType != GameStateType.Action)
        {
            Debug.LogError("Trying to restart non-action sequence.");
            return;
        }

        for(int i = 0; i < _roaches.Length; i++)
        {
            _roaches[i].ResetRoach(_roachOriginalPositions[i]);
        }

        SetupPlayer(true);
    }

    // ------------------------------------------------------------------------
    private void SetupPlayer (bool setPlayerPosition)
    {
        if(setPlayerPosition && _playerStartPos != null)
        {
            Player._Instance.TeleportTo(_playerStartPos); 
        }

        if(_gameStateType == GameStateType.Action)
        {
            Player._Instance.SetupForActionSequence();
        }
    }
}
