using SubLib.ComponentInitializer;
using SubLib.Extensions;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    [ComponentDependency(typeof(Hitable))]
    public abstract class RewardOnDeath : MonoBehaviour
    {
        [SerializeField, HideInInspector] private Hitable _hitable;
        [field: SerializeField, Min(1)] protected int Reward { get; private set; }

        private void Start()
        {
            _hitable.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            _hitable.OnDeath -= OnDeath;
        }

        protected abstract void OnDeath();

        private void OnValidate()
        {
            gameObject.TrySetComponent(ref _hitable);
        }
    }
}