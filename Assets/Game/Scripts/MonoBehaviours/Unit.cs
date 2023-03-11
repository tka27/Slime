using System.Collections.Generic;
using Game.Scripts.Attributes;
using SubLib.Async;
using SubLib.Extensions;
using UnityEngine;
using UnityEngine.AI;
using UtilsSubmodule.States;

namespace Game.Scripts.MonoBehaviours
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Unit : MonoBehaviour, IAutoInit
    {
        [SerializeField, @ReadOnly] private Hitable _hitable;
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Attackable Attackable { get; private set; }
        [field: SerializeField, @ReadOnly] public NavMeshAgent Agent { get; private set; }

        private const float DefaultNavigationDelay = 0.3f;
        private float _navigationDelay;
        protected Hitable Hitable => _hitable;
        public StateMachine StateMachine { get; private set; }

        private void Awake()
        {
            _navigationDelay = DefaultNavigationDelay;
            InitStates();
            Hitable.OnDeath += OnDeath;
        }

        protected void OnDestroy()
        {
            Hitable.OnDeath -= OnDeath;
        }

        private void InitStates()
        {
            Dictionary<UnitState, IState> states = new()
            {
                [UnitState.Idle] = new AgentIdleState(this),
                [UnitState.Run] = new AgentRunState(this),
                [UnitState.Death] = new AgentDeathState(this)
            };

            StateMachine = new(states, UnitState.Idle);
        }

        protected virtual void Update()
        {
            StateMachine.Update();
            _navigationDelay -= Time.deltaTime;
            if (_navigationDelay < 0)
            {
                _navigationDelay = DefaultNavigationDelay;
                Navigate();
            }
        }

        protected abstract void Navigate();

        private async void OnDeath()
        {
            enabled = false;
            var token = AsyncCancellation.Token;
            StateMachine.SetState(UnitState.Death);
            Attackable.enabled = false;
            enabled = false;

            if (!await SubLib.Utils.Async.Delay(2, token)) return;
            await transform.MoveAsync(transform.position + Vector3.down * 2, token, 1);
            if (token.IsCancellationRequested) return;
            Destroy(gameObject);
        }

        public void AutoInit()
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        private void OnValidate()
        {
            if (!Animator) Animator = GetComponentsInChildren<Animator>()[0];
            gameObject.TrySetComponent(ref _hitable);
        }
    }
}