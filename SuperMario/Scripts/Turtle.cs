using UnityEngine;

public class Turtle : Tile
{
    private float m_fSpeed = 1.8f;
    private int m_nDir = -1;
    [SerializeField] private GameObject m_Shell;  //�� ������ �Ӹ��� ������ �� �����Ǵ� ����� ���ӿ�����Ʈ

    public int m_nDirection
    {
        get { return m_nDir; }
        set
        {
            if (value > 0)
            {
                m_nDir = 1;
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            else if (value < 0)
            {
                m_nDir = -1;
                transform.rotation = Quaternion.identity;
            }
        }
    }

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
    }

    //�θ�Ŭ������ DecideType ����Լ��� ������
    protected override HitType DecideHitType(Collision2D col)
    {
        float left = Mathf.Abs((col.transform.position.x + col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x) - (m_BoxCollider.transform.position.x - m_BoxCollider.bounds.extents.x));
        float right = Mathf.Abs((col.transform.position.x - col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x) - (m_BoxCollider.transform.position.x + m_BoxCollider.bounds.extents.x));
        float top = Mathf.Abs((col.transform.position.y - col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y) - (m_BoxCollider.transform.position.y + m_BoxCollider.bounds.extents.y));
        float bottom = Mathf.Abs((col.transform.position.y + col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y) - (m_BoxCollider.transform.position.y - m_BoxCollider.bounds.extents.y));

        float[] a = { left, right, top, bottom };
        
        int k = 0;
        float min = 100.0f;

        for (int i = 0; i < 4; i++)
        {
            if (a[i] < min)
            {
                min = a[i];
                k = i;
            }
        }

        // (left  vs  bottom) �Ǵ� (right  vs  bottom) �����̰� �� �ȳ��� �Ʒ��ʸ�� �ε����ٰ� �����ϴ� �ڵ�.
        if (col.gameObject.CompareTag("Block"))
        {
            float l = Mathf.Abs(a[3] - a[0]);
            float r = Mathf.Abs(a[3] - a[1]);

            if (l < 0.05f || r < 0.05f)
            {
                k = 3;
            }
        }

        return (HitType)k;
    }

    private void FixedUpdate()
    {
        //�̵��ϴ� �ڵ�
        transform.position += Vector3.right * m_nDir * m_fSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���� �����۰� �浹 �� �浹 ó���� �ǳʶڴ�.
        if (collision.gameObject.CompareTag("Ground")|| collision.gameObject.CompareTag("Item"))
            return;

        //�������� ���������϶�, �� ���Ͱ� ���ظ� �԰� �ϴ� �ڵ�.
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Mario>().m_bIsImmune == true)
            {
                Destroy(gameObject);
                return;
            }
        }

        //�浹�� ����
        HitType hitType = DecideHitType(collision);

        //������ �浹�鿡 ���� ó��
        switch (hitType)
        {
            case HitType.Bottom:   //�� ������ �Ʒ��鿡 �ε����� ��
                {
                    if (collision.gameObject.tag == "Player")
                    {
                        collision.gameObject.GetComponent<Mario>().m_nState -= 1;                       
                    }
                }
                break;            
            case HitType.Left:
            case HitType.Right:      //�� ������ ���� �Ǵ� �����ʿ� �ε����� ��
                {
                    //�������� �ε������� ���������� ���ظ� ������, �ƴϸ� �� ������ ������ �ٲ۴�.
                    if (collision.gameObject.tag != "Player")
                    {
                        m_nDirection *= -1;
                    }
                    else 
                    {
                        collision.gameObject.GetComponent<Mario>().m_nState -= 1;                        
                    }
                }
                break;
            case HitType.Top:        //�� ������ ���ʸ鿡 �ε����� ��
                {
                    //�������� �� ������ �Ӹ��� ����� �� ó���ϴ� �ڵ�.
                    if (collision.gameObject.tag == "Player")
                    {
                        //�ı��ǰ�, �������� ������ �ణ �����ϰ� �Ѵ�.
                        Destroy(gameObject);
                        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
                        collision.gameObject.GetComponent<Mario>().m_fJumpTime = 0.5f;

                        //����⸦ �� �ڸ��� ��ȯ�Ѵ�.
                        GameObject shell = Instantiate(m_Shell);
                        shell.transform.position = transform.position;
                        shell.transform.parent = null;

                        //���� ����
                        GameInformation.m_nScore += 100;
                    }
                }
                break;
        }

    }
}
