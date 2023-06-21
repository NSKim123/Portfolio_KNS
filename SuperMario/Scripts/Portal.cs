using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{    
    public KeyCode m_Keycode;
    public string m_SceneName;
        
    private void OnTriggerStay2D(Collider2D collision)
    {
        //�� ��Ż�� �ƹ� ������ �� �Ǿ��ִٸ� �ǳʶڴ�.
        if (m_SceneName == null || m_Keycode == KeyCode.None)
            return;

        //�������� ��Ż ������ �˸��� Ű�� ������ ������ ���ƿ����� ������ ��ġ�� �����ϰ� �ٸ� ������ �̵��Ѵ�.
        if (collision.gameObject.CompareTag("Player") && Input.GetKey(m_Keycode))
        {
            Vector3 NextSpawnPos;
            NextSpawnPos.x = 132.0f;
            NextSpawnPos.y = -10.0f;
            NextSpawnPos.z = 0.0f;
            MapLoader.MarioPos = NextSpawnPos;
            SceneManager.LoadScene(m_SceneName);             
        }        
    }
}
