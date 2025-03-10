using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyMove
{
    Spawn,
    Find,
    Question,
    Follow,
    Attack
}

public class EnemyController : MonoBehaviour
{
    EnemyStat state;
    private EnemyMove enemyMove;
    private EnemyMove BeforeEnemyMove = EnemyMove.Spawn;

    public float GetQuestion;
    public float FollowDistance;
    public float AttackDistance;
    public LayerMask layerMask;
    public GameObject RayPosition; 
    public float checkRate = 0.05f;
    public GameObject QustionWord;
    public GameObject FindWord;
    private float lastCheckTime;

    private Vector3 nextDistance;
    Coroutine nowEnemyMove;
    private NavMeshAgent navMeshAgent;
    private Transform playertransform;

    private void Start()
    {
        state = GetComponent<EnemyStat>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        enemyMove = EnemyMove.Find;
        QustionWord.SetActive(false);
        FindWord.SetActive(false);
        nextDistance = transform.position;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = new Ray(RayPosition.transform.position, RayPosition.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, GetQuestion, layerMask))
            {
                playertransform = hit.transform;
                if (hit.distance > FollowDistance)
                {
                    enemyMove = EnemyMove.Question;
                }else if (hit.distance > AttackDistance)
                {
                    enemyMove = EnemyMove.Follow;
                }
                else if (hit.distance <= AttackDistance)
                {
                    enemyMove = EnemyMove.Attack;
                }
            }
        }
        switch (enemyMove)
        {
            case EnemyMove.Find:
                if(enemyMove != BeforeEnemyMove)
                {
                    BeforeEnemyMove = enemyMove;
                    FindPlayer();
                }
                break;
            case EnemyMove.Question:
                if (enemyMove != BeforeEnemyMove)
                {
                    BeforeEnemyMove = enemyMove;
                    GetQuestionToPlayer();
                }
                break;
            case EnemyMove.Follow:
                if (enemyMove != BeforeEnemyMove)
                {
                    BeforeEnemyMove = enemyMove;
                    navMeshAgent.speed = gameObject.GetComponent<EnemyStat>().RunSpeed;
                    FindWord.SetActive(true);
                    Invoke("InvokeOffFindWord", 2f);
                }
                navMeshAgent.SetDestination(playertransform.transform.position);
                break;
            case EnemyMove.Attack:
                if (enemyMove != BeforeEnemyMove)
                {
                    BeforeEnemyMove = enemyMove;
                    navMeshAgent.speed = 0.1f;
                    FindWord.SetActive(false);
                }
                navMeshAgent.SetDestination(playertransform.transform.position);
                break;
        }
    }

    void InvokeOffFindWord()
    {
        FindWord.SetActive(false);
    }

    void FindPlayer()
    {
        if (nowEnemyMove != null)
        {
            StopCoroutine(nowEnemyMove);
        }
        navMeshAgent.speed = gameObject.GetComponent<EnemyStat>().WalkSpeed;
        nowEnemyMove = StartCoroutine(CoroutineFindPlayer());
    }

    IEnumerator CoroutineFindPlayer()
    {
        while (enemyMove == EnemyMove.Find)
        {
            nextDistance = transform.position;
            float x = transform.position.x + Random.Range(-10f, 10f);
            float z = transform.position.z + Random.Range(-10f, 10f);
            nextDistance = new Vector3(x, transform.position.y, z);

            navMeshAgent.SetDestination(nextDistance);

            yield return new WaitForSeconds(10f);
        }
    }

    void GetQuestionToPlayer()
    {
        if (nowEnemyMove != null)
        {
            StopCoroutine(nowEnemyMove);
        }

        QustionWord.SetActive(true);
        navMeshAgent.speed = 0;

        nowEnemyMove = StartCoroutine(CoroutineGetQuestionToPlayer());
    }
    IEnumerator CoroutineGetQuestionToPlayer()
    {
        nextDistance = playertransform.position;
        navMeshAgent.SetDestination(nextDistance);
        yield return new WaitForSeconds(4f);

        QustionWord.SetActive(false);
        navMeshAgent.speed = gameObject.GetComponent<EnemyStat>().WalkSpeed;
        navMeshAgent.SetDestination(nextDistance);

        yield return new WaitForSeconds(5f);
        enemyMove = EnemyMove.Find;
    }

}
