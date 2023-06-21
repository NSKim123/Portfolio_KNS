using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class MapLoader : MonoBehaviour
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
    [SerializeField] private GameObject m_Prefab5;
    [SerializeField] private GameObject m_Prefab6;
    [SerializeField] private GameObject m_Prefab7;
    [SerializeField] private GameObject m_Prefab8;
    [SerializeField] private GameObject m_Prefab9;

    //아이템 프리팹
    [SerializeField] private GameObject m_ItemMushroom;
    [SerializeField] private GameObject m_ItemFlower;
    [SerializeField] private GameObject m_ItemStar;

    //마리오 프리팹
    [SerializeField] private GameObject m_Mario;

    //마리오가 스폰될 위치
    public static Vector3 MarioPos = new Vector3(1.0f, -12.0f, 0.0f);

    //m_GameObjects 에 (문자, 게임오브젝트) 쌍을 등록하는 멤버함수
    private void InitDictionary()
    {
        m_GameObjects.Add('0', m_Prefab0);
        m_GameObjects.Add('1', m_Prefab1);
        m_GameObjects.Add('2', m_Prefab2);
        m_GameObjects.Add('3', m_Prefab3);
        m_GameObjects.Add('4', m_Prefab4);
        m_GameObjects.Add('5', m_Prefab5);
        m_GameObjects.Add('6', m_Prefab6);
        m_GameObjects.Add('7', m_Prefab7);
        m_GameObjects.Add('8', m_Prefab8);
        m_GameObjects.Add('9', m_Prefab9);        
    }

    //맵에 디테일한 세팅을 하는 멤버함수 (블록의 아이템 설정, 블록의 내구도 설정 등)
    private void SettingMapDetail()
    {
        
        map[9, 11].gameObject.GetComponent<Block2>().m_Item = m_ItemMushroom;        
        map[9, 35].gameObject.GetComponentInChildren<Portal>().m_SceneName = "HiddenMap";
        map[9, 62].gameObject.GetComponent<Block2>().m_Item = m_ItemFlower;
        map[9, 80].gameObject.GetComponent<Block1>().HP = 5;
        map[9, 80].gameObject.GetComponent<Block1>().m_BlockAfterHPisZero = m_Prefab5;
        map[9, 87].gameObject.GetComponent<Block1>().m_Item = m_ItemStar;
        map[9, 87].gameObject.GetComponent<Block1>().m_BlockAfterHPisZero = m_Prefab5;
        map[5, 95].gameObject.GetComponent<Block2>().m_Item = m_ItemFlower;

    }

    //텍스트 파일의 줄 수를 반환하는 함수
    private int CountNumberOfRow(string fileName)
    {
        int count = 0;
        StreamReader sr = new StreamReader(fileName);
        
        string line = sr.ReadLine();
        while(line != null) 
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
        sr = new StreamReader(@".\Assets\Resources\SuperMarioMap.txt");

        //행 갯수를 계산
        int _rowCount = CountNumberOfRow(@".\Assets\Resources\SuperMarioMap.txt");
       
        //열 갯수를 계산
        int _colCount = 0;
        string line = sr.ReadLine();
        for (int i = 0; i<line.Length; i++)
        {
            if (line[i]== ' ')
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

        SettingMapDetail();

        //마리오 생성
        GameObject mario = Instantiate(m_Mario);
        mario.transform.position = MarioPos;
        mario.transform.parent = null;
    }
       
    private void FixedUpdate()
    {
        //맵 컬링
        for (int j = 0; j < map.GetLength(1); j++) 
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                try
                {
                    //오브젝트의 x좌표가 카메라의 영역 밖이라면 그 오브젝트를 비활성화 시킨다.
                    float x = Mathf.Abs(CameraMove.m_CameraPos.x - map[i,j].gameObject.transform.position.x);
                    bool a;

                    if (x < CameraMove.m_fWidth / 2.0f + 1.0f)
                    {
                        a = true;
                    }
                    else
                    {
                        a = false;
                    }                    
                    map[i, j].gameObject.SetActive(a);
                }
                catch
                {
                    continue;
                }             
            
            }
        }
    }

}
