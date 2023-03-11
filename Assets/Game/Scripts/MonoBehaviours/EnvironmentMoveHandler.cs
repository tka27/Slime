using SubLib.Extensions;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    public class EnvironmentMoveHandler : MonoBehaviour
    {
        private const float MoveStep = 15f;

        private void Start()
        {
            LevelLoopSequence.ReadyMoveToNextStageEvent += Move;
        }

        private void OnDestroy()
        {
            LevelLoopSequence.ReadyMoveToNextStageEvent -= Move;
        }

        private async void Move()
        {
            var target = transform.position;
            target.z += MoveStep;
            await transform.MoveAsync(target, default, 1f);

            if (target.z < 1) return;
            target.z -= MoveStep * 2;
            transform.position = target;
        }
    }
}