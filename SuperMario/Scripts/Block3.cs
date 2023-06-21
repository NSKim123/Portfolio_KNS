using UnityEngine;

public class Block3 : Tile
{
    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�������� �Ӹ��� �� ��Ͽ� �ε����� �� ó���ϴ� �ڵ�
        if (collision.gameObject.CompareTag("Player") && base.DecideHitType(collision) == HitType.Bottom)
        {
            //���������� �Ʒ� �������� �ݻ��Ű��, �� ������ ���ϰ� �����Ѵ�.
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 velocity = rb.velocity;
            velocity.y = -rb.velocity.y;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
            collision.gameObject.GetComponent<Mario>().m_fJumpTime = 1.0f;
        }
    }
}
