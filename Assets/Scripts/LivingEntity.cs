using System;
using Unity.Services.Analytics;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float MaxHealth = 100f;
    public float Health {  get; private set; }
    public bool IsDead { get; private set; }
    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = MaxHealth;
    }
    public virtual void OnDamge(float damage, Vector3 hitPoing, Vector3 hitNormal)
    {
        Health -= damage;
        if(Health < 0 && !IsDead)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        OnDeath?.Invoke();
        IsDead = true;
    }
}
