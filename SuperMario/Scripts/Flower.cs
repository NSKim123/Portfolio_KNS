using UnityEngine;

public class Flower : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //마리오와 충돌했을 때의 처리
        if (collision.gameObject.CompareTag("Player"))
        {
            //불마법사 마리오로 성장시킨다.
            collision.gameObject.GetComponent<Mario>().m_nState = 3;
            Destroy(gameObject);
        }
    }
}
