using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all objects that can be placed on the grid
public abstract class Placeable : MonoBehaviour, IHealthEntity
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] GameObject textIndicatorPrefab;

    float health;

    [SerializeField] protected int metalCost;
    [SerializeField] protected int energyCost;

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
        Debug.Log(health);
        health -= amount;
        var indicatorGO = Instantiate(textIndicatorPrefab, transform.position, Quaternion.identity);
        var indicator = indicatorGO.GetComponent<TextIndicator>();
        indicator.Text = $"<color=\"red\">-{amount}</color>";
        Debug.Log(indicatorGO);

        if (health < 0) Die();
    }

    public void Die() {
        GameController.gameController.OnPlaceableDie(this);
        Cell.OnPlaceableDie();
        Destroy(gameObject);
    }

    public float GetHealth() => health;

    public void Heal(float amount) => health = Mathf.Min(health + amount, maxHealth);

    public bool TryPlace(Vector3Int loc)
    {
        if (ValidatePlace(loc))
        {
            UpdateResources();
            return true;
        }
        else
        {
            return false;
        }
    }

    public abstract bool ValidatePlace(Vector3Int loc);
    public abstract void UpdateResources();
}
