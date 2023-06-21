using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{    
    public KeyCode m_Keycode;
    public string m_SceneName;
        
    private void OnTriggerStay2D(Collider2D collision)
    {
        //이 포탈에 아무 설정이 안 되어있다면 건너뛴다.
        if (m_SceneName == null || m_Keycode == KeyCode.None)
            return;

        //마리오가 포탈 위에서 알맞은 키를 누르면 다음에 돌아왔을때 스폰될 위치를 저장하고 다른 맵으로 이동한다.
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
