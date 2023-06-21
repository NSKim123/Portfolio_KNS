using UnityEngine;
using UnityEngine.SceneManagement;

public class Mario : MonoBehaviour
{
    private static Mario m_Instance;
        
    private int m_nDir;
    private int m_nstate = 1; //작은 마리오인지 큰 마리오인지 등을 나타내는 멤버변수
    private float m_fStarBuffTimer = 0.0f; //별 버프 지속 시간 체크    
    private Vector3 m_Velocity = Vector3.zero;
    private Rigidbody2D m_Rigidbody;
    private BoxCollider2D m_BoxCollider;
    private Animator m_Animator;
    public float m_fJumpTime = 0.5f;
    public bool m_bIsJumpKeyActive = true;  //점프 조작이 가능한지 나타내는 멤버변수
    public bool m_bIsGrounded = false;   //마리오가 지형을 밟고 있는지 나타내는 멤버변수
    public bool m_bIsImmune = false;     //마리오가 무적상태인지 나타내는 멤버변수
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
    //m_nstate 변수의 get, set 함수. 
    public int m_nState
    {
        get { return m_nstate; }
        set
        {
            //m_nstate의 값을 정하는 부분. 0이면 즉시 파괴하며,
            //2(큰 마리오) 또는 3(불마법사 마리오)일때 피해를 입으면 무조건 1(작은 마리오)로 강제로 내려버린다.
            if (value == 0)
            {
                m_bIsAlive = false;
                Destroy(gameObject);
            }
            else if (m_nstate > value)
                m_nstate = 1;
            else
                m_nstate = value;


            //설정된 m_nstate값에 따라 충돌체 사이즈 조절, groundDetector 위치 조절을 해줌
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
        //씬이 시작될 때, 저장되어있던 마리오 정보를 가져와서 대입한다.
        m_nState = GameInformation.m_nMarioState;
    }
    private void Update()
    {
        //애니메이터에 정보를 전달해준다.
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
        //좌우 이동 코드
        //어느 방향키를 누르냐에 따라 마리오의 가속도를 조절되도록 설계하였다.
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
            //아무 키를 안 눌었을 때에는 속력이 점점 줄도록 하였다.
            if(m_Velocity.x != 0.0f)
            {
                acceleration = -(m_Velocity.x / Mathf.Abs(m_Velocity.x)) * (1.0f / 0.3f);
            }
        }
        m_Velocity += Vector3.right * acceleration * Time.fixedDeltaTime;
        transform.position += m_Velocity * 6.0f * Time.fixedDeltaTime;



        //점프 코드
        if (Input.GetKey(KeyCode.Space))
        {
            if(m_fJumpTime < 0.5f && m_bIsJumpKeyActive)
            {
                //오랫동안 누를 수록 마리오에게 위쪽으로 가해지는 힘의 크기가 작아지도록 설계되어있다.
                // f(x) = 80 * (1 - 2x)^4  라는 함수로 정의했다.
                float jumpscale = 90.0f * Mathf.Pow(1.0f - 2.0f * m_fJumpTime, 4.0f) * Time.fixedDeltaTime;
                Vector2 jumpforce;
                jumpforce.x = 0.0f;
                jumpforce.y = jumpscale;
                m_Rigidbody.AddForce(jumpforce, ForceMode2D.Impulse);
                m_fJumpTime += Time.fixedDeltaTime;
            }            
        } 
        //점프키를 뗐다면 땅에 닿기 전까지 더 이상 점프할 수 없다.
        if(Input.GetKeyUp(KeyCode.Space))
        {
            m_bIsJumpKeyActive = false;
        }

        //무적 버프 코드
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
        //스테이지 시간 감소 코드.
        GameInformation.m_fTime -= Time.fixedDeltaTime;
    }

    private void OnDestroy()
    {
        //맵 이동을 할때 마리오의 상태 저장
        if (m_bIsAlive)
        {
            GameInformation.m_nMarioState = m_nstate;
            return;
        }
        
        //마리오가 죽었을 때의 코드
        //목숨을 차감하고 어느 씬을 로드할지 결정한다.
        GameInformation.m_nHeart -= 1;
        string scenename;
        //목숨이 남아있다면 1 스테이지로 다시 이동한다.
        if (GameInformation.m_nHeart > 0)
            scenename = "SuperMarioMap";
        //목숨이 모두 소진되었다면 게임 시작 씬으로 이동한다.
        else
        {
            scenename = "StartGameScene";
            GameInformation.m_nHeart = 3;            
        }
        //목숨을 제외한 게임 정보를 초기화 한다.
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
