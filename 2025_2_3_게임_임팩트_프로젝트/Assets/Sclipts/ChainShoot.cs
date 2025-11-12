using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainShoot : MonoBehaviour
{
    [SerializeField] float refreshRate = 0.01f;
    [SerializeField][Range(1, 10)] int maximunEnemiesInChain;
    [SerializeField] float delayBetweenEachChain = 0.3f;
    [SerializeField] Transform playerFirePoint;
    [SerializeField] EnemyDetector playerEnemyDetecteor;
    [SerializeField] GameObject lineRendererPrefab;

    bool shooting;
    bool shot;
    float counter = 1;
    GameObject currentClosestEnemy;
    List<GameObject> spawnedLineRenderers = new List<GameObject>();
    List<GameObject> enemisInChain = new List<GameObject>();
    List<GameObject> activeEffect = new List<GameObject>();

    void stopShooting()
    {
        shooting = false;
        shot = false;
        counter = 1;

        for (int i = 0; i < spawnedLineRenderers.Count; i++)
        {
            Destroy(spawnedLineRenderers[i]);
        }

        spawnedLineRenderers.Clear();
        enemisInChain.Clear();

        for (int i = 0;i < activeEffect.Count;i++)
        {
            Destroy(activeEffect[i]);
        }

        activeEffect.Clear();
    }

    IEnumerator UpdateLineRenderer(GameObject lineR, Transform startPos, Transform endPos, bool getClosestEnemyToPlayer = false)
    {
        if (shooting && shot && lineR != null)
        {
            lineR.GetComponent<LineRendererController>().SetPosition(startPos, endPos);

            yield return new WaitForSeconds(refreshRate);

            if (getClosestEnemyToPlayer)
            {
                StartCoroutine(UpdateLineRenderer(lineR, startPos, playerEnemyDetecteor.GetClosestEnemy().transform, true));

                if (currentClosestEnemy != playerEnemyDetecteor.GetClosestEnemy())
                {
                    stopShooting();
                    //StartShooting();
                }
            }
            else
            {
                StartCoroutine(UpdateLineRenderer(lineR, startPos, endPos));
            }

        }
    }

    void NewLineRenderer(Transform startPos, Transform endPos, bool getClosesetEnemyToPlayer = false)
    {
        GameObject lineR = Instantiate(lineRendererPrefab);
            spawnedLineRenderers.Add(lineR);
        StartCoroutine(UpdateLineRenderer(lineR, startPos, endPos, getClosesetEnemyToPlayer));
    }

    IEnumerator ChainReaction(GameObject closestEnemey)
    {
        yield return new WaitForSeconds(delayBetweenEachChain);

        if (counter == maximunEnemiesInChain)
        { 
            yield return null; 
        }
        else
        {
            if(shooting)
            { 
                counter++;
                enemisInChain.Add(closestEnemey);

                if(!enemisInChain.Contains(closestEnemey.GetComponent<EnemyDetector>().GetClosestEnemy()))
                {
                    NewLineRenderer(closestEnemey.transform, closestEnemey.GetComponent<EnemyDetector>().GetClosestEnemy().transform);
                    StartCoroutine(ChainReaction(closestEnemey.GetComponent<EnemyDetector>().GetClosestEnemy()));
                }
            }
        }
    }
}
