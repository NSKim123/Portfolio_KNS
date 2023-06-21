using UnityEngine;
using UnityEngine.SceneManagement;

public class Mario : MonoBehaviour
{
    private static Mario m_Instance;
        
    private int m_nDir;
    private int m_nstate = 1; //���� ���������� ū ���������� ���� ��Ÿ���� �������
    private float m_fStarBuffTimer = 0.0f; //�� ���� ���� �ð� üũ    
    private Vector3 m_Velocity = Vector3.zero;
    private Rigidbody2D m_Rigidbody;
    private BoxCollider2D m_BoxCollider;
    private Animator m_Animator;
    public float m_fJumpTime = 0.5f;
    public bool m_bIsJumpKeyActive = true;  //���� ������ �������� ��Ÿ���� �������
    public bool m_bIsGrounded = false;   //�������� ������ ��� �ִ��� ��Ÿ���� �������
    public bool m_bIsImmune = false;     //�������� ������������ ��Ÿ���� �������
    public bool m_bIsAlive = true;       

    public static Mario Instance
    {
        get { return  m_Instance; }
        private set { m_Instance = value; }
    }

    public int m_nDirection
    {
        get { return m_nDir; }
        set
        {
            if (value > 0)
            {
                m_nDir = 1;
                transform.rotation = Quaternion.identity;                
            }
            else if (value < 0)
            {
                m_nDir = -1;
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
        }
    }
    //m_nstate ������ get, set �Լ�. 
    public int m_nState
    {
        get { return m_nstate; }
        set
        {
            //m_nstate�� ���� ���ϴ� �κ�. 0�̸� ��� �ı��ϸ�,
            //2(ū ������) �Ǵ� 3(�Ҹ����� ������)�϶� ���ظ� ������ ������ 1(���� ������)�� ������ ����������.
            if (value == 0)
            {
                m_bIsAlive = false;
                Destroy(gameObject);
            }
            else if (m_nstate > value)
                m_nstate = 1;
            else
                m_nstate = value;


            //������ m_nstate���� ���� �浹ü ������ ����, groundDetector ��ġ ������ ����
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

            Vector3 groundDetectorPos;
            groundDetectorPos.x = 0.0f;
            groundDetectorPos.y = -m_BoxCollider.size.y / 2.0f;
            groundDetectorPos.z = 0.0f;
            transform.GetChild(0).transform.localPosition = groundDetectorPos;            
        }
    }
    private void Awake()
    {        
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_BoxCollider = GetComponent<BoxCollider2D>();
        m_Animator = GetComponent<Animator>();

        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
            Destroy(this.gameObject);
    }
    private void Start()
    {
        //���� ���۵� ��, ����Ǿ��ִ� ������ ������ �����ͼ� �����Ѵ�.
        m_nState = GameInformation.m_nMarioState;
    }
    private void Update()
    {
        //�ִϸ����Ϳ� ������ �������ش�.
        bool IsMoving = Mathf.Abs(m_Velocity.x) > 0;        
        bool IsJumping = m_fJumpTime > 0;
        int behavior = 0;

        if (IsJumping)
        {
            behavior = 2;
        }        
        else if(IsMoving)
        {
            behavior = 1;
        }
        else
        {
            behavior = 0;
        }

        m_Animator.SetBool("IsGround", m_bIsGrounded);
        m_Animator.SetInteger("State", m_nstate);
        m_Animator.SetInteger("Behavior", behavior);
        
    }

    private void FixedUpdate()
    {
        //�¿� �̵� �ڵ�
        //��� ����Ű�� �����Ŀ� ���� �������� ���ӵ��� �����ǵ��� �����Ͽ���.
        float acceleration = 0.0f;
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            if(m_Velocity.x > -1.0f)
            {
                m_nDirection = -1;
                acceleration = - (1.0f/0.3f);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (m_Velocity.x < 1.0f)
            {
                m_nDirection = 1;
                acceleration = 1.0f / 0.3f;
            }
        }
        else
        {
            //�ƹ� Ű�� �� ������ ������ �ӷ��� ���� �ٵ��� �Ͽ���.
            if(m_Velocity.x != 0.0f)
            {
                acceleration = -(m_Velocity.x / Mathf.Abs(m_Velocity.x)) * (1.0f / 0.3f);
            }
        }
        m_Velocity += Vector3.right * acceleration * Time.fixedDeltaTime;
        transform.position += m_Velocity * 6.0f * Time.fixedDeltaTime;



        //���� �ڵ�
        if (Input.GetKey(KeyCode.Space))
        {
            if(m_fJumpTime < 0.5f && m_bIsJumpKeyActive)
            {
                //�������� ���� ���� ���������� �������� �������� ���� ũ�Ⱑ �۾������� ����Ǿ��ִ�.
                // f(x) = 80 * (1 - 2x)^4  ��� �Լ��� �����ߴ�.
                float jumpscale = 90.0f * Mathf.Pow(1.0f - 2.0f * m_fJumpTime, 4.0f) * Time.fixedDeltaTime;
                Vector2 jumpforce;
                jumpforce.x = 0.0f;
                jumpforce.y = jumpscale;
                m_Rigidbody.AddForce(jumpforce, ForceMode2D.Impulse);
                m_fJumpTime += Time.fixedDeltaTime;
            }            
        } 
        //����Ű�� �ôٸ� ���� ��� ������ �� �̻� ������ �� ����.
        if(Input.GetKeyUp(KeyCode.Space))
        {
            m_bIsJumpKeyActive = false;
        }

        //���� ���� �ڵ�
        if(m_bIsImmune == true)
        {
            if (m_fStarBuffTimer < 10.0f)
                m_fStarBuffTimer += Time.fixedDeltaTime;
            else
            {
                m_bIsImmune = false;
                m_fStarBuffTimer = 0.0f;
            }
        }
        //�������� �ð� ���� �ڵ�.
        GameInformation.m_fTime -= Time.fixedDeltaTime;
    }

    private void OnDestroy()
    {
        //�� �̵��� �Ҷ� �������� ���� ����
        if (m_bIsAlive)
        {
            GameInformation.m_nMarioState = m_nstate;
            return;
        }
        
        //�������� �׾��� ���� �ڵ�
        //����� �����ϰ� ��� ���� �ε����� �����Ѵ�.
        GameInformation.m_nHeart -= 1;
        string scenename;
        //����� �����ִٸ� 1 ���������� �ٽ� �̵��Ѵ�.
        if (GameInformation.m_nHeart > 0)
            scenename = "SuperMarioMap";
        //����� ��� �����Ǿ��ٸ� ���� ���� ������ �̵��Ѵ�.
        else
        {
            scenename = "StartGameScene";
            GameInformation.m_nHeart = 3;            
        }
        //����� ������ ���� ������ �ʱ�ȭ �Ѵ�.
        GameInformation.m_nScore = 0;
        GameInformation.m_fTime = 300.0f;
        GameInformation.m_nMarioState = 1;
        Vector3 NextSpawnPos;
        NextSpawnPos.x = 1.0f;
        NextSpawnPos.y = -12.0f;
        NextSpawnPos.z = 0.0f;
        MapLoader.MarioPos = NextSpawnPos;

        SceneManager.LoadScene(scenename);
    }
        
}
