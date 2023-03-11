using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private AnimationCurve _enemiesInWave;
        [SerializeField] private AnimationCurve _spawnDelayInWave;

        public int WaveNumber { get; private set; }
        public bool SpawnIsFinished { get; private set; }

        private void Start()
        {
            SpawnEnemies();
        }

        private async void SpawnEnemies()
        {
            SpawnIsFinished = false;
            var delay = _spawnDelayInWave.Evaluate(WaveNumber);
            for (int i = 0; i < _enemiesInWave.Evaluate(WaveNumber); i++)
            {
                Instantiate(_enemyPrefab, transform.position + SubLib.Utils.Vector3.DisplaceXZ(2), Quaternion.identity);

                await UniTask.Delay(TimeSpan.FromSeconds(delay));
            }

            SpawnIsFinished = true;
        }
    }
}