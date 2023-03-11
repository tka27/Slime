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
        public static EnemySpawner Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SpawnEnemies();
            LevelLoopSequence.ReadyMoveToNextStageEvent += SpawnEnemies;
        }

        private void OnDestroy()
        {
            LevelLoopSequence.ReadyMoveToNextStageEvent -= SpawnEnemies;
        }

        private async void SpawnEnemies()
        {
            WaveNumber++;
            SpawnIsFinished = false;
            await UniTask.Delay(1500);
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