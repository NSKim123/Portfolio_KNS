using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_ResultPanel; //스테이지 클리어 시 활성화되어 결과를 알려주는 판넬
    [SerializeField] private TMP_Text m_TMPResultHeart; //클리어 시 남은 목숨을 보여주는 TMP Text
    [SerializeField] private TMP_Text m_TMPResultScore; //클리어 시 쌓은 점수를 보여주는 TMP Text
    [SerializeField] private TMP_Text m_TMPResultTime; //클리어 시 남은 시간을 보여주는 TMP Text
    [SerializeField] private TMP_Text m_TMPTotalScore; //클리어 시 총 점수를 보여주는 TMP Text   
    private bool m_bIsFinished = false; 


    private void Update()
    {
        if (m_bIsFinished)
            return;

        //클리어하면 점수를 집계해 결과 판넬을 활성화한다.
        if(GameInformation.m_bIsClear)
        {        
            m_ResultPanel.SetActive(true);

            m_TMPResultHeart.text = GameInformation.m_nHeart.ToString();
            m_TMPResultScore.text = GameInformation.m_nScore.ToString();
            m_TMPResultTime.text = ((int)GameInformation.m_fTime).ToString();

            int totalscore = GameInformation.m_nScore + GameInformation.m_nHeart * 300 + ((int)GameInformation.m_fTime) * 3;
            m_TMPTotalScore.text = totalscore.ToString();

            m_bIsFinished = true;
        }
    }
    /// <summary>
    /// 아래의 세 함수는 버튼이 클릭되었을때 사용될 함수이다. 
    /// </summary>
    
    // 씬 이동을 하는 함수이다.
    public void MoveToScene(string name)
    {
        SceneManager.LoadScene(name);        
    }
    // 게임 정보를 초기화하는 함수이다.
    public void ResetInfomation()
    {
        GameInformation.m_nHeart = 3;
        GameInformation.m_nScore = 0;
        GameInformation.m_fTime = 300.0f;

        GameInformation.m_nMarioState = 1;

        GameInformation.m_bIsClear = false;

        MapLoader.MarioPos.x = 1.0f;
        MapLoader.MarioPos.y = -12.0f;
    }
    //게임을 종료하는 함수이다.
    public void QuitGame()
    {
        Application.Quit();
    }    
}
