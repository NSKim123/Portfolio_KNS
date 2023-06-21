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
        //�̵��ϴ� �ڵ�
        transform.position += Vector3.right * 2.0f * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�������� �浹���� ���� ó��
        if(collision.gameObject.CompareTag("Player"))
        {
            //���������� ���� ������ �ο��Ѵ�.
            collision.gameObject.GetComponent<Mario>().m_bIsImmune = true;
            Destroy(gameObject);
        }
        //������ ���� ������ ������ �Ѵ�.
        else if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Block"))
        {
            m_Rigidbody.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
        }
    }
}
