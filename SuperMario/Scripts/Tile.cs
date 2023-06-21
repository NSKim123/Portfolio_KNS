using UnityEngine;

//�� �簢���� ��� ���� �ε������� ��Ÿ���� ������ ����
public enum HitType
{
    Left,
    Right,
    Top,
    Bottom
}

public class Tile : MonoBehaviour
{
    protected BoxCollider2D m_BoxCollider;

    //��� ���� �ε������� �����ϴ� �Լ�
    protected virtual HitType DecideHitType(Collision2D col)
    {
        //�� �浹ü�� left ��ǥ - �ٸ� �浹ü�� right ��ǥ
        float left = Mathf.Abs((col.transform.position.x + col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x) - (m_BoxCollider.transform.position.x - m_BoxCollider.bounds.extents.x));
        //�� �浹ü�� right ��ǥ - �ٸ� �浹ü�� left ��ǥ
        float right = Mathf.Abs((col.transform.position.x - col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x) - (m_BoxCollider.transform.position.x + m_BoxCollider.bounds.extents.x));
        //�� �浹ü�� top ��ǥ - �ٸ� �浹ü�� bottom ��ǥ
        float top = Mathf.Abs((col.transform.position.y - col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y) - (m_BoxCollider.transform.position.y + m_BoxCollider.bounds.extents.y));
        //�� �浹ü�� bottom ��ǥ - �ٸ� �浹ü�� top ��ǥ
        float bottom = Mathf.Abs((col.transform.position.y + col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y) - (m_BoxCollider.transform.position.y - m_BoxCollider.bounds.extents.y));

        float[] a = { left, right, top, bottom };

        //�� ���� �� �� �ּڰ��� �ε����� ���Ѵ�.
        int k = 0;
        float min = 100.0f;

        for (int i = 0; i < 4; i++)
        {
            if (a[i] < min)
            {
                min = a[i];
                k = i;
            }
        }

        return (HitType)k;
    }  
}
