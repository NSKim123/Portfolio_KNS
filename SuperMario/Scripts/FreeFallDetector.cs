using UnityEngine;

public class FreeFallDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //마리오와 충돌하게 된다면 낙사로 판정한다.
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Mario>().m_bIsAlive = false;
        }
        Destroy(collision.gameObject);
    }
}
