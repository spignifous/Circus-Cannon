using UnityEngine;

namespace Game.StateMachine
{
    public abstract class State
    {
        public string name;
        protected StateMachine stateMachine;

        protected float startTime;

        private AnimatorClipInfo[] animatorInfo;

        public State(string name, StateMachine stateMachine)
        {
            this.name = name;
            this.stateMachine = stateMachine;
        }

        public virtual void OnEnter()
        {
            DoChecks();
            startTime = Time.time;
        }

        public virtual void OnUpdate() => DoChecks();
        public virtual void OnFixedUpdate() { }
        public virtual void OnLateUpdate() { }
        public virtual void OnExit() { }
        public virtual void DoChecks() { }
        public bool TimeReached(float targetTime) => (Time.time > startTime + targetTime);

        protected bool AnimationIsName(Animator animator, string name)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
        }

        protected bool AnimationEnd(Animator animator, string name)
        {
            return AnimationIsName(animator, name) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
        }
    }
}