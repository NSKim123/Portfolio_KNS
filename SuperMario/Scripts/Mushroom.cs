using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private int m_nDir = 1;
    private float m_fSpeed = 3.0f;
    private BoxCollider2D m_BoxCollider;

    //��� ���� �ε������� �����ϴ� �Լ�
    private HitType DecideHitType(Collision2D col)
    {
        float left = Mathf.Abs((col.transform.position.x + col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x) - (m_BoxCollider.transform.position.x - m_BoxCollider.bounds.extents.x));
        float right = Mathf.Abs((col.transform.position.x - col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x) - (m_BoxCollider.transform.position.x + m_BoxCollider.bounds.extents.x ));
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
        //���̳� ���Ϳ� �浹 �� �浹 ó���� �ǳʶڴ�.
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Monster"))
            return;

        //�������� �浹���� ���� ó��
        if(collision.gameObject.CompareTag("Player"))
        {
            //���� �������϶��� ū �������� ���� ��Ų��.
            if (collision.gameObject.GetComponent<Mario>().m_nState == 1)
            {
                collision.gameObject.GetComponent<Mario>().m_nState = 2;
            }
            Destroy(this.gameObject);
        }

        //�浹�� ����
        HitType hittype = DecideHitType(collision);

        //������ �浹�鿡 ���� ó��
        switch (hittype) 
        {
            //�¿�� �ε����� ���� ������ ��ȯ�Ѵ�.
            case HitType.Left:                
            case HitType.Right:
                {
                    m_nDir *= -1;
                }
                break;
            case HitType.Top:
            case HitType.Bottom:
                break;
        }
    }
}
