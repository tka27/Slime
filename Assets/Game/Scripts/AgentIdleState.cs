using Game.Scripts.MonoBehaviours;
using SubLib.Extensions;
using UnityEngine;

namespace Game.Scripts
{
    public readonly struct AgentIdleState : IState
    {
        private readonly Unit _unit;
        private static readonly int RunHash = Animator.StringToHash("Run");

        public AgentIdleState(Unit unit)
        {
            _unit = unit;
        }

        public void Enter()
        {
            _unit.Animator.SetBool(RunHash, false);
            _unit.Agent.isStopped = true;
        }

        public void Update()
        {
            if (!_unit.Agent.IsReached()) _unit.StateMachine.SetState(UnitState.Run);
        }

        public void Exit()
        {
        }
    }
}