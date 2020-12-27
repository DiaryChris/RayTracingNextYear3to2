using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum PlayerState
{
    Walking,
    Pushing,
    Falling
}

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerState pState;
    public GameObject PushCol;
    public PlayableDirector lightOn;
     

    //移动变量
    public float horizontal;
    public float vertical;
    Vector3 m_Movement;
     public  bool canMove = true;

    [Header("旋转速度")]
    public float turnSpeed = 1f;
    [Header("移动速度")]
    public float moveSpeed = 1f;
    //旋转四元数
    Quaternion m_Rotation = Quaternion.identity;
    
    //动画
    public  Animator m_Animator;

    Rigidbody m_Rigidbody;
    void Start()
    {
      
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckState();

        bool isPushing = Input.GetKey(KeyCode.F);
        m_Animator.SetBool("IsPushing", isPushing);
    }     

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //变量传入向量
        m_Movement.Set(horizontal, 0f, vertical);
        //向量单位化,防止对角移动时更快
        m_Movement.Normalize();



        //判断是否有输入
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);



        //动画
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

      
   


        //旋转向量
        Vector3 desiredForward =
            //                      正前方          输入方向          旋转速度            大小变化
            Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);

        //转换为四元数
        m_Rotation = Quaternion.LookRotation(desiredForward);

        CheckKnok();
        if (canMove)
        {
            Move();
        }
       
    }



    void Move()
    {
        //移动                                                    //根运动导致的距离
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveSpeed * m_Movement/* * m_Animator.deltaPosition.magnitude*/);
        //旋转
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    /// <summary>
    /// 检测敲地板
    /// </summary>
    void CheckKnok()
    {

        //开运动
        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.knokModified")
            && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            Debug.Log("Over");
            canMove = true;
        }

        ///开动画，锁运动
        if (canMove)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Animator.Play("knokModified", 0, 0);
                canMove = false;

                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
                {
                    lightOn.Play();
                }
            }

          

        }
    }

    void CheckState()
    {
        if (Input.GetKey(KeyCode.F))
        {
            pState = PlayerState.Pushing;
            PushCol.SetActive(true);
        }
        else if (!Mathf.Approximately(horizontal, 0f) || !Mathf.Approximately(vertical, 0f))
        {
            pState = PlayerState.Walking;
            PushCol.SetActive(false);
        }
       
    }
}
