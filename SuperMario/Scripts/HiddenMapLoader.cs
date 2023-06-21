using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HiddenMapLoader : MonoBehaviour
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

    //������ ������
    [SerializeField] private GameObject m_Mario;

    //m_GameObjects �� (����, ���ӿ�����Ʈ) ���� ����ϴ� ����Լ�
    private void InitDictionary()
    {
        m_GameObjects.Add('0', m_Prefab0);
        m_GameObjects.Add('1', m_Prefab1);
        m_GameObjects.Add('2', m_Prefab2);
        m_GameObjects.Add('3', m_Prefab3);
        m_GameObjects.Add('4', m_Prefab4);
    }

    //�ؽ�Ʈ ������ �� ���� ��ȯ�ϴ� �Լ�
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

        //�� ������ ���
        int _rowCount = CountNumberOfRow(@".\Assets\Resources\HiddenMap.txt");

        //�� ������ ���        
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

        //������ ����
        GameObject mario = Instantiate(m_Mario);
        Vector3 mariopos;
        mariopos.x = 1.0f;
        mariopos.y = 1.0f;
        mariopos.z = 0.0f;
        mario.transform.position = mariopos;
    }
}
