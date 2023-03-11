using Game.Upgrades.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Upgrades.Scripts
{
    public class FillUpgradeProgress : UpgradeProgress
    {
        [SerializeField] private Image _filler;


        public override void UpdateProgress(UpgradeData data)
        {
            _filler.fillAmount = data.CurrentLvl / data.MaxLvl;
        }
    }
}