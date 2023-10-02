using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealthEntity
{
    [SerializeField] DropSettingsSO dropSettings;
    [SerializeField] GameObject textIndicatorPrefab;

    [SerializeField] private Sprite ded;

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
        StartCoroutine(FlashRed());
    }
    
    IEnumerator FlashRed()
    {
        SpriteRenderer sr = this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        Color c = Color.red;
        sr.material.color = c;
        for (float i = 0f; i <= 1f; i+=.05f)
        {
            c.g = i;
            c.b = i;
            sr.material.color = c;
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
    }

    public void Heal(float amount)
    {
        health = Mathf.Min(health + amount, MAX_HEALTH);
    }

    public void Die()
    {
        SpawnDrop();
        GameController.gameController.OnEnemyDie();
        Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        Destroy(this.gameObject.GetComponent<EnemyMovement>());
        this.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("dead", true);
        StartCoroutine(DestroyDelayed());
    }

    IEnumerator DestroyDelayed()
    {
        yield return new WaitForSeconds(1.78f);
        Destroy(this.gameObject);
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
