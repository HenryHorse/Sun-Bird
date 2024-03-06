using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;
    [SerializeField] float speed = 15f;
    //[SerializeField] Animator animator;


    Rigidbody2D _rigidbody;
    PlayerInput _playerInput;
    InputAction _moveAction;


    Vector2 _move;

    public GameObject bullet;
    bool _shooting, _readyToShoot;
    public float shootForce;
    public bool allowInvoke = true;
    public float timeBetweenShooting, timeBetweenShots;
    public int bulletsPerTap;
    //public AudioSource shootSound;

    int _bulletsShot;
    private Vector3 mousePos;

    public bool ultimate = false;
    public bool allowUlt = true;
    public float ultimateCD = 30f;
    float ultimateTime = 10f;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _readyToShoot = true;
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Update()
    {
        //if (_rigidbody.velocity.x != 0f)
        //{
        //    animator.SetBool("move", true);
        //}
        //else
        //{
        //    animator.SetBool("move", false);
        //}
        MyInput();
        if (Keyboard.current.spaceKey.wasPressedThisFrame && allowUlt)
        {
            allowUlt = false;
            ultimate = true;
            Invoke("ResetUltimate", ultimateTime);
        }

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _move = _moveAction.ReadValue<Vector2>();
        _rigidbody.velocity = new Vector2(_move.x * speed, _move.y * speed);
        if (_rigidbody.velocity.x < 0f)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }
        else if (_rigidbody.velocity.x > 0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }


    void MyInput()
    {
        _shooting = Mouse.current.leftButton.wasPressedThisFrame;
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector3 direction = mousePos - transform.position;
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, rotZ);

        // Shooting
        if (_readyToShoot && _shooting)
        {
            _bulletsShot = 0;
            _readyToShoot = false;
            StartCoroutine(Shoot(direction, rotation));
        }
    }


    IEnumerator Shoot(Vector3 direction, Quaternion rotation)
    {

        while(_bulletsShot < bulletsPerTap)
        {
            GameObject currentBullet = Instantiate(bullet, transform.position, rotation);

            currentBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * shootForce;

            _bulletsShot++;
            //shootSound.PlayOneShot(shootSound.clip);

            if (allowInvoke)
            {
                Invoke("ResetShot", timeBetweenShooting);
                allowInvoke = false;
            }
            yield return new WaitForSeconds(timeBetweenShots);
        }


    }

    void ResetShot()
    {
        // Allow shooting and invoking again
        _readyToShoot = true;
        allowInvoke = true;
    }

    void ResetUltimate()
    {
        ultimate = false;
        Invoke("UltCoolDown", ultimateCD);
    }

    void UltCoolDown()
    {
        allowUlt = true;
    }
}
