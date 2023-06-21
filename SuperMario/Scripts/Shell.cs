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
        //이동하는 코드
        transform.position += Vector3.right * m_nDir * m_fSpeed * Time.fixedDeltaTime;
    }
   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //땅과 아이템과 충돌 시 충돌 처리를 건너뛴다.
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Item"))
            return;

        //이 오브젝트가 움직이고 있을때에는 마리오와 몬스터 모두에게 피해를 입힌다.
        if (m_bIsMoving == true)
        {            
            //마리오와 충돌했을 때의 처리
            if (collision.gameObject.CompareTag("Player"))
            {
                //마리오가 무적일 때는 이 오브젝트가 파괴된다.
                if (collision.gameObject.GetComponent<Mario>().m_bIsImmune == true)
                {
                    Destroy(gameObject);
                    return;
                }
                //아니면 마리오가 피해를 입는다.
                else
                    collision.gameObject.GetComponent<Mario>().m_nState -= 1;
            }
            //몬스터와 충돌했을 때의 처리
            else if (collision.gameObject.CompareTag("Monster"))
            {
                //점수를 증가시키고, 몬스터에게 피해를 입힌다.
                GameInformation.m_nScore += 100;
                Destroy(collision.gameObject);
            }
            //다른 지형과 충돌했을 때의 처리
            else
            {
                HitType hitType = DecideHitType(collision);

                //다른 지형과 왼쪽 또는 오른쪽으로 부딪혔다면 방향을 바꾼다.
                if(hitType == HitType.Left || hitType == HitType.Right) 
                {
                    m_nDir *= -1;
                }
            }
        }

        //마리오가 가만히 멈춰있는 이 등껍데기와 충돌했을 때의 처리
        if (collision.gameObject.CompareTag("Player") && m_bIsMoving == false)
        {
            //이 오브젝트의 x좌표에서 마리오의 x좌표를 뺀다.
            float a = transform.position.x - collision.transform.position.x;

            //그 값의 부호에 따라 초기 방향이 결정된다.
            if (a > 0)
            {
                m_nDir = 1;
            }
            else
            {
                m_nDir = -1;
            }
            //약간의 위치 조정
            transform.position += Vector3.right * a * (1.0f + 0.5f / Mathf.Abs(a));

            m_bIsMoving = true;
        }
    }
}
