using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbTower : Tower
{
    public static Vector3 TOWER_OFFSET = new Vector3(0f,0.7f,0f);
    [SerializeField] float shotInterval = 1.0f;
    [SerializeField] ShotParams shotParams = ShotParams.Default;
    [SerializeField] GameObject shotPrefab;

    [SerializeField] Sprite inactiveSprite;
    [SerializeField] Sprite activeSprite;

    private SpriteRenderer sr;

    float shotReadyTime = Mathf.NegativeInfinity;

    protected override void Start()
    {
        base.Start();
        sr = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.time > shotReadyTime)
        {
            TryShoot();
        }
    }

    void TryShoot()
    {
        Enemy closestEnemy = GetClosestEnemyInRange();
        if(closestEnemy != null)
        {
            sr.sprite = activeSprite;
            Vector3 delta = closestEnemy.transform.position - transform.position;
            float theta = Mathf.Atan2(delta.y, delta.x);
            var go = Instantiate(shotPrefab, transform.position + TOWER_OFFSET, Quaternion.Euler(0, 0, theta * 180 / Mathf.PI));
            var shot = go.GetComponent<Shot>();
            shot.Initialize(shotParams);
            AudioManager.Instance.PlaySFX(0);

            var buff = GameController.gameController.NetPlantBuff;
            shotParams += buff.DeltaShotParams;

            shotReadyTime = Time.time + shotInterval * buff.ShotIntervalMult;
        }
        else
        {
            sr.sprite = inactiveSprite;
        }
    }
}
