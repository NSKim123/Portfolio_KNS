using UnityEngine;

public class Coin : Tile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�������� �浹������ ���� ����
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameInformation.m_nScore += 200;
        }
    }
}
