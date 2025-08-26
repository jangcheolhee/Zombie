using Newtonsoft.Json.Bson;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading,
    }
    public State currentState = State.Ready;
    public State CurrentState
    {
        get { return currentState; }
        set
        {
            currentState = value;
            switch (currentState)
            {
                case State.Ready:

                    break;
                case State.Empty:
                    break;
                case State.Reloading:
                    break;
            }
        }
    }
    public GunData gunData;

    public ParticleSystem muzzleEffect;
    public ParticleSystem shellEffect;

    private AudioSource audioSource;
    private LineRenderer lineRenderer;

    public Transform firePosition;

    public int ammoRemain;
    public int magAmmo;

    private float lastFireTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;


    }
    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        lastFireTime = 0f;
        CurrentState = State.Ready;
    }
    private IEnumerator ShotEffect(Vector3 hitposition)
    {
        audioSource.PlayOneShot(gunData.shootClip);

        muzzleEffect.Play();
        shellEffect.Play();

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePosition.position);

        lineRenderer.SetPosition(1, hitposition);

        yield return new WaitForSeconds(0.2f);
        lineRenderer.enabled = false;

    }

    private void Update()
    {

        switch (currentState)
        {
            case State.Ready:
                UpdateReady();
                break;
            case State.Empty:
                UpdateEmpty();
                break;
            case State.Reloading:
                UpdateReload();
                break;
        }
    }
    private void UpdateReady()
    {

    }
    private void UpdateEmpty()
    {

    }
    private void UpdateReload()
    {

    }

    public void Fire()
    {
        if (currentState == State.Ready && Time.time > (lastFireTime + gunData.timeBetFire))
        {
            lastFireTime = Time.time;
            Shoot();
        }

    }
    public void Shoot()
    {
        Vector3 hitPosition = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(firePosition.position, firePosition.forward, out hit, gunData.fireDistance))
        {
            hitPosition = hit.point;
            var target = hit.collider.GetComponent<IDamagable>();
            if (target != null)
            {
                target.OnDamage(gunData.damage, hitPosition, hit.normal);
            }
        }
        else
        {
            hitPosition = firePosition.position + firePosition.forward * gunData.fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;
        if (magAmmo < 0)
        {
            CurrentState = State.Empty;
        }

    }
    public bool Reloading()
    {
        if (CurrentState == State.Reloading 
            || ammoRemain == 0)
            return false;
        StartCoroutine(CoReload());
        return true;
        
    }
    private IEnumerator CoReload()
    {
        CurrentState = State.Reloading;
        audioSource.PlayOneShot(gunData.reloadClip);
        yield return new WaitForSeconds(gunData.reloadTime);

        magAmmo += ammoRemain;
        if(magAmmo > gunData.magCapacity)
        {
            magAmmo = gunData.magCapacity;
            ammoRemain -= magAmmo;
        }
        else
        {
            ammoRemain = 0;
        }

        CurrentState = State.Ready;

    }
    public void AddAmmo(int amount)
    {
        ammoRemain += Mathf.Min(ammoRemain + amount, gunData.startAmmoRemain);
        
    }
}
