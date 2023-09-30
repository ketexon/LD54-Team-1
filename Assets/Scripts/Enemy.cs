using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealthEntity
{
    protected static float MAX_HEALTH = 100f;
    
    private float health;

    void Start()
    {
        health = MAX_HEALTH;
    }
    
    public float GetHealth()
    {
        return health;
    }

    public void Damage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        health = Mathf.Min(health + amount, MAX_HEALTH);
    }

    public void Die()
    {
        GameController.gameController.OnEnemyDie();
        Destroy(this.gameObject);
    }
}
