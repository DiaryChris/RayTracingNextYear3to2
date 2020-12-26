using UnityEngine;

namespace Common
{
    /// <summary>
    /// 动画事件行为
    /// </summary>
    public class AnimationEventBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 动画组件
        /// </summary>
        private Animator anim; 
        private void Start()
        {
            anim = GetComponent<Animator>();
        }
          
        /// <summary>
        /// 撤销动画播放
        /// </summary>
        public void OnCancelAnim(string animPara)
        {
            anim.SetBool(animPara, false);
        }

        public delegate void AttackHandler();

        public event AttackHandler attackHandler;

        /// <summary>
        /// 攻击时使用
        /// </summary>
        public void OnAttack()
        {
            if (attackHandler != null)
                attackHandler();
        }
    }
}