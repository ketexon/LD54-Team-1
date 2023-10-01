using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealthEntity
{
    [SerializeField] DropSettingsSO dropSettings;
    [SerializeField] GameObject textIndicatorPrefab;

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

        var indicatorGo = Instantiate(textIndicatorPrefab, transform.position, Quaternion.identity);
        var indicator = indicatorGo.GetComponent<TextIndicator>();
        indicator.Text = $"<color=\"red\">-{amount}</color>";

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
        SpawnDrop();
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        GameController.gameController.OnEnemyDie();
    }

    void SpawnDrop()
    {
        DropSO drop = dropSettings.GetRandomDrop();
        if (drop != null)
        {
            var indicatorGo = Instantiate(textIndicatorPrefab, transform.position, Quaternion.identity);
            var indicator = indicatorGo.GetComponent<TextIndicator>();
            indicator.Text = drop.IndicatorText(dropSettings);
            ResourceManager.Instance.AddDrop(drop);
        }
    }
}
