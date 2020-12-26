using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    //移动变量
    public float horizontal;
    public float vertical;
    Vector3 m_Movement;


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

        Move();
    }



    void Move()
    {
        //移动                                                    //根运动导致的距离
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveSpeed * m_Movement/* * m_Animator.deltaPosition.magnitude*/);
        //旋转
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
