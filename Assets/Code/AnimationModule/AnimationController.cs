using UnityEngine;

namespace Code.AnimationModule
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

        private void Awake() => _animator = GetComponent<Animator>();

        public void SetIdle() => _animator.SetTrigger(Idle);

        public void SetMoveSpeed(float velocityMagnitude) => _animator.SetFloat(MoveSpeed,velocityMagnitude);
    }
}