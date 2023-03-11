using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Scripts.Attributes;
using Sirenix.OdinInspector;
using SubLib.Async;
using SubLib.Extensions;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Hitable : MonoBehaviour
    {
        public static event System.Action<int, Hitable> OnDamageTakenWithDamage;
        public event System.Action<float> OnHealthChanged;
        public event System.Action OnDeath;
        public event System.Action OnDamageTaken;


        [SerializeField] private Renderer _renderer;
        [SerializeField, Min(0)] private int _reflectionDamage = 0;


        [SerializeField] private float _defaultMaxHealth = 100;
        [SerializeField, @ReadOnly] private float _currentHealth;
        [SerializeField, @ReadOnly] private Collider _collider;

        [SerializeField] private bool _enableRegeneration;
        [field: SerializeField] public Transform AimPoint { get; private set; }
        [field: SerializeField] public Transform DamageInfoTarget { get; private set; }

        [SerializeField, ShowIf("_enableRegeneration")]
        private int _regenerationDelay = 2000;

        [SerializeField, Min(0.1f), ShowIf("_enableRegeneration")]
        private float _regenPerSecond = 5;


        private Timer _regenerationTimer;
        private float _maxHealth;
        private ReusableCancellationTokenSource _cts;

        private void Awake()
        {
            _cts = new(this.GetCancellationTokenOnDestroy());
            UpdateMaxHealth(1);
        }


        public int ReflectionDamage => _reflectionDamage;
        public bool IsAlive => _currentHealth > 0;

        public void UpdateMaxHealth(float multiplier)
        {
            _maxHealth = _defaultMaxHealth * multiplier;
            Heal();
        }

        private void Heal()
        {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(_currentHealth / _maxHealth);
        }


        public void TakeDamage(int damage)
        {
            if (!IsAlive) return;

            if (_enableRegeneration)
            {
                _regenerationTimer?.Destroy();
                _regenerationTimer = new(_regenerationDelay, Regen, false);
            }

            _cts.Cancel();
            _currentHealth -= damage;

            OnHealthChanged?.Invoke(_currentHealth / _maxHealth);
            OnDamageTaken?.Invoke();
            OnDamageTakenWithDamage?.Invoke(damage, this);
            _ = _renderer.BlinkAsync(default, 0.2f);

            if (IsAlive) return;
            enabled = false;
            OnDeath?.Invoke();
            _regenerationTimer?.Destroy();
        }

        private void OnEnable()
        {
            Heal();
            _collider.enabled = true;
        }

        private void OnDisable()
        {
            _collider.enabled = false;
        }

        private async void Regen()
        {
            var token = _cts.Create();
            while (_currentHealth < _maxHealth)
            {
                _currentHealth += Time.deltaTime * _regenPerSecond;
                OnHealthChanged?.Invoke(_currentHealth / _maxHealth);
                await Task.Yield();
                if (!token.IsCancellationRequested) continue;
                return;
            }

            _currentHealth = _maxHealth;
        }

        private void OnValidate()
        {
            if (!_renderer)
            {
                var renderers = GetComponentsInChildren<Renderer>();
                if (renderers.Length > 0) _renderer = GetComponentsInChildren<Renderer>()[0];
            }

            if (!_collider) _collider = GetComponent<Collider>();
        }
    }
}