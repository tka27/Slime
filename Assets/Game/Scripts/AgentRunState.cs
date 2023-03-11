using Game.Scripts.MonoBehaviours;
using SubLib.Extensions;
using UnityEngine;

namespace Game.Scripts
{
    public class AgentRunState : IState
    {
        private readonly Unit _unit;
        private static readonly int RunHash = Animator.StringToHash("Run");

        public AgentRunState(Unit unit)
        {
            _unit = unit;
        }

        public void Enter()
        {
            _unit.Animator.SetBool(RunHash, true);
            _unit.Agent.isStopped = false;
        }

        public void Update()
        {
            if (_unit.Agent.IsReached()) _unit.StateMachine.SetState(UnitState.Idle);
        }

        public void Exit()
        {
        }
    }
}