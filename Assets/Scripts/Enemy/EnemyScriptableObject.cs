using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ScriptableObject that holds the BASE STATS for an enemy. These can then be modified at object creation time to buff up enemies
/// and to reset their stats if they died or were modified at runtime.
/// </summary>
[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    public Enemy Prefab;
    public AttackScriptableObject AttackConfiguration;

    // Enemy Stats
    public int Health = 100;

    // Movement Stats
    public EnemyState DefaultState;
    public float IdleLocationRadius = 4f;
    public float IdleMovespeedMultiplier = 0.5f;
    [Range(2, 10)]
    public int Waypoints = 4;
    public float LineOfSightRange = 6f;
    public float FieldOfView = 90f;

    // NavMeshAgent Configs
    public float AIUpdateInterval = 0.1f;

    public float Acceleration = 8;
    public float AngularSpeed = 120;
    // -1 means everything
    public int AreaMask = -1;
    public int AvoidancePriority = 50;
    public float BaseOffset = 0;
    public float Height = 2f;
    public ObstacleAvoidanceType ObstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    public float Radius = 0.5f;
    public float Speed = 3f;
    public float StoppingDistance = 0.5f;

    public EnemyScriptableObject ScaleUpForLevel(ScalingScriptableObject Scaling, int Level)
    {
        EnemyScriptableObject scaledUpEnemy = CreateInstance<EnemyScriptableObject>();

        scaledUpEnemy.name = name;
        scaledUpEnemy.Prefab = Prefab;

        scaledUpEnemy.AttackConfiguration = AttackConfiguration.ScaleUpForLevel(Scaling, Level);

        scaledUpEnemy.Health = Mathf.FloorToInt(Health * Scaling.HealthCurve.Evaluate(Level));

        scaledUpEnemy.DefaultState = DefaultState;
        scaledUpEnemy.IdleLocationRadius = IdleLocationRadius;
        scaledUpEnemy.IdleMovespeedMultiplier = IdleMovespeedMultiplier;
        scaledUpEnemy.Waypoints = Waypoints;
        scaledUpEnemy.LineOfSightRange = LineOfSightRange;
        scaledUpEnemy.FieldOfView = FieldOfView;

        scaledUpEnemy.AIUpdateInterval = AIUpdateInterval;
        scaledUpEnemy.Acceleration = Acceleration;
        scaledUpEnemy.AngularSpeed = AngularSpeed;

        scaledUpEnemy.AreaMask = AreaMask;
        scaledUpEnemy.AvoidancePriority = AvoidancePriority;

        scaledUpEnemy.BaseOffset = BaseOffset;
        scaledUpEnemy.Height = Height;
        scaledUpEnemy.ObstacleAvoidanceType = ObstacleAvoidanceType;
        scaledUpEnemy.Radius = Radius;
        scaledUpEnemy.Speed = Speed * Scaling.SpeedCurve.Evaluate(Level);
        scaledUpEnemy.StoppingDistance = StoppingDistance;

        return scaledUpEnemy;
    }

    public void SetupEnemy(Enemy enemy)
    {
        enemy.Agent.acceleration = Acceleration;
        enemy.Agent.angularSpeed = AngularSpeed;
        enemy.Agent.areaMask = AreaMask;
        enemy.Agent.avoidancePriority = AvoidancePriority;
        enemy.Agent.baseOffset = BaseOffset;
        enemy.Agent.height = Height;
        enemy.Agent.obstacleAvoidanceType = ObstacleAvoidanceType;
        enemy.Agent.radius = Radius;
        enemy.Agent.speed = Speed;
        enemy.Agent.stoppingDistance = StoppingDistance;

        enemy.Movement.UpdateRate = AIUpdateInterval;
        enemy.Movement.DefaultState = DefaultState;
        enemy.Movement.IdleMovespeedMultiplier = IdleMovespeedMultiplier;
        enemy.Movement.IdleLocationRadius = IdleLocationRadius;
        enemy.Movement.Waypoints = new Vector3[Waypoints];
        enemy.Movement.LineOfSightChecker.FieldOfView = FieldOfView;
        enemy.Movement.LineOfSightChecker.Collider.radius = LineOfSightRange;
        enemy.Movement.LineOfSightChecker.LineOfSightLayers = AttackConfiguration.LineOfSightLayers;

        enemy.Health = Health;

        AttackConfiguration.SetupEnemy(enemy);
    }
}
