using UnityEngine;

public class ClearMario : MonoBehaviour
{
    private int m_nstate;  //작은 마리오인지 큰 마리오인지 등을 나타내는 멤버변수
    private float m_fTimer = 0.0f;
    private BoxCollider2D m_BoxCollider;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;

    //m_nstate 변수의 get, set 함수. 
    public int m_nState
    {
        get { return m_nstate; }
        set
        {       
            m_nstate = value;

            //설정된 m_nstate값에 따라 충돌체 사이즈 조절
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
    /// 3초동안 깃발을 내리고 그 이후에는 오른쪽으로 걸어가면서 성 안으로 들어가도록 설계하였다,
    /// </summary>
 
    private void Awake()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
                
        //깃발을 내리는 처리는 등속도 운동으로 적용하기 위해 처음에 중력에 영향을 받지 않도록 하였다.
        m_Rigidbody.gravityScale = 0.0f;        

        Destroy(gameObject, 7.0f);
    }
    private void Start()
    {
        m_Animator.SetInteger("State", m_nState);
    }
    private void FixedUpdate()
    {        
        //3초 동안 깃발을 내리고, 이후 약 4초동안 성 안으로 이동한다.
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
