using System;
using Game.Scripts.Resource;
using Game.Upgrades.Scripts;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    public class PlayerUpgradeHandler : MonoBehaviour
    {
        [SerializeField] private UpgradeSource _healthUpgrade;
        [SerializeField] private UpgradeSource _damageUpgrade;
        [SerializeField] private UpgradeSource _speedUpgrade;

        private void Start()
        {
            UpgradeHealth();
            UpgradeDamage();
            UpgradeSpeed();

            _healthUpgrade.Data.OnUpgrade += UpgradeHealth;
            _damageUpgrade.Data.OnUpgrade += UpgradeDamage;
            _speedUpgrade.Data.OnUpgrade += UpgradeSpeed;
        }

        private void OnDestroy()
        {
            _healthUpgrade.Data.OnUpgrade -= UpgradeHealth;
            _damageUpgrade.Data.OnUpgrade -= UpgradeDamage;
            _speedUpgrade.Data.OnUpgrade -= UpgradeSpeed;
        }

        private void UpgradeHealth()
        {
            Player.Instance.Hitable.UpdateMaxHealth(_healthUpgrade.Data.UpgradedValue);
        }

        private void UpgradeDamage()
        {
            Player.Instance.Shootable.Damage = (int)_damageUpgrade.Data.UpgradedValue;
        }

        private void UpgradeSpeed()
        {
            Player.Instance.Shootable.AttackCooldown = _speedUpgrade.Data.UpgradedValue;
        }
    }
}