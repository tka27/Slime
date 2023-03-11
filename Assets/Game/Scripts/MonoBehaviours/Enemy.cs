namespace Game.Scripts.MonoBehaviours
{
    public class Enemy : Unit
    {
        public static event System.Action OnEnemyKilled;
        public static int EnemiesCount { get; private set; }

        private void Start()
        {
            EnemiesCount++;
        }

        private void OnDisable()
        {
            if (Hitable.IsAlive) return;
            EnemiesCount--;
            OnEnemyKilled?.Invoke();
        }

        protected override void Navigate()
        {
            if (Player.Instance.Hitable.IsAlive && !Attackable.Scanner.CurrentTarget)
            {
                Agent.destination = Player.Instance.transform.position;
            }
            else
            {
                Agent.destination = Agent.transform.position;
            }
        }
    }
}