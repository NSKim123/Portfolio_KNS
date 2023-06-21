using UnityEngine;

public class Block2 : Tile
{    
    public GameObject m_Item; //마리오가 머리로 이 블록을 치면 나오는 아이템. null 이면 생성되는 아이템이 없다.
    [SerializeField] private GameObject m_Block3;  //마리오가 머리로 이 블록을 치면 바로 Block3 타입 블록으로 변한다.

    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
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
            }

            //Block3 타입 블록으로 변환
            GameObject block3 = Instantiate(m_Block3);
            block3.transform.position = gameObject.transform.position;
            block3.transform.parent = null;

            Destroy(gameObject);
        }
    }
}
