using TMPro;
using UnityEngine;

/// <summary>
/// 게임이 진행되는 동안 화면에 나타낼 TMP Text를 관리하는 클래스이다.
/// </summary>
public class GameInfoText : MonoBehaviour
{
    [SerializeField] private TMP_Text m_ScoreText; //누적된 점수를 나타내는 TMP Text
    [SerializeField] private TMP_Text m_HeartText; //남은 목숨를 나타내는 TMP Text
    [SerializeField] private TMP_Text m_TimeText;  //남은 시간를 나타내는 TMP Text

    //누적된 점수, 남은 목숨, 남은 시간을 각각 TMP Text에 입력한다.
    private void Start()
    {        
        m_HeartText.text = GameInformation.m_nHeart.ToString();        
    }
    private void FixedUpdate()
    {
        m_ScoreText.text = GameInformation.m_nScore.ToString();
        int time = (int)GameInformation.m_fTime;
        m_TimeText.text = time.ToString();
    }
}
