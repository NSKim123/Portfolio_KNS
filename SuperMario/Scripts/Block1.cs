using UnityEngine;

public class Block1 : Tile
{
    private int m_nHP = 1; //�� ����� ������.
    public GameObject m_Item = null; //�������� �Ӹ��� �� ����� ġ�� ������ ������. null �̸� �����Ǵ� �������� ����.
    public GameObject m_BlockAfterHPisZero = null; //�� ����� �������� 0�� �� �� �� ���ӿ�����Ʈ�� ��ȯ�ȴ�. null �̸� �׳� �ı��ȴ�.

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
    }

    //m_nHP�� get, set �Լ�
    public int HP
    {        
        get { return m_nHP; }
        set
        {
            if (value <= 0)
            {   
                //�������� 0�� �� �� ó���ϴ� �ڵ�.
                if (m_BlockAfterHPisZero != null)
                {
                    GameObject go = Instantiate(m_BlockAfterHPisZero);
                    go.transform.position = transform.position;
                    go.transform.parent = null;
                }
                Destroy(gameObject);
                
            }
            else
                m_nHP = value;
        }
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

                m_Item = null;
            }

            HP -= 1;
        }

    }
}
