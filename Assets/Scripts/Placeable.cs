using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Base class for all objects that can be placed on the grid
public abstract class Placeable : MonoBehaviour, IHealthEntity
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] protected GameObject textIndicatorPrefab;

    float health;

    [SerializeField] protected PlaceableSO associatedSO;
    public virtual PlaceableSO GetAssociatedSO() => associatedSO;

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
        if (health <= 0) Die();
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
        Color c = Color.red;
        sr.color = c;
        for (float i = 0f; i <= 1f; i+=.05f)
        {
            c.g = i;
            c.b = i;
            sr.color = c;
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
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

    public virtual bool ValidatePlace(Vector3Int loc)
    {
        return ResourceManager.Instance.CanAfford(associatedSO);
    }

    public virtual void UpdateResources()
    {
        ResourceManager.Instance.Buy(associatedSO);
    }

    public abstract void Place(Vector3Int loc, PlaceableSO placeable);
}
