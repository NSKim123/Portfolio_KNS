using UnityEngine;

public class Gumba : Tile
{
    private float m_fSpeed = 6.0f * 0.3f;  
    private int m_nDir = -1;
    private bool m_IsDead = false;
    private Animator m_Animator;
    private Rigidbody2D m_Rigidbody;    

    //m_nDir의 get, set 함수
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
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    //부모클래스의 DecideType 멤버함수를 재정의
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

    private void FixedUpdate()
    {
        //이동하는 코드
        if (m_IsDead == false)        
            transform.position += Vector3.right * m_nDir * m_fSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //땅과 아이템과 충돌 시 충돌 처리를 건너뛴다.
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Item"))
            return;

        //마리오가 무적상태일때, 이 몬스터가 피해를 입게 하는 코드.
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<Mario>().m_bIsImmune == true)
            {
                Destroy(gameObject);
                return;
            }
        }

        //충돌면 판정
        HitType hitType = DecideHitType(collision);

        //판정된 충돌면에 따른 처리
        switch(hitType) 
        {
            case HitType.Bottom:    //이 몬스터의 아랫면에 부딪혔을 때
                {
                    //마리오가 피해를 입음
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        collision.gameObject.GetComponent<Mario>().m_nState -= 1;                      
                    }
                }
                break;                      
            case HitType.Left:                
            case HitType.Right:      //이 몬스터의 왼쪽 또는 오른쪽에 부딪혔을 때
                {
                    //마리오가 부딪혔으면 마리오에게 피해를 입히고, 아니면 이 몬스터의 방향을 바꾼다.
                    if (collision.gameObject.CompareTag("Player") == false)
                    {
                        m_nDirection *= -1;
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Mario>().m_nState -= 1;                        
                    }
                }
                break;
            case HitType.Top:        //이 몬스터의 위쪽면에 부딪혔을 때
                {
                    //마리오가 이 몬스터의 머리를 밟았을 때 처리하는 코드.
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        //0.2초의 처치 애니메이션과 함께 파괴됨
                        Destroy(gameObject, 0.2f);
                        m_Animator.SetBool("IsDead", true);
                        m_BoxCollider.enabled = false;
                        Destroy(this.m_Rigidbody);
                        m_IsDead = true;

                        //마리오가 강제로 약간 점프하게 한다.
                        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
                        collision.gameObject.GetComponent<Mario>().m_fJumpTime = 0.5f;

                        //점수 증가
                        GameInformation.m_nScore += 100;
                    }
                }
                break;
        }       
        
    }
}
