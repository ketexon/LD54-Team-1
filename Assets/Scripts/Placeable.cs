using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all objects that can be placed on the grid
public class Placeable : MonoBehaviour, IHealthEntity
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] protected GameObject textIndicatorPrefab;

    float health;


    public GridCell Cell { get; private set; } = null;

    virtual protected void Awake()
    {
        health = maxHealth;
    }

    virtual protected void Start()
    {
        GameController.gameController.AddPlaceable(this);
    }

    public void AttachToCell(GridCell cell)
    {
        Cell = cell;
    }

    public void Damage(float amount) {
        health -= amount;
        var indicatorGO = Instantiate(textIndicatorPrefab, transform.position, Quaternion.identity);
        var indicator = indicatorGO.GetComponent<TextIndicator>();
        indicator.Text = $"<color=\"red\">-{amount}</color>";

        if (health < 0) Die();
    }

    public virtual void Die() {
        GameController.gameController.OnPlaceableDie(this);
        Cell.OnPlaceableDie();
        Destroy(gameObject);
    }

    public float GetHealth() => health;

    public void Heal(float amount) => health = Mathf.Min(health + amount, maxHealth);
}
