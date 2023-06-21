using UnityEngine;

public class Block1 : Tile
{
    private int m_nHP = 1; //이 블록의 내구도.
    public GameObject m_Item = null; //마리오가 머리로 이 블록을 치면 나오는 아이템. null 이면 생성되는 아이템이 없다.
    public GameObject m_BlockAfterHPisZero = null; //이 블록의 내구도가 0이 될 때 이 게임오브젝트로 변환된다. null 이면 그냥 파괴된다.

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
    }

    //m_nHP의 get, set 함수
    public int HP
    {        
        get { return m_nHP; }
        set
        {
            if (value <= 0)
            {   
                //내구도가 0이 될 때 처리하는 코드.
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
        //마리오가 머리로 이 블록에 부딪혔을 때 처리하는 코드
        if (collision.gameObject.CompareTag("Player") && base.DecideHitType(collision) == HitType.Bottom)
        {
            //마리오에게 아래 방향으로 반사시키고, 더 점프를 못하게 강제한다.
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 velocity = rb.velocity;
            velocity.y = -rb.velocity.y;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
            collision.gameObject.GetComponent<Mario>().m_fJumpTime = 1.0f;
            
            //아이템 생성 코드
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
