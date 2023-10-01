using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all objects that can be placed on the grid
public abstract class Placeable : MonoBehaviour, IHealthEntity
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] protected GameObject textIndicatorPrefab;

    float health;


    [SerializeField] protected PlaceableSO associatedSO;
    public virtual PlaceableSO GetAssociatedSO() => associatedSO;
    [SerializeField] protected int metalCost;
    [SerializeField] protected int energyCost;

    virtual protected void Awake()
    {
        health = maxHealth;
    }

    virtual protected void Start()
    {
        GameController.gameController.AddPlaceable(this);
    }

    public void Damage(float amount) {
        health -= amount;
        var indicatorGO = Instantiate(textIndicatorPrefab, transform.position, Quaternion.identity);
        var indicator = indicatorGO.GetComponent<TextIndicator>();
        indicator.Text = $"<color=\"red\">-{amount}</color>";

        if (health < 0) Die();
    }

    public abstract void Die();

    public float GetHealth() => health;

    public void Heal(float amount) => health = Mathf.Min(health + amount, maxHealth);

    public void TryPlace(Vector3Int loc, PlaceableSO placeable)
    {
        if (ValidatePlace(loc))
        {
            UpdateResources();
            Place(loc, placeable);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public abstract bool ValidatePlace(Vector3Int loc);
    public abstract void UpdateResources();
    public abstract void Place(Vector3Int loc, PlaceableSO placeable);
}
