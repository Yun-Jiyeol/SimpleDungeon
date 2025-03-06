using UnityEngine;

public class SaveZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (GameManager.Instance.savePosition != gameObject.transform.position)
            {
                GameManager.Instance.savePosition = gameObject.transform.position;
                GameManager.Instance.saveRotation = gameObject.transform.eulerAngles;
            }
        }
    }
}
