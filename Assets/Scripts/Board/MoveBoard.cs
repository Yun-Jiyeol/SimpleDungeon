using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveBoard : MonoBehaviour
{
    public List<Vector3> positions;
    public float speed;
    public float waittime;
    public int addNum;

    private int num = 0;
    public List<GameObject> colliderObjects;

    Vector3 vec;

    private void Awake()
    {
        NormalizeVector();
    }

    private void Start()
    {
        colliderObjects.Add(gameObject);
        StartCoroutine(GoToNextPosition());
    }

    IEnumerator GoToNextPosition()
    {
        while (true)
        {
            if (lengthbetweenPositions(positions[num]))
            {
                num += addNum;
                if (num >= positions.Count)
                {
                    num -= positions.Count;
                }
                NormalizeVector();
                yield return new WaitForSeconds(waittime);
            }

            foreach (GameObject obj in colliderObjects)
            {
                obj.transform.position += vec * speed * Time.deltaTime;
            }

            yield return null;
        }
    }

    bool lengthbetweenPositions(Vector3 nextPosition)
    {
        float x = nextPosition.x - transform.position.x;
        float y = nextPosition.y - transform.position.y;
        float z = nextPosition.z - transform.position.z;

        return (Mathf.Sqrt(x * x + y * y + z * z) < 0.1f);
    }

    void NormalizeVector()
    {
        vec = new Vector3(positions[num].x - transform.position.x, positions[num].y - transform.position.y, positions[num].z - transform.position.z);
        vec.Normalize();
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliderObjects.Add(collision.gameObject);
    }
    private void OnCollisionExit(Collision collision)
    {
        colliderObjects.Remove(collision.gameObject);
    }
}
