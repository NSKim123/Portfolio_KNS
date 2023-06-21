using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class MapLoader : MonoBehaviour
{
    //�� Ÿ�ϸ��� ������ ��� 2���� �迭�� �������
    private Tile[,] map; 
    //�ؽ�Ʈ ���������� ���ڸ� ���� ���ӿ�����Ʈ �˻��� ���� ������ �������
    private Dictionary<char, GameObject> m_GameObjects = new Dictionary<char, GameObject>(); 

    //Ÿ�� ������
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

    //������ ������
    [SerializeField] private GameObject m_ItemMushroom;
    [SerializeField] private GameObject m_ItemFlower;
    [SerializeField] private GameObject m_ItemStar;

    //������ ������
    [SerializeField] private GameObject m_Mario;

    //�������� ������ ��ġ
    public static Vector3 MarioPos = new Vector3(1.0f, -12.0f, 0.0f);

    //m_GameObjects �� (����, ���ӿ�����Ʈ) ���� ����ϴ� ����Լ�
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

    //�ʿ� �������� ������ �ϴ� ����Լ� (����� ������ ����, ����� ������ ���� ��)
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

    //�ؽ�Ʈ ������ �� ���� ��ȯ�ϴ� �Լ�
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

        //�� ������ ���
        int _rowCount = CountNumberOfRow(@".\Assets\Resources\SuperMarioMap.txt");
       
        //�� ������ ���
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


        //�ؽ�Ʈ ������ �����鼭 ���ڿ� ���� �˸��� ���ӿ�����Ʈ�� ��ġ�ϰ� map ������ �����Ѵ�.
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

        //������ ����
        GameObject mario = Instantiate(m_Mario);
        mario.transform.position = MarioPos;
        mario.transform.parent = null;
    }
       
    private void FixedUpdate()
    {
        //�� �ø�
        for (int j = 0; j < map.GetLength(1); j++) 
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                try
                {
                    //������Ʈ�� x��ǥ�� ī�޶��� ���� ���̶�� �� ������Ʈ�� ��Ȱ��ȭ ��Ų��.
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
