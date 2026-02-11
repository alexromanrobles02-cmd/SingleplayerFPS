using System.Collections;
using System.Linq;
using UnityEngine;

enum WeaponState
{
   Idle,
   Shooting,
   Reloading
}

public class WeaponAnimationController : MonoBehaviour
{

    private float speed = 0f;
    private Animator animator;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource reloadSound;
    [SerializeField] private ParticleSystem bulletParticles;
    [SerializeField] private GameObject muzzleFlash;
    private WeaponController weaponController;

    private WeaponState state;

    private bool hasShot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        state = WeaponState.Idle;
        weaponController = GetComponentInParent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reload()
    {
        StartCoroutine(StartReloading());
        animator.Play("Reload");
        reloadSound.Play();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void Shoot()
    {
        hasShot = true;
        reloadSound.Stop();
        animator.Play("Shoot");
        shootSound.Play();
        bulletParticles.Emit(1);

        StartCoroutine(HandleMuzzleFlash());
    }


    IEnumerator HandleMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
    }

    IEnumerator StartReloading()
    {
        hasShot = false;
        yield return new WaitForSeconds(1.2f);
        if (!hasShot)
        {
            weaponController.RequestReload();
        }
    }

}
