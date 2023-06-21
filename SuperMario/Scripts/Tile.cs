using UnityEngine;

//이 사각형의 어느 면이 부딪혔는지 나타내는 열거형 형식
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

    //어느 면이 부딪혔는지 판정하는 함수
    protected virtual HitType DecideHitType(Collision2D col)
    {
        //이 충돌체의 left 좌표 - 다른 충돌체의 right 좌표
        float left = Mathf.Abs((col.transform.position.x + col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x) - (m_BoxCollider.transform.position.x - m_BoxCollider.bounds.extents.x));
        //이 충돌체의 right 좌표 - 다른 충돌체의 left 좌표
        float right = Mathf.Abs((col.transform.position.x - col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.x) - (m_BoxCollider.transform.position.x + m_BoxCollider.bounds.extents.x));
        //이 충돌체의 top 좌표 - 다른 충돌체의 bottom 좌표
        float top = Mathf.Abs((col.transform.position.y - col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y) - (m_BoxCollider.transform.position.y + m_BoxCollider.bounds.extents.y));
        //이 충돌체의 bottom 좌표 - 다른 충돌체의 top 좌표
        float bottom = Mathf.Abs((col.transform.position.y + col.gameObject.GetComponent<BoxCollider2D>().bounds.extents.y) - (m_BoxCollider.transform.position.y - m_BoxCollider.bounds.extents.y));

        float[] a = { left, right, top, bottom };

        //네 가지 값 중 최솟값과 인덱스를 구한다.
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
