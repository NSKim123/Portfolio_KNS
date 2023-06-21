using UnityEngine;

public class Flower : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�������� �浹���� ���� ó��
        if (collision.gameObject.CompareTag("Player"))
        {
            //�Ҹ����� �������� �����Ų��.
            collision.gameObject.GetComponent<Mario>().m_nState = 3;
            Destroy(gameObject);
        }
    }
}
