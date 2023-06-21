using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //������ ���� ��ƾ� ���� �ð��� �ʱ�ȭ�Ǹ鼭 �ٽ� ������ �� �� �ִ�.
        gameObject.transform.parent.GetComponent<Mario>().m_bIsGrounded = true;
        gameObject.transform.parent.GetComponent<Mario>().m_bIsJumpKeyActive = true;
        gameObject.transform.parent.GetComponent<Mario>().m_fJumpTime = 0.0f;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //�ٸ� �ѵ�ü�� �� �浹ü���� ����� �߿� �ƹ��� ������ ���ٰ� �����Ѵ�.
        gameObject.transform.parent.GetComponent<Mario>().m_bIsGrounded = false;
    }
}
