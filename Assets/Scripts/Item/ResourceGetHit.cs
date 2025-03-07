using System.Collections;
using UnityEngine;

public class ResourceGetHit : MonoBehaviour
{
    public GameObject MovePosition;

    public int getDamage = 0;
    public int Hp;
    int fallHp;
    GameObject fallResource;

    float x;
    float z;

    private void Start()
    {
        if(GetComponent<ItemObject>() == null)
        {
            Debug.Log("아이템 애러");
            return;
        }
        Hp = GetComponent<ItemObject>().data.Hp;
        fallHp = GetComponent<ItemObject>().data.fallHp;
        fallResource = GetComponent<ItemObject>().data.fallResource;
    }

    public void OnHit(int damage)
    {
        for(int i =0; i < damage; i++)
        {
            getDamage++;
            if(getDamage % fallHp == 0)
            {
                if(Hp < getDamage) return;
                MakeResource();
            }
        }
        if (Hp <= getDamage)
        {
            MakeResource();
            Destroy(gameObject);
        }
        StartCoroutine(Shake());
    }

    void MakeResource()
    {
        GetRandomPosition();
        GameObject Res = Instantiate(fallResource);
        Res.transform.position = new Vector3(transform.localPosition.x + x * 10, transform.localPosition.y + 2, transform.localPosition.z + z * 10);
        Res.transform.eulerAngles = new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
    }

    IEnumerator Shake()
    {
        int i = 0;

        while (i <= 10)
        {
            GetRandomPosition();
            MovePosition.transform.localPosition = new Vector3(x, 0, z);
            i++;
            yield return new WaitForSeconds(0.1f);
        }

        MovePosition.transform.localPosition = new Vector3(0, 0, 0);
    }
    void GetRandomPosition()
    {
        x = Random.Range(-0.1f, 0.1f);
        z = Random.Range(-0.1f, 0.1f);
    }
}
