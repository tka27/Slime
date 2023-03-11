using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.MonoBehaviours
{
    [RequireComponent(typeof(Image))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _hpImage;
        [SerializeField] private Hitable _hitable;
        [SerializeField] private bool _destroyOnKill;


        private void Start()
        {
            _hitable.OnHealthChanged += UpdateBar;
            _hitable.OnDeath += OnKill;
        }

        private void OnDestroy()
        {
            _hitable.OnHealthChanged -= UpdateBar;
            _hitable.OnDeath -= OnKill;
        }

        private void OnKill()
        {
            if (!_destroyOnKill) return;
            
            _hitable.OnHealthChanged -= UpdateBar;
            _hitable.OnDeath -= OnKill;
            Destroy(gameObject);
        }

        private void UpdateBar(float amount)
        {
            _hpImage.fillAmount = amount;
            enabled = true;
        }
    }
}