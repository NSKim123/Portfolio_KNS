using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //지형에 발이 닿아야 점프 시간이 초기화되면서 다시 점프를 할 수 있다.
        gameObject.transform.parent.GetComponent<Mario>().m_bIsGrounded = true;
        gameObject.transform.parent.GetComponent<Mario>().m_bIsJumpKeyActive = true;
        gameObject.transform.parent.GetComponent<Mario>().m_fJumpTime = 0.0f;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //다른 총돌체가 이 충돌체에서 벗어나면 발에 아무런 지형이 없다고 판정한다.
        gameObject.transform.parent.GetComponent<Mario>().m_bIsGrounded = false;
    }
}
