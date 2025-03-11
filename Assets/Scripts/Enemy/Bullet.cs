using System.Collections;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask mask;
    public float Damage;
    public float Speed;

    private void Start()
    {
        Invoke("DestroyThisObject", 10f);
        StartCoroutine(BulletMove());
    }

    IEnumerator BulletMove()
    {
        while (true)
        {
            transform.position += Vector3.left * Time.deltaTime * Speed;
            yield return null;
        }
    }
    
    void DestroyThisObject()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<PlayerStat>() != null)
        {
            collision.gameObject.GetComponent<PlayerStat>().TakeSomethingToHp(-Damage);
            DestroyThisObject();
        }
    }
}
