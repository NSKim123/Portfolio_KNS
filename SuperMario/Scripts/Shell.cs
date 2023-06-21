using UnityEngine;

public class Shell : MonoBehaviour
{
    private float m_fSpeed = 6.0f;
    private bool m_bIsMoving = false;
    private int m_nDir = 0;
    private BoxCollider2D m_BoxCollider;

    private HitType DecideHitType(Collision2D col)
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

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
    }


    private void FixedUpdate()
    {
        //�̵��ϴ� �ڵ�
        transform.position += Vector3.right * m_nDir * m_fSpeed * Time.fixedDeltaTime;
    }
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���� �����۰� �浹 �� �浹 ó���� �ǳʶڴ�.
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Item"))
            return;

        //�� ������Ʈ�� �����̰� ���������� �������� ���� ��ο��� ���ظ� ������.
        if (m_bIsMoving == true)
        {            
            //�������� �浹���� ���� ó��
            if (collision.gameObject.CompareTag("Player"))
            {
                //�������� ������ ���� �� ������Ʈ�� �ı��ȴ�.
                if (collision.gameObject.GetComponent<Mario>().m_bIsImmune == true)
                {
                    Destroy(gameObject);
                    return;
                }
                //�ƴϸ� �������� ���ظ� �Դ´�.
                else
                    collision.gameObject.GetComponent<Mario>().m_nState -= 1;
            }
            //���Ϳ� �浹���� ���� ó��
            else if (collision.gameObject.CompareTag("Monster"))
            {
                //������ ������Ű��, ���Ϳ��� ���ظ� ������.
                GameInformation.m_nScore += 100;
                Destroy(collision.gameObject);
            }
            //�ٸ� ������ �浹���� ���� ó��
            else
            {
                HitType hitType = DecideHitType(collision);

                //�ٸ� ������ ���� �Ǵ� ���������� �ε����ٸ� ������ �ٲ۴�.
                if(hitType == HitType.Left || hitType == HitType.Right) 
                {
                    m_nDir *= -1;
                }
            }
        }

        //�������� ������ �����ִ� �� ������ �浹���� ���� ó��
        if (collision.gameObject.CompareTag("Player") && m_bIsMoving == false)
        {
            //�� ������Ʈ�� x��ǥ���� �������� x��ǥ�� ����.
            float a = transform.position.x - collision.transform.position.x;

            //�� ���� ��ȣ�� ���� �ʱ� ������ �����ȴ�.
            if (a > 0)
            {
                m_nDir = 1;
            }
            else
            {
                m_nDir = -1;
            }
            //�ణ�� ��ġ ����
            transform.position += Vector3.right * a * (1.0f + 0.5f / Mathf.Abs(a));

            m_bIsMoving = true;
        }
    }
}
