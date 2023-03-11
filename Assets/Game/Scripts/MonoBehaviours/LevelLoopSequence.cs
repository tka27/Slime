using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    public class LevelLoopSequence : MonoBehaviour
    {
        public static event Action ReadyMoveToNextStageEvent;

        private void Start()
        {
            Enemy.OnEnemyKilled += TryMoveNextStage;
        }

        private void OnDestroy()
        {
            Enemy.OnEnemyKilled -= TryMoveNextStage;
        }

        private async void TryMoveNextStage()
        {
            if (!EnemySpawner.Instance.SpawnIsFinished || Enemy.EnemiesCount > 0) return;
            await UniTask.Delay(4000);

            ReadyMoveToNextStageEvent?.Invoke();
        }
    }
}