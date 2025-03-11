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

public class EnemyController_Change : MonoBehaviour
{
    public EnemyMove enemyMove;
    private EnemyMove BeforenemyMove = EnemyMove.Spawn;

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

    public GameObject Bullet;
    public GameObject SootPosition;
    public int reloading = 0;
    public int shootRate;

    private void Start()
    {
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

            RayCastFindPlayer();

            if (BeforenemyMove != enemyMove)
            {
                BeforenemyMove = enemyMove;
                SwichState();
            }
        }
    }
    void RayCastFindPlayer()
    {
        Ray[] ray = new Ray[5]
        {
             new Ray (RayPosition.transform.position, RayPosition.transform.TransformVector(new Vector3(1,0,1))),
             new Ray (RayPosition.transform.position, RayPosition.transform.TransformVector(new Vector3(0,0,1))),
             new Ray (RayPosition.transform.position, RayPosition.transform.TransformVector(new Vector3(-1,0,1))),
             new Ray (RayPosition.transform.position, RayPosition.transform.TransformVector(new Vector3(1,0,2))),
             new Ray (RayPosition.transform.position, RayPosition.transform.TransformVector(new Vector3(-1,0,2)))
        };

        RaycastHit hit = new RaycastHit();

        EnemyMove nowMove = EnemyMove.Spawn;

        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], out hit, GetQuestion, layerMask))
            {
                playertransform = hit.transform;
                if (hit.distance > FollowDistance)
                {
                    if (nowMove != EnemyMove.Follow || nowMove != EnemyMove.Attack)
                    {
                        nowMove = EnemyMove.Question;
                    }
                }
                else if (hit.distance > AttackDistance)
                {
                    if (nowMove != EnemyMove.Attack)
                    {
                        nowMove = EnemyMove.Follow;
                    }
                }
                else if (hit.distance <= AttackDistance)
                {
                    nowMove = EnemyMove.Attack;
                }
            }
        }

        if (enemyMove == EnemyMove.Attack || enemyMove == EnemyMove.Follow)
        {
            if(Vector3.Distance(transform.position, playertransform.position) > GetQuestion)
            {
                nowMove = EnemyMove.Question;
            }
        }

        if (nowMove != EnemyMove.Spawn)
        {
            enemyMove = nowMove;
        }
    }

    void SwichState()
    {
        switch (enemyMove)
        {
            case EnemyMove.Find:
                FindPlayer();
                break;
            case EnemyMove.Question:
                GetQuestionToPlayer();
                break;
            case EnemyMove.Follow:
                FollowPlayer();
                break;
            case EnemyMove.Attack:
                AttackPlayer();
                break;
        }
    }

    void FindPlayer()
    {
        navMeshAgent.speed = gameObject.GetComponent<EnemyStat>().WalkSpeed;
        navMeshAgent.angularSpeed = 240;

        if (nowEnemyMove != null)
        {
            StopCoroutine(nowEnemyMove);
            nowEnemyMove = null;
        }

        FindWord.SetActive(false);
        QustionWord.SetActive(false);
        nowEnemyMove = StartCoroutine(CoroutineFindPlayer());
    }

    IEnumerator CoroutineFindPlayer()
    {
        while (true)
        {
            float x = transform.position.x + Random.Range(-10f, 10f);
            float z = transform.position.z + Random.Range(-10f, 10f);
            nextDistance = new Vector3(x, transform.position.y, z);

            yield return new WaitForSeconds(4f);

            navMeshAgent.SetDestination(nextDistance);
            while (navMeshAgent.remainingDistance >= 1f)
            {
                yield return null;
            }
        }
    }

    void GetQuestionToPlayer()
    {
        if (nowEnemyMove != null)
        {
            StopCoroutine(nowEnemyMove);
            nowEnemyMove = null;
        }

        navMeshAgent.angularSpeed = 240;
        FindWord.SetActive(false);
        nowEnemyMove = StartCoroutine(CoroutineGetQuestionToPlayer());
    }
    IEnumerator CoroutineGetQuestionToPlayer()
    {
        QustionWord.SetActive(true);
        navMeshAgent.speed = 0.1f;
        nextDistance = playertransform.position;
        navMeshAgent.SetDestination(nextDistance);

        yield return new WaitForSeconds(3f);

        QustionWord.SetActive(false);
        navMeshAgent.speed = gameObject.GetComponent<EnemyStat>().WalkSpeed;
        navMeshAgent.SetDestination(nextDistance);
        while (navMeshAgent.remainingDistance >= 1f)
        {
            yield return null;
        }

        enemyMove = EnemyMove.Find;
    }

    void FollowPlayer()
    {
        if (nowEnemyMove != null)
        {
            StopCoroutine(nowEnemyMove);
            nowEnemyMove = null;
        }
        navMeshAgent.speed = gameObject.GetComponent<EnemyStat>().RunSpeed;
        navMeshAgent.angularSpeed = 240;
        FindWord.SetActive(true);
        QustionWord.SetActive(false);
        nowEnemyMove = StartCoroutine(CoroutineFollowPlayer());

        Invoke("InvokeOffFindWord", 2f);
    }

    IEnumerator CoroutineFollowPlayer()
    {
        while (true)
        {
            navMeshAgent.SetDestination(playertransform.transform.position);
            yield return null;
        }
    }

    void InvokeOffFindWord()
    {
        FindWord.SetActive(false);
    }

    void AttackPlayer()
    {
        if (nowEnemyMove != null)
        {
            StopCoroutine(nowEnemyMove);
            nowEnemyMove = null;
        }

        navMeshAgent.speed = 0.2f;
        navMeshAgent.angularSpeed = 2400;
        FindWord.SetActive(false);
        QustionWord.SetActive(false);
        reloading = 0;
        nowEnemyMove = StartCoroutine(CoroutineAttackPlayer());
    }

    IEnumerator CoroutineAttackPlayer()
    {
        while (true)
        {
            reloading++;
            if(reloading >= shootRate)
            {
                GameObject go = Instantiate(Bullet);
                go.transform.position = SootPosition.transform.position;
                go.transform.eulerAngles = SootPosition.transform.eulerAngles;
                reloading = 0;
            }

            navMeshAgent.SetDestination(playertransform.transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
