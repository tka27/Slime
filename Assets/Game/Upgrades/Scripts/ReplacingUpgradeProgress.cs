using Game.Scripts.Attributes;
using Game.Upgrades.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Upgrades.Scripts
{
    public class ReplacingUpgradeProgress : UpgradeProgress, IAutoInit
    {
        [SerializeField] private Sprite _filledUpgradeLvl;

        [SerializeField, @ReadOnly] private Image[] _levels;

        [Sirenix.OdinInspector.Button]
        public void AutoInit()
        {
            _levels = GetComponentsInChildren<Image>();
        }

        public override void UpdateProgress(UpgradeData data)
        {
            for (int i = 0; i < data.CurrentLvl; i++)
            {
                _levels[i].sprite = _filledUpgradeLvl;
            }
        }
    }
}