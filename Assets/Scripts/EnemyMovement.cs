using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public struct EnemyStats
{
    public float Damage;
    public float Speed;
    public float AttackInterval;

    public static EnemyStats Default => new() { Damage = 10, Speed = 5, AttackInterval = 1 };
}

public class EnemyMovement : MonoBehaviour
{
    private static List<Vector3Int> directions = new List<Vector3Int>()
    {
        Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
    };

    [SerializeField] EnemyStats stats = EnemyStats.Default;

    private List<Vector3Int> path;
    private Vector3 to;

    Placeable target;

    float nextAttackTime = Mathf.NegativeInfinity;

    void Start()
    {
        TargetNewPlaceable();
    }

    void Update()
    {
        if (path == null) return;

        if (path.Count == 0)
        {
            AttackTarget();
        }
        else if (this.transform.position == to)
        {
            // Debug.Log($"Reached {to}");
            to = GameController.gameController.GetGrid().GetCellCenterWorld(path[path.Count - 1]);
            path.RemoveAt(path.Count - 1);
        }
        else
        {
            // Debug.Log($"Moving towards {to}");
            this.transform.position = Vector3.MoveTowards(this.transform.position, to, Time.deltaTime * stats.Speed);
        }
    }

    void TargetNewPlaceable()
    {
        path = null;
        nextAttackTime = Mathf.NegativeInfinity;
        target = GetClosestTarget(GameController.gameController.Placeables);
        if(target != null)
        {
            path = GetPath(target.gameObject);
            to = this.transform.position;
        }
    }

    void AttackTarget()
    {
        if (target == null)
        {
            TargetNewPlaceable();
            return;
        }

        if(Time.time > nextAttackTime)
        {
            target.Damage(stats.Damage);
            nextAttackTime = Time.time + stats.AttackInterval;
        }
    }

    bool IsValid(Vector3Int cell)
    {
        return (
            (-1 * GameController.X_LIM <= cell.x && cell.x <= GameController.X_LIM) && 
            (-1 * GameController.Y_LIM <= cell.y && cell.y <= GameController.Y_LIM) && 
            !GameController.gameController.HasLand(cell)
        );
    }

    List<Vector3Int> GetPath (GameObject target)
    {
        Vector3Int curPos = GameController.gameController.GetGrid().WorldToCell(this.transform.position);
        Vector3Int targetPos = GameController.gameController.GetGrid().WorldToCell(target.transform.position);

        Vector3Int cur = curPos;
        Vector3Int next = curPos;

        bool[,] seen = new bool[GameController.X_LIM * 2 + 1, GameController.Y_LIM * 2 + 1]; 
        Dictionary<Vector3Int, Vector3Int> parents = new Dictionary<Vector3Int, Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        queue.Enqueue(curPos);

        while (queue.Count != 0)
        {
            cur = queue.Dequeue();
            foreach(var dir in directions)
            {
                next = cur + dir;
                if (next == targetPos)
                {
                    List<Vector3Int> path = new List<Vector3Int>(){ next, cur };
                    while (cur != curPos)
                    {
                        path.Add(parents[cur]);
                        cur = parents[cur];
                    }
                    path.RemoveAt(path.Count - 1);
                    queue.Clear();
                    return path;
                }
                else if (IsValid(next) && !seen[next.x + GameController.X_LIM, next.y + GameController.Y_LIM])
                {
                    queue.Enqueue(next);
                    parents[next] = cur;
                    seen[next.x + GameController.X_LIM, next.y + GameController.Y_LIM] = true;
                }
            }
        }
        return null;
    }

    Placeable GetClosestTarget(IReadOnlyList<Placeable> targets)
    {
        if (targets.Count == 0) 
        {
            Debug.LogWarning($"{this.gameObject.name} has no targets to path to");
            return null;
        }

        Placeable closest = null;
        float distance = float.MaxValue;
        float newDistance;

        foreach (var target in targets)
        {
            if (target == null) continue;
            Debug.Log(target.gameObject.name);
            newDistance = Vector3.Distance(this.transform.position, target.transform.position);
            if (newDistance < distance)
            {
                distance = newDistance;
                closest = target;
            }
        }
        return closest;
    }
}
