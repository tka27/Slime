using Cysharp.Threading.Tasks;
using SubLib.Extensions;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

        [field: SerializeField] public Hitable Hitable { get; private set; }
        [field: SerializeField] public Shootable Shootable;


        private void Awake()
        {
            Instance = this;
            Hitable.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            Hitable.OnDeath -= OnDeath;
        }

        private async void OnDeath()
        {
            SwitchComponents(false);
            await transform.RescaleAsync(Vector3.one, Vector3.zero, default, 0.5f);
            await UniTask.Delay(1000);
            await transform.RescaleAsync(Vector3.zero, Vector3.one, default, 0.5f);
            SwitchComponents(true);
        }

        private void SwitchComponents(bool value)
        {
            Hitable.enabled = value;
            Shootable.enabled = value;
        }
    }
}