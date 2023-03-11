using Game.Upgrades.Scripts.ScriptableObjects;
using UnityEngine;

namespace Game.Upgrades.Scripts
{
    [DefaultExecutionOrder(-555)]
    public class UpgradeSource : MonoBehaviour
    {
        [field: Header("Save key = GameObject.name + Data.name")]
        [field: SerializeField]
        public UpgradeData Data { get; private set; }


        private void Awake()
        {
            Data.Init(gameObject.name);
        }
    }
}