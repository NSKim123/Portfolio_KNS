using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float m_TargetPosX;
    public static float m_fWidth;  //이 카메라의 너비값을 담는 멤버변수이다.
    public static Vector3 m_CameraPos;  //이 카메라의 위치를 담는 멤버변수이다. 
    
    private void Awake()
    {
        //16:10 비율의 카메라라서 가로길이는 세로 길이의 1.6배이다.
        m_fWidth = this.gameObject.GetComponent<Camera>().orthographicSize * 2.0f * 1.6f;       
    }   
    private void FixedUpdate()
    {
        if (Mario.Instance == null)
            return;

        //마리오 위치에 따라 카메라의 위치를 결정한다.
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
