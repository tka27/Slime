using Game.Scripts.Attributes;
using SubLib.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.MonoBehaviours
{
    public class AttackableAgentHandler : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField, @ReadOnly] private Attackable _attackable;

        private void OnEnable()
        {
            _attackable.OnSwitchAgentState += SwitchAgentState;
        }

        private void OnDisable()
        {
            _attackable.OnSwitchAgentState -= SwitchAgentState;
        }

        private void SwitchAgentState(bool isStopped)
        {
            if (!_agent || !_agent.enabled) return;
            _agent.isStopped = isStopped;
        }

        private void OnValidate()
        {
            gameObject.TrySetComponent(ref _attackable);
        }
    }
}