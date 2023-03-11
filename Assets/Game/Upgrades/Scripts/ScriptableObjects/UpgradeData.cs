using System;
using Game.Scripts.Common.SavableValues;
using Game.Scripts.Common.Vibration;
using Game.Scripts.Resource;
using UnityEngine;

namespace Game.Upgrades.Scripts.ScriptableObjects
{
    [CreateAssetMenu]
    public class UpgradeData : ScriptableObject
    {
        public static Action OnAnyUpgrade;
        public Action OnUpgrade;
        [field: SerializeField] public ResourceType CurrencyType;
        [SerializeField] private AnimationCurve _upgradeValues;
        [SerializeField] private AnimationCurve _prices;

        public float MaxLvl { get; private set; }

        private IntDataValueSavable _upgradeData;

        public virtual bool HasNextLvl => MaxLvl > CurrentLvl;
        public int CurrentLvl => _upgradeData.Value;
        public int NextLvlPrice => (int)_prices.Evaluate(CurrentLvl);
        public float UpgradedValue => _upgradeValues.Evaluate(CurrentLvl);

        public void ResetData()
        {
            _upgradeData.Value = 0;
            _upgradeData.Save();
            OnUpgrade?.Invoke();
        }

        public void Init(string key)
        {
            MaxLvl = _prices.keys[_prices.length - 1].time + 1;
            _upgradeData = new IntDataValueSavable(name + key);
            OnUpgrade?.Invoke();
        }

        public bool LvlUp()
        {
            if (!HasNextLvl) return false;

            VibrationHandler.Instance.AddVibration(MyHapticTypes.LightImpact);
            _upgradeData.Value++;
            _upgradeData.Save();
            OnUpgrade?.Invoke();
            OnAnyUpgrade?.Invoke();
            return true;
        }

        public bool AbleToUp => HasNextLvl && ResourceHandler.GetResourceCount(CurrencyType) >= NextLvlPrice;
    }
}