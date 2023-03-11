using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SubLib.Extensions;
using SubLib.ObjectPool;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    public class Shootable : Attackable
    {
        [SerializeField] private Transform _projectileStart;
        [SerializeField] private ObjectPool<Projectile> _projectilesPool;
        public float AttackCooldown = 0.85f;

        private CancellationToken _token;

        protected override async void StartAttack()
        {
            if (IsCooldown) return;
            if (!Scanner.CurrentTarget) return;
            var token = this.GetCancellationTokenOnDestroy();
            ResetCD();
            base.StartAttack();

            var target = Scanner.CurrentTarget;
            var projectile = _projectilesPool.Get(_projectileStart.position);
            await projectile.transform.MoveAsync(Scanner.CurrentTarget.AimPoint.position, token);
            if (token.IsCancellationRequested) return;
            projectile.gameObject.SetActive(false);

            OnDealDamage?.Invoke(target);
            target.TakeDamage(Damage);
        }


        public override async void ResetCD()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(AttackCooldown), this.GetCancellationTokenOnDestroy());
            }
            catch
            {
                return;
            }

            base.ResetCD();
        }
    }
}