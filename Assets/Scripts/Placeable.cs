using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all objects that can be placed on the grid
public class Placeable : MonoBehaviour, IHealthEntity
{
    [SerializeField] float maxHealth = 100f;

    float health;

    public GridCell Cell { get; private set; } = null;

    void Awake()
    {
        health = maxHealth;
    }

    public void AttachToCell(GridCell cell)
    {
        Cell = cell;
    }

    public void Damage(float amount) {
        health -= amount;
        if (health < 0) Die();
    }

    public void Die() {
        GameController.gameController.OnPlaceableDie(this);
        Cell.OnPlaceableDie();
        Destroy(gameObject);
    }

    public float GetHealth() => health;

    public void Heal(float amount) => health = Mathf.Min(health + amount, maxHealth);
}
