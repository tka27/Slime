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


        public int Damage = 10;
        protected bool IsCooldown;
        private Transform _parent;
        private int _attackHash = Animator.StringToHash("Attack");
        private ReusableCancellationTokenSource _cts;

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
            Scanner.CurrentTarget.TakeDamage(Damage);
            OnDealDamage?.Invoke(Scanner.CurrentTarget);
        }

        protected virtual void StartAttack()
        {
            if (IsCooldown) return;
            if (Scanner.CurrentTarget == null) return;

            IsCooldown = true;
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
            IsCooldown = false;
            OnSwitchAgentState?.Invoke(false);
            Scanner.Scan();
        }

        private void OnValidate()
        {
            gameObject.TrySetComponent(ref _animator);
        }
    }
}