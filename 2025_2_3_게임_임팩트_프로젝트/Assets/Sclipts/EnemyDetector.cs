using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10.0f;
    [SerializeField] private LayerMask enemyLayer;

    public GameObject GetClosestEnemy()
    {
        Collider[] enemiesInRainge = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        if (enemiesInRainge.Length > 0)
        {
            GameObject bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currenetPosition = transform.position;

            foreach (Collider enemyCollider in enemiesInRainge)
            {
                if (enemyCollider.gameObject == this)
                    continue;


                Vector3 directionToTarget = enemyCollider.transform.position - currenetPosition;
                float dSqrtoTarget = directionToTarget.sqrMagnitude;

                if (dSqrtoTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrtoTarget;
                    bestTarget = enemyCollider.gameObject;
                }

            }
            return bestTarget;
        }
        else
        {
            return null;
        }
    }

    public List<GameObject> GetEnemiesInRange()
    {
        List<GameObject> enemiesList = new List<GameObject>();
        Collider[] enemisInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        foreach (Collider enemyCollider in enemisInRange)
        {
            if (enemyCollider.gameObject != this.gameObject)
            {
                enemiesList.Add(enemyCollider.gameObject);
            }
        }
        return enemiesList;
    }
}

