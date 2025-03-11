using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public LayerMask mask;
    public float Damage;
    public float Speed;
    private Quaternion angle;

    private void Start()
    {
        Invoke("DestroyThisObject", 10f);
        StartCoroutine(BulletMove());
        angle = gameObject.transform.rotation;
    }

    IEnumerator BulletMove()
    {
        while (true)
        {
            transform.position += angle * Vector3.forward * Time.deltaTime * Speed;
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
