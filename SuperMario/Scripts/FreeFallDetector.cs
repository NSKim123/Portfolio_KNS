using UnityEngine;

public class FreeFallDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�������� �浹�ϰ� �ȴٸ� ����� �����Ѵ�.
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Mario>().m_bIsAlive = false;
        }
        Destroy(collision.gameObject);
    }
}
