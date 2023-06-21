using UnityEngine;

public class Coin : Tile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //마리오와 충돌했을때 점수 증가
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameInformation.m_nScore += 200;
        }
    }
}
