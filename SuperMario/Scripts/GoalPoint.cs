using UnityEngine;

public class GoalPoint : MonoBehaviour
{
    [SerializeField] private GameObject m_ClearMario;
    private bool m_bisGameFinished = false;

    private void FixedUpdate()
    {
        //�������� ���� ������ ���������� ��� ������Ʈ�� ������ �������� �ڵ�
        if(m_bisGameFinished)
        {
            if(gameObject.transform.parent.GetChild(0).transform.position.y > gameObject.transform.parent.position.y + 0.5f)
                gameObject.transform.parent.GetChild(0).transform.position += Vector3.down * Time.fixedDeltaTime * (15.5f / 3.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�������� �� ������Ʈ�� ������ �������� �ı��ϰ� Ŭ���� �ִϸ��̼��� �ִ� �������� �����Ѵ�.
        if (collision.gameObject.CompareTag("Player"))
        {
            GameInformation.m_nMarioState = collision.gameObject.GetComponent<Mario>().m_nState;

            GameObject go = Instantiate(m_ClearMario);
            Vector3 pos;
            pos.x = transform.position.x;
            pos.y = collision.gameObject.transform.position.y;
            pos.z = 0.0f;
            go.transform.position = pos;
            go.GetComponent<ClearMario>().m_nState = GameInformation.m_nMarioState;            

            Destroy(collision.gameObject);

            m_bisGameFinished = true;
        }
    }
}
