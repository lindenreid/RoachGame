/*
 * File: RoachState.cs
 * Created: 28/05/2026, 11:59:26 AM
 * Author: Travis Reid
 * Copyright 2019 - 2026 Studio Tilia
 */

using UnityEngine;

public partial class Roach
{
    protected abstract class RoachState
    {
        protected Roach _roach;

        public virtual void EnterState(Roach roach)
        {
            _roach = roach;
        }

        public abstract void ExitState();
        public abstract void RunState(float deltaTime);
    }

    protected class RoachIdleState : RoachState
    {
        private float _maxStateTime;
        private float _antennaeAnimTime;
        private Vector3 _leftRot;
        private Vector3 _rightRot;

        public override void EnterState(Roach roach)
        {
            base.EnterState(roach);

            _roach._stateTime = 0;
            _maxStateTime = Random.Range(_roach._idleTimeMinMax.x, _roach._idleTimeMinMax.y);
            _leftRot = Vector3.Lerp(_roach._antennaeAnimMin, _roach._antennaeAnimMax, Random.Range(0.0f, 1.0f));
            _rightRot = Vector3.Lerp(_roach._antennaeAnimMin, _roach._antennaeAnimMax, Random.Range(0.0f, 1.0f));
        }

        public override void ExitState()
        {
            
        }

        public override void RunState(float deltaTime)
        {
            _antennaeAnimTime += Time.deltaTime;
            if(_antennaeAnimTime >= _roach._antennaeFlipTime)
            {
                _antennaeAnimTime = 0;
                _leftRot = new Vector3(-_leftRot.x, _leftRot.y, _leftRot.z);
                _rightRot = new Vector3(-_rightRot.x, _rightRot.y, _rightRot.z);
            }
            _roach._leftAntennae.Rotate(_leftRot * Time.deltaTime);
            _roach._rightAntennae.Rotate(_rightRot * Time.deltaTime);

            _roach._stateTime += Time.deltaTime;
            if(_roach._stateTime >= _maxStateTime)
            {
                _roach.EnterState(RoachStateType.Running);
            }
        }
    }

    protected class RoachRunningState : RoachState
    {
        public override void EnterState(Roach roach)
        {
            base.EnterState(roach);

            _roach.ResetAntennae();

            _roach.transform.Rotate(0, Random.Range(0, 350), 0);
            _roach._roachSplines.Rotate(0, Random.Range(0, 350), 0);
            _roach._roachSplines.position = _roach.transform.position;

            _roach._movementSplineAnimator.Restart(true);
        }

        public override void ExitState()
        {
            
        }

        public override void RunState(float deltaTime)
        {
            if(!_roach._movementSplineAnimator.IsPlaying)
            {
                _roach.EnterState(RoachStateType.Idle);
            }
        }
    }
    protected class RoachDeadState : RoachState
    {
        public override void EnterState(Roach roach)
        {
            base.EnterState(roach);

            _roach.ResetAntennae();

            _roach._roachSplines.position = _roach.transform.position;
            _roach._deathSplineAnimator.Play();
        }

        public override void ExitState()
        {
            
        }

        public override void RunState(float deltaTime)
        {
            
        }
    }

    protected class RoachCollectedState : RoachState
    {
        public override void EnterState(Roach roach)
        {
            base.EnterState(roach);

            _roach._movementSplineAnimator.enabled = false;
            _roach._deathSplineAnimator.enabled = false;

            _roach._collider.enabled = false;
            EventBus.Instance.InvokeRoachCollected(_roach);
        }

        public override void ExitState()
        {
            
        }

        public override void RunState(float deltaTime)
        {
            
        }
    }

    protected class RoachAttackingState : RoachState
    {
        public override void EnterState(Roach roach)
        {
            base.EnterState(roach);
        }

        public override void ExitState()
        {
            
        }

        public override void RunState(float deltaTime)
        {
            
        }
    }
}