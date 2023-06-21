using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private int m_nDir = 1;
    private float m_fSpeed = 3.0f;
    private BoxCollider2D m_BoxCollider;

    //어느 면이 부딪혔는지 판정하는 함수
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

        // (left  vs  bottom) 또는 (right  vs  bottom) 값차이가 얼마 안나면 아래쪽면과 부딪혔다고 강제하는 코드.
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
        //이동하는 코드
        transform.position += Vector3.right * m_nDir * m_fSpeed * Time.fixedDeltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //땅이나 몬스터와 충돌 시 충돌 처리를 건너뛴다.
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Monster"))
            return;

        //마리오와 충돌했을 때의 처리
        if(collision.gameObject.CompareTag("Player"))
        {
            //작은 마리오일때만 큰 마리오로 성장 시킨다.
            if (collision.gameObject.GetComponent<Mario>().m_nState == 1)
            {
                collision.gameObject.GetComponent<Mario>().m_nState = 2;
            }
            Destroy(this.gameObject);
        }

        //충돌면 판정
        HitType hittype = DecideHitType(collision);

        //판정된 충돌면에 따른 처리
        switch (hittype) 
        {
            //좌우로 부딪혔을 때만 방향을 전환한다.
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
