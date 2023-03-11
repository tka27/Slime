using Game.Scripts.Data;
using Game.Scripts.Resource;
using UnityEngine;

namespace Game.Scripts.MonoBehaviours
{
    public class ResourceOnDeath : RewardOnDeath
    {
        [SerializeField] private ResourceType _resourceType;

        protected override void OnDeath()
        {
            ResourceHandler.AddResource(_resourceType, Reward, true,
                StaticData.Instance.MainCamera.WorldToScreenPoint(transform.position));
        }
    }
}