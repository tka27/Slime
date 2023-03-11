using Cysharp.Threading.Tasks;
using SubLib.Extensions;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private TransitionCurves _transitionCurves;

        public async UniTask Shoot(Hitable target)
        {
            await transform.CurveMoveAsync(target.AimPoint, _transitionCurves, target.GetCancellationTokenOnDestroy());
        }
    }
}