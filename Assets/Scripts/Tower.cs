using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Placeable
{
    [SerializeField] public float Range = 5;
    [SerializeField] LayerMask enemyLayerMask;

    protected List<Enemy> GetEnemiesInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Range, enemyLayerMask.value);
        List<Enemy> res = new();
        foreach(var collider in colliders)
        {
            var enemy = collider.GetComponent<Enemy>();
            if(enemy != null)
            {
                res.Add(enemy);
            }
        }
        return res;
    }

    protected Enemy GetClosestEnemyInRange()
    {
        List<Enemy> enemies = GetEnemiesInRange();
        Enemy closestEnemy = null;
        float closestEnemySqrDistance = Mathf.Infinity;
        foreach(var enemy in enemies)
        {
            float sqrDistance = Vector2.SqrMagnitude(enemy.transform.position - transform.position);
            if (sqrDistance < closestEnemySqrDistance)
            {
                closestEnemy = enemy;
                closestEnemySqrDistance = sqrDistance;
            }
        }
        return closestEnemy;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    public override void Die()
    {
        GameController.gameController.OnPlaceableDie(this);
        GameController.gameController.RemoveBuilding(this.transform.position);
    }

    public override bool ValidatePlace(Vector3Int loc)
    {
        return base.ValidatePlace(loc) &&
               GameController.gameController.HasFertileLand(loc) &&
               !GameController.gameController.HasBuilding(loc);
    }
}
