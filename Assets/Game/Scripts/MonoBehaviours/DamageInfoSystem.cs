using Game.Scripts.Data;
using SubLib.ObjectPool;
using SubLib.UI;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    public class DamageInfoSystem : MonoBehaviour
    {
        [SerializeField] private ObjectPool<FadingText> _fadingTextPool;

        private void Start()
        {
            Hitable.OnDamageTakenWithDamage += ShowText;
        }

        private void OnDestroy()
        {
            Hitable.OnDamageTakenWithDamage -= ShowText;
        }

        private void ShowText(int damage, Hitable hitable)
        {
            var text = _fadingTextPool.Get(
                StaticData.Instance.MainCamera.WorldToScreenPoint(hitable.DamageInfoTarget.position + Vector3.up));
            text.Show(damage.ToString()).Forget();
        }
    }
}