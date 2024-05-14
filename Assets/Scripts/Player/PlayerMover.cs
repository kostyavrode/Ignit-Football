using UnityEngine;
using System;
using DG.Tweening;
using UniRx;

public class PlayerMover : MonoBehaviour
{
    public static Action onDeath;
    [SerializeField] public Transform[] transforms;
    [SerializeField] private float transformSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Joystick joystick;

    private Animator animator;
    private Rigidbody rigidbody;
    private SphereCollider sphereCollider;
    private CapsuleCollider capsuleCollider;
    private int transformPlayer=1;
    private int lastTransformPlayer;
    private bool isTransforming;
    private bool canJump=true;
    private bool isCanMove;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        Move();
    }
    private void Update()
    {
        MoveLeft();
        MoveRight();
        Jump();
        Roll();
        CheckIsCanMove();
    }
    private void CheckIsCanMove()
    {
        if (joystick.Horizontal==0f && !isCanMove)
        {
            isCanMove = true;
        }
    }
    private void Move()
    {
        isCanMove = false;
        //transform.DOMove(new Vector3(transforms[transformPlayer].position.x,gameObject.transform.position.y, transforms[transformPlayer].position.z), 0.2f).SetEase(Ease.Flash);
        transform.DOMoveZ(transforms[transformPlayer].position.z, 0.2f).SetEase(Ease.Flash);
    }
    private void MoveLeft()
    {
        if(Input.GetKeyDown(KeyCode.A)&&transformPlayer>0)
        {
            lastTransformPlayer = transformPlayer;
            transformPlayer=transformPlayer-1;
            Move();
        }
        if (joystick.Horizontal<=-0.9f && transformPlayer > 0 && isCanMove)
        {
            lastTransformPlayer = transformPlayer;
            transformPlayer = transformPlayer - 1;
            Move();
        }
    }
    private void MoveRight()
    {
        if (Input.GetKeyDown(KeyCode.D) && transformPlayer<2)
        {
            lastTransformPlayer = transformPlayer;
            transformPlayer =transformPlayer+1;
            Move();
        }
        if (joystick.Horizontal >= 0.9f && transformPlayer < 2 && isCanMove)
        {
            lastTransformPlayer = transformPlayer;
            transformPlayer = transformPlayer + 1;
            Move();
        }
    }
    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space)&&canJump)
        {
            animator.SetTrigger("jump");
            canJump = false;
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Observable.Timer(System.TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject).Subscribe(x => canJump = true);
        }
        if (joystick.Vertical >= 0.9f && canJump )
        {
            animator.SetTrigger("jump");
            canJump = false;
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Observable.Timer(System.TimeSpan.FromSeconds(1f)).TakeUntilDisable(gameObject).Subscribe(x => canJump = true);
        }
    }
    private void Roll()
    {
        if(Input.GetKeyDown(KeyCode.S)&&canJump)
        {
            animator.SetTrigger("roll");
            canJump = false;
            capsuleCollider.enabled = false;
            sphereCollider.enabled = true;
            Observable.Timer(System.TimeSpan.FromSeconds(.9f)).TakeUntilDisable(gameObject).Subscribe(x =>
            {
            canJump = true;
            capsuleCollider.enabled = true;
            sphereCollider.enabled = false;
            });
        }
        if (joystick.Vertical<=-0.9f && canJump)
        {
            animator.SetTrigger("roll");
            canJump = false;
            capsuleCollider.enabled = false;
            sphereCollider.enabled = true;
            Observable.Timer(System.TimeSpan.FromSeconds(.9f)).TakeUntilDisable(gameObject).Subscribe(x =>
            {
                canJump = true;
                capsuleCollider.enabled = true;
                sphereCollider.enabled = false;
            });
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            transform.DOMove(transforms[lastTransformPlayer].position, 0.3f).SetEase(Ease.Flash).OnComplete(RewindDOT) ;
            transformPlayer = lastTransformPlayer;
        }
        if(collision.gameObject.CompareTag("LoseCollision"))
        {
            Death();
        }
    }
    private void RewindDOT()
    {
        DOTween.PauseAll();
        transformPlayer = lastTransformPlayer;
    }
    private void Death()
    {
        animator.SetBool("dead", true);
        onDeath?.Invoke();
        this.enabled = false;
        GameManager.instance.EndGame();
    }
}
