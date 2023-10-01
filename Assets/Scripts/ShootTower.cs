using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbTower : Tower
{
    [SerializeField] float shotInterval = 1.0f;
    [SerializeField] ShotParams shotParams = ShotParams.Default;
    [SerializeField] GameObject shotPrefab;

    float shotReadyTime = Mathf.NegativeInfinity;


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
            Vector3 delta = closestEnemy.transform.position - transform.position;
            float theta = Mathf.Atan2(delta.y, delta.x);
            var go = Instantiate(shotPrefab, transform.position, Quaternion.Euler(0, 0, theta * 180 / Mathf.PI));
            var shot = go.GetComponent<Shot>();
            shot.Initialize(shotParams);

            var buff = GameController.gameController.NetPlantBuff;
            shotParams += buff.DeltaShotParams;

            shotReadyTime = Time.time + shotInterval * buff.ShotIntervalMult;
        }
    }
}
