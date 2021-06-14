using UnityEngine;

/// <summary>
/// A class should extend MonoBehaviour and implement this interface, then implement what should happen when that object takes damage.
/// </summary>
public interface IDamageable
{
    void TakeDamage(int Damage);
    Transform GetTransform();
}
