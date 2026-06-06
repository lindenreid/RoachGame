/*
 * File: GameState.cs
 * Created: 06/06/2026, 2:54:39 PM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public partial class SequenceController : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // Types
    // ------------------------------------------------------------------------
    public class GameState
    {
        // --------------------------------------------------------------------
        // Methods
        // --------------------------------------------------------------------
        public virtual void EnterState(SequenceController controller) {}

        // --------------------------------------------------------------------
        public virtual void ExitState() {}

        // --------------------------------------------------------------------
        public virtual void RunState(float deltaTime) {}
    }

    // ------------------------------------------------------------------------
    public class GameActionState : GameState
    {
        public override void EnterState(SequenceController controller)
        {
            Cursor.lockState = CursorLockMode.Locked;
            controller._player.enabled = true;
        }
    }

    // ------------------------------------------------------------------------
    public class GameCinematicState : GameState
    {
        public override void EnterState(SequenceController controller)
        {
            Cursor.lockState = CursorLockMode.Locked;
            controller._player.enabled = false;
        }
    }

    // ------------------------------------------------------------------------
    public class GameDialogueState : GameState
    {
        public override void EnterState(SequenceController controller)
        {
            Cursor.lockState = CursorLockMode.None;
            controller._player.enabled = false;
        }
    }
}