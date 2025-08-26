using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : LivingEntity
{
    private readonly int hashDie = Animator.StringToHash("Die");
    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemPickUpClip;

    private AudioSource audioSource;
    private Animator animator;

    private PlayerMovement movement;
    private PlayerShooter shooter;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        shooter = GetComponent<PlayerShooter>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        healthSlider.gameObject.SetActive(true);
        healthSlider .value = Health/MaxHealth;

        movement.enabled = true;
        shooter.enabled = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDamage(10f, Vector3.zero, Vector3.zero);
        }
    }
    
    public override void OnDamage(float damage, Vector3 hitpoint,Vector3 hitNormal)
    {
        if (IsDead)
            return;
        base.OnDamage(damage, hitpoint, hitNormal);
        healthSlider .value = Health/MaxHealth;
        audioSource.PlayOneShot(hitClip);
    }
    public  void heal(int amount)
    {
        Health = Mathf.Min(Health + amount, MaxHealth);
        healthSlider.value = Health / MaxHealth;

    }
    protected override void Die()
    {
        base.Die();
        healthSlider.gameObject .SetActive(false);
        animator.SetTrigger(hashDie);
        audioSource.PlayOneShot(deathClip);
        movement.enabled = false;
        shooter.enabled=false;
    }

}
