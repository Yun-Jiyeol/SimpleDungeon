using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Setting")]
    public float Speed;
    public float FallTime;
    public float Damage;
    public Vector3 Way;
    public Vector3 RayScale;
    public float Distance;
    public LayerMask _PlayerLayer;
    public LayerMask _MonsterLayer;
    public bool isFall = true;

    public float checkRate = 0.05f;
    private float lastCheckTime;
    Coroutine fallwall;
    Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (!isFall) return;
        if (fallwall != null) return;

        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            RaycastHit hit;
            if (Physics.BoxCast(transform.position, RayScale / 2, Way.normalized, out hit, Quaternion.identity,Distance, _PlayerLayer) ||
                Physics.BoxCast(transform.position, RayScale / 2, Way.normalized, out hit, Quaternion.identity, Distance, _MonsterLayer))
            {
                fallwall = StartCoroutine(FallWall());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isFall = false;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            float x = transform.position.x - other.transform.position.x;
            float z = transform.position.z - other.transform.position.z;

            other.GetComponent<PlayerStat>().TakeSomethingToHp(-Damage, new Vector3(x, 0, z));

            if (Vector3.Distance(lastPosition, transform.position) > 0.2f) isFall = false;

        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (Vector3.Distance(lastPosition, transform.position) > 0.2f)
            {
                isFall = false;
                other.GetComponent<EnemyStat>().TakeSomethingToHp(-Damage);
            }
        }
    }

    IEnumerator FallWall()
    {
        yield return new WaitForSeconds(FallTime);
        while (isFall)
        {
            transform.position += Way.normalized * Speed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        while (Vector3.Distance(lastPosition, transform.position) >= 0.1f)
        {
            transform.position -= Way.normalized * Speed / 3 * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);
        isFall = true;
        fallwall = null;
    }
}
