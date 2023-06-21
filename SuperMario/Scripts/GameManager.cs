using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject m_ResultPanel; //�������� Ŭ���� �� Ȱ��ȭ�Ǿ� ����� �˷��ִ� �ǳ�
    [SerializeField] private TMP_Text m_TMPResultHeart; //Ŭ���� �� ���� ����� �����ִ� TMP Text
    [SerializeField] private TMP_Text m_TMPResultScore; //Ŭ���� �� ���� ������ �����ִ� TMP Text
    [SerializeField] private TMP_Text m_TMPResultTime; //Ŭ���� �� ���� �ð��� �����ִ� TMP Text
    [SerializeField] private TMP_Text m_TMPTotalScore; //Ŭ���� �� �� ������ �����ִ� TMP Text   
    private bool m_bIsFinished = false; 


    private void Update()
    {
        if (m_bIsFinished)
            return;

        //Ŭ�����ϸ� ������ ������ ��� �ǳ��� Ȱ��ȭ�Ѵ�.
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
    /// �Ʒ��� �� �Լ��� ��ư�� Ŭ���Ǿ����� ���� �Լ��̴�. 
    /// </summary>
    
    // �� �̵��� �ϴ� �Լ��̴�.
    public void MoveToScene(string name)
    {
        SceneManager.LoadScene(name);        
    }
    // ���� ������ �ʱ�ȭ�ϴ� �Լ��̴�.
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
    //������ �����ϴ� �Լ��̴�.
    public void QuitGame()
    {
        Application.Quit();
    }    
}
