using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HiddenMapLoader : MonoBehaviour
{
    //이 타일맵의 정보를 담는 2차원 배열의 멤버변수
    private Tile[,] map;
    //텍스트 문서에서의 문자를 통해 게임오브젝트 검색을 위한 사전형 멤버변수
    private Dictionary<char, GameObject> m_GameObjects = new Dictionary<char, GameObject>();

    //타일 프리팹
    [SerializeField] private GameObject m_Prefab0;
    [SerializeField] private GameObject m_Prefab1;
    [SerializeField] private GameObject m_Prefab2;
    [SerializeField] private GameObject m_Prefab3;
    [SerializeField] private GameObject m_Prefab4;

    //마리오 프리팹
    [SerializeField] private GameObject m_Mario;

    //m_GameObjects 에 (문자, 게임오브젝트) 쌍을 등록하는 멤버함수
    private void InitDictionary()
    {
        m_GameObjects.Add('0', m_Prefab0);
        m_GameObjects.Add('1', m_Prefab1);
        m_GameObjects.Add('2', m_Prefab2);
        m_GameObjects.Add('3', m_Prefab3);
        m_GameObjects.Add('4', m_Prefab4);
    }

    //텍스트 파일의 줄 수를 반환하는 함수
    private int CountNumberOfRow(string fileName)
    {
        int count = 0;
        StreamReader sr = new StreamReader(fileName);

        string line = sr.ReadLine();
        while (line != null)
        {
            count++;
            line = sr.ReadLine();
        }
        sr.Close();

        return count;
    }

    private void Awake()
    {
        InitDictionary();

        StreamReader sr;
        sr = new StreamReader(@".\Assets\Resources\HiddenMap.txt");

        //행 갯수를 계산
        int _rowCount = CountNumberOfRow(@".\Assets\Resources\HiddenMap.txt");

        //열 갯수를 계산        
        int _colCount = 0;
        string line = sr.ReadLine();
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == ' ')
            {
                continue;
            }
            else
            {
                _colCount++;
            }
        }
        map = new Tile[_rowCount, _colCount];


        //텍스트 문서를 읽으면서 문자에 따른 알맞은 게임오브젝트를 배치하고 map 변수에 저장한다.
        int index = 0;
        while (line != null)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    continue;
                }
                else
                {
                    int j = i / 2;

                    Vector3 pos;
                    pos.x = j;
                    pos.y = -index;
                    pos.z = 0;

                    GameObject go = Instantiate(m_GameObjects[line[i]]);
                    go.transform.position = pos;  
                    map[index, j] = go.GetComponent<Tile>();
                }
            }
            index++;
            line = sr.ReadLine();
        }
        sr.Close();

        //마리오 생성
        GameObject mario = Instantiate(m_Mario);
        Vector3 mariopos;
        mariopos.x = 1.0f;
        mariopos.y = 1.0f;
        mariopos.z = 0.0f;
        mario.transform.position = mariopos;
    }
}
