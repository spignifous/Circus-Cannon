using UnityEngine;

namespace Game.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State _mainState;

        protected State CurrentState { get; private set; }
        protected State PreviousState { get; private set; }

        private void Start()
        {
            OnStart();

            if (_mainState != null)
            {
                _mainState?.OnEnter();
            }

            if (CurrentState != null)
            {
                CurrentState?.OnEnter();
            }
        }

        private void Update()
        {
            OnUpdate();

            if (_mainState != null)
            {
                _mainState?.OnUpdate();
            }

            if (CurrentState != null)
            {
                CurrentState?.OnUpdate();
            }
        }

        private void FixedUpdate()
        {
            OnFixedUpdate();

            if (_mainState != null)
            {
                _mainState?.OnFixedUpdate();
            }

            if (CurrentState != null)
            {
                CurrentState?.OnFixedUpdate();
            }
        }

        private void LateUpdate()
        {
            OnLateUpdate();

            if (_mainState != null)
            {
                _mainState?.OnLateUpdate();
            }

            if (CurrentState != null)
            {
                CurrentState?.OnLateUpdate();
            }
        }

        public void ChangeState(State newState)
        {
            PreviousState = CurrentState;
            CurrentState?.OnExit();
            CurrentState = newState;
            CurrentState?.OnEnter();
        }

        protected void SetInitialState(State mainState, State startingState)
        {
            _mainState = mainState;
            CurrentState = startingState;
        }

        // Override unity methods
        protected virtual void OnStart() { }

        protected virtual void OnUpdate() { }

        protected virtual void OnFixedUpdate() { }

        protected virtual void OnLateUpdate() { }
    }
}
