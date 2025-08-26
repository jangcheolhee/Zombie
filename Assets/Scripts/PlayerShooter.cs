using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public static readonly int IdReload = Animator.StringToHash("Reload");
    public Gun gun;
    private Rigidbody gunRb;
    private Collider gunCollider;
    private Vector3 gunInitPosition;
    private Quaternion gunInitRotation;


    private PlayerInput input;
    private Animator animator;

    public Transform gunPivot;
    public Transform leftHandleMount;
    public Transform rightHandleMount;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        gunRb= gun.GetComponent<Rigidbody>();
        gunCollider = gun.GetComponent<Collider>();
        gunInitPosition = gun.transform.localPosition;
        gunInitRotation = gun.transform.localRotation;
    }
    private void OnEnable()
    {
        gunRb.isKinematic = true;
        gunCollider.enabled = false;
        gun.transform.localRotation = gunInitRotation;
        gun.transform.localPosition = gunInitPosition;

    }
    private void OnDisable()
    {
        gunRb.linearVelocity = Vector3.zero;
        gunRb.angularVelocity = Vector3.zero;
        gunRb.isKinematic = false;
        gunCollider.enabled = true;
    }
    private void Update()
    {
        if (input.Fire)
        {
            gun.Fire();
        }
        else if (input.Reload)
        {
            if (gun.Reloading())
            {
                animator.SetTrigger(IdReload);
            }
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandleMount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandleMount.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandleMount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandleMount.rotation);

    }
}
