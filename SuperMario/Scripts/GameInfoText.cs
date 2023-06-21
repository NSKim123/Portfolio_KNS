using TMPro;
using UnityEngine;

/// <summary>
/// ������ ����Ǵ� ���� ȭ�鿡 ��Ÿ�� TMP Text�� �����ϴ� Ŭ�����̴�.
/// </summary>
public class GameInfoText : MonoBehaviour
{
    [SerializeField] private TMP_Text m_ScoreText; //������ ������ ��Ÿ���� TMP Text
    [SerializeField] private TMP_Text m_HeartText; //���� ����� ��Ÿ���� TMP Text
    [SerializeField] private TMP_Text m_TimeText;  //���� �ð��� ��Ÿ���� TMP Text

    //������ ����, ���� ���, ���� �ð��� ���� TMP Text�� �Է��Ѵ�.
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
