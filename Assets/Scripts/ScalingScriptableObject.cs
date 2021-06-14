using UnityEngine;

[CreateAssetMenu(fileName = "Scaling Configuration", menuName = "ScriptableObject/Scaling Configuration")]
public class ScalingScriptableObject : ScriptableObject
{
    public AnimationCurve HealthCurve;
    public AnimationCurve DamageCurve;
    public AnimationCurve SpeedCurve;
    public AnimationCurve SpawnRateCurve;
    public AnimationCurve SpawnCountCurve;
}
