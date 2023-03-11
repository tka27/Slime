using Game.Upgrades.Scripts.ScriptableObjects;
using UnityEngine;

namespace Game.Upgrades.Scripts
{
    public abstract class UpgradeProgress : MonoBehaviour
    {
        public abstract void UpdateProgress(UpgradeData data);
    }
}