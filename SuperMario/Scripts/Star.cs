using UnityEngine;

public class Star : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        //이동하는 코드
        transform.position += Vector3.right * 2.0f * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //마리오와 충돌했을 때의 처리
        if(collision.gameObject.CompareTag("Player"))
        {
            //마리오에게 무적 버프를 부여한다.
            collision.gameObject.GetComponent<Mario>().m_bIsImmune = true;
            Destroy(gameObject);
        }
        //지형에 닿을 때마다 점프를 한다.
        else if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Block"))
        {
            m_Rigidbody.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
        }
    }
}
