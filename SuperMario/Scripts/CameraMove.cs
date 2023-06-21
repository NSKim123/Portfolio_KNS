using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float m_TargetPosX;
    public static float m_fWidth;  //�� ī�޶��� �ʺ��� ��� ��������̴�.
    public static Vector3 m_CameraPos;  //�� ī�޶��� ��ġ�� ��� ��������̴�. 
    
    private void Awake()
    {
        //16:10 ������ ī�޶�� ���α��̴� ���� ������ 1.6���̴�.
        m_fWidth = this.gameObject.GetComponent<Camera>().orthographicSize * 2.0f * 1.6f;       
    }   
    private void FixedUpdate()
    {
        if (Mario.Instance == null)
            return;

        //������ ��ġ�� ���� ī�޶��� ��ġ�� �����Ѵ�.
        m_TargetPosX = Mario.Instance.gameObject.transform.position.x;
        bool leftBound = m_TargetPosX < (m_fWidth / 2.0f) - 3.0f;
        bool rightBound = m_TargetPosX > 178.0f - ((m_fWidth / 2.0f) + 3.0f);

        float x;
        if (leftBound)
        {
            x = 0.0f + m_fWidth / 2.0f;
        }
        else if (rightBound)
        {
            x = 178.0f - m_fWidth / 2.0f;
        }
        else
        {
            x = m_TargetPosX + 3.0f;
        }

        Vector3 pos = transform.position;
        pos.x = x;
        transform.position = pos;

        m_CameraPos = pos;
    }
}
