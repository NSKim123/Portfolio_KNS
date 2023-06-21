using UnityEngine;

public class Block2 : Tile
{    
    public GameObject m_Item; //�������� �Ӹ��� �� ����� ġ�� ������ ������. null �̸� �����Ǵ� �������� ����.
    [SerializeField] private GameObject m_Block3;  //�������� �Ӹ��� �� ����� ġ�� �ٷ� Block3 Ÿ�� ������� ���Ѵ�.

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

            //������ ���� �ڵ�
            if (m_Item != null)
            {
                GameObject item = Instantiate(m_Item);
                Vector3 pos = transform.position + Vector3.up;
                item.transform.position = pos;
                item.transform.parent = null;
            }

            //Block3 Ÿ�� ������� ��ȯ
            GameObject block3 = Instantiate(m_Block3);
            block3.transform.position = gameObject.transform.position;
            block3.transform.parent = null;

            Destroy(gameObject);
        }
    }
}
