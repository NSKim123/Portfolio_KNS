//게임 정보를 저장하는 정적 클래스
public static class GameInformation
{
    public static int m_nScore = 0; //누적된 점수을 나타내는 변수
    public static int m_nHeart = 3; //남은 목숨을 나타내는 변수
    public static float m_fTime = 300.0f; //남은 시간을 나타내는 변수
    public static int m_nMarioState = 1; //마리오의 상태(작은 마리오, 큰마리오 등)을 저장하는 변수

    public static bool m_bIsClear = false; //스테이지가 클리어됐는지 나타내는 변수
}
