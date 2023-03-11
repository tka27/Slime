using SubLib.Async;
using Unity.Collections;
using UnityEngine;

namespace Game.Scripts.Data
{
    [RequireComponent(typeof(AsyncCancellation), typeof(LevelData))]
    [DefaultExecutionOrder(0)]
    public class LevelInitializer : MonoBehaviour, IAutoInit
    {
        [SerializeField, ReadOnly] private LevelData _levelData;
        [SerializeField, ReadOnly] private StaticData _staticData;

        public void AutoInit()
        {
            _levelData = GetComponent<LevelData>();
            _staticData = SubLib.Utils.Editor.GetAllInstances<StaticData>()[0];
        }

        private void Awake()
        {
            LevelData.Instance = _levelData;
            StaticData.Instance = _staticData;
        }
    }
}