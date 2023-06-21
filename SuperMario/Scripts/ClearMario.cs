using UnityEngine;

public class ClearMario : MonoBehaviour
{
    private int m_nstate;  //���� ���������� ū ���������� ���� ��Ÿ���� �������
    private float m_fTimer = 0.0f;
    private BoxCollider2D m_BoxCollider;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;

    //m_nstate ������ get, set �Լ�. 
    public int m_nState
    {
        get { return m_nstate; }
        set
        {       
            m_nstate = value;

            //������ m_nstate���� ���� �浹ü ������ ����
            Vector2 size;
            size.x = 0.75f;
            size.y = 1.05f;

            switch (m_nstate)
            {
                case 1:
                    {
                        size.y = 1.05f;
                    }
                    break;
                case 2:
                    {
                        size.y = 1.7f;
                    }
                    break;
                case 3:
                    {
                        size.y = 1.7f;
                    }
                    break;
            }
            m_BoxCollider.size = size;            
        }
    }
    /// <summary>
    /// 3�ʵ��� ����� ������ �� ���Ŀ��� ���������� �ɾ�鼭 �� ������ ������ �����Ͽ���,
    /// </summary>
 
    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
                
        //����� ������ ó���� ��ӵ� ����� �����ϱ� ���� ó���� �߷¿� ������ ���� �ʵ��� �Ͽ���.
        m_Rigidbody.gravityScale = 0.0f;        

        Destroy(gameObject, 7.0f);
    }
    private void Start()
    {
        m_Animator.SetInteger("State", m_nState);
    }
    private void FixedUpdate()
    {        
        //3�� ���� ����� ������, ���� �� 4�ʵ��� �� ������ �̵��Ѵ�.
        if (m_fTimer < 3.0f)
        {
            transform.position += Vector3.down * Time.fixedDeltaTime * (15.5f / 3.0f);            
        }        
        else if(m_fTimer > 6.9f)
        {
            GameInformation.m_bIsClear = true;
        }
        else
        {
            m_Animator.SetBool("IsMoving", true);
            m_Rigidbody.gravityScale = 1.0f;
            transform.position += 3.0f * Time.fixedDeltaTime * Vector3.right;            
        }
        m_fTimer += Time.fixedDeltaTime;
    }
}
