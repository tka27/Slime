using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Attributes;
using SubLib.Async;
using SubLib.Extensions;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    public class Attackable : MonoBehaviour
    {
        public delegate void AgentHandler(bool isStopped);

        public AgentHandler OnSwitchAgentState;
        public Action<Hitable> OnDealDamage;

        [SerializeField, @ReadOnly] private Animator _animator;
        [field: SerializeField] public Scanner Scanner { get; private set; }
        [SerializeField] private int _damage = 10;

        private ReusableCancellationTokenSource _cts;


        private Transform _parent;
        private int _attackHash = Animator.StringToHash("Attack");
        protected bool Cooldown;

        protected int Damage => _damage;

        public bool TargetInRadius =>
            Scanner.CurrentTarget && transform.DistanceTo(Scanner.CurrentTarget.AimPoint.position) < Scanner.Radius;

        private void Awake()
        {
            _cts = new(this.GetCancellationTokenOnDestroy());
            _parent = _animator.transform.parent;
        }

        private void OnEnable()
        {
            _ = _cts.Create();
            Scanner.Run();
            Scanner.OnTargetFound += StartAttack;
        }

        private void OnDisable()
        {
            _cts.Cancel();
            Scanner.Stop();
            Scanner.OnTargetFound -= StartAttack;


            OnSwitchAgentState?.Invoke(true);
        }

        public void DealDamage()
        {
            if (!Scanner.CurrentTarget) return;
            Scanner.CurrentTarget.TakeDamage(_damage);
            OnDealDamage?.Invoke(Scanner.CurrentTarget);
        }

        protected virtual void StartAttack()
        {
            if (Cooldown) return;
            if (Scanner.CurrentTarget == null) return;

            Cooldown = true;
            OnSwitchAgentState?.Invoke(true);
            _animator.SetTrigger(_attackHash);
        }

        private void Update()
        {
            if (!Scanner.CurrentTarget) return;
            _parent.HorizontalSoftLookAt(Scanner.CurrentTarget.transform.position);
            if (!Scanner.CurrentTarget.IsAlive) Scanner.CurrentTarget = null;
        }

        public virtual void ResetCD()
        {
            Cooldown = false;
            OnSwitchAgentState?.Invoke(false);
            Scanner.Scan();
        }

        private void OnValidate()
        {
            gameObject.TrySetComponent(ref _animator);
        }
    }
}