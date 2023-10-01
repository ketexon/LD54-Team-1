using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShotParams
{
    public float Damage;
    public float Speed;
    public int PeirceCap;
    public float Range;

    public static ShotParams operator+(ShotParams left, ShotParams right) => new(){
        Damage = left.Damage + right.Damage,
        Speed = left.Speed + right.Speed,
        PeirceCap = left.PeirceCap + right.PeirceCap,
        Range = left.Range + right.Range
    };

    public static ShotParams Default => new() { Damage = 50, Speed = 5, PeirceCap = 1, Range = 10 };
    public static ShotParams Zero => new() { Damage = 0, Speed = 0, PeirceCap = 0, Range = 0 };
}

[RequireComponent(typeof(Rigidbody2D))]
public class Shot : MonoBehaviour
{
    ShotParams shotParams = new() { Damage = 10, PeirceCap = 1, Speed = 1 };
    Rigidbody2D rb;

    Vector3 startPos;

    float sqrRange;
    int peircesLeft = 0;

    Queue<Enemy> enemiesHitThisTick = new Queue<Enemy>();

    public void Initialize(ShotParams shotParams)
    {
        this.shotParams = shotParams;
        sqrRange = shotParams.Range * shotParams.Range;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPos = transform.position;
        rb.velocity = transform.rotation * Vector2.right * shotParams.Speed;
        peircesLeft = shotParams.PeirceCap;
    }

    void Update()
    {
        // kill shot with too much range
        float sqrDelta = Vector2.SqrMagnitude(transform.position - startPos);

        if(sqrDelta > sqrRange)
        {
            Destroy(gameObject);
        }

        while(enemiesHitThisTick.TryDequeue(out Enemy enemy))
        {
            enemy.Damage(shotParams.Damage);
            if (--peircesLeft <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() is Enemy enemy)
        {
            enemiesHitThisTick.Enqueue(enemy);
        }
    }
}
