using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    private Vector2 lastInputVector;
    private Coroutine moveCoroutine;

    private IPlayerState currentState;
    public readonly IPlayerState IdleState = new PlayerIdleState();
    public readonly IPlayerState WalkingState = new PlayerWalkingState();
    public readonly IPlayerState FallingAsleepState = new PlayerFallingAsleepState();
    
    public PlayerController playerController;

    public Vector2 InputVector { get; private set; }

    public bool isMoving = false;
    private Vector3 targetPosition;

    private bool stopMoving = false;

    private void Awake()
    {
        playerController = transform.gameObject.GetComponent<PlayerController>();
    }

    private void Start()
    {
        //TransitionToState(IdleState);
    }

    private void Update()
    {
        //currentState.UpdateState(this);
    }

    public void SetMoveDirection(Vector2 inputVector)
    {
        if (inputVector != Vector2.zero)
        {
            lastInputVector = inputVector;
        }

        Vector2 directionToAnimate = inputVector != Vector2.zero ? inputVector : lastInputVector;

        animator.SetFloat("xDir", directionToAnimate.x);
        animator.SetFloat("yDir", directionToAnimate.y);
        animator.SetBool("isMove", inputVector != Vector2.zero);

        /*
         * if(inputVector != Vector2.zero)
         *      SoundManager.Instance.PlaySFX("WalkSound");
         */
    }



    public void TransitionToState(IPlayerState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    /*
    public void SetTargetPosition(Vector3 targetPos)
    {
        targetPosition = targetPos;
        isMoving = true;

        Vector3 direction = (targetPosition - transform.position).normalized;
        UpdateAnimatorParameters(direction);
    }

    public void MoveToPosition()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        Vector3 direction = (targetPosition - transform.position).normalized;
        UpdateAnimatorParameters(direction);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            animator.SetBool("isMove", false);
        }
    }*/

    public void MoveToPosition(Vector3 targetPos, float speed)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveToPositionCoroutine(targetPos, speed));
    }

    private IEnumerator MoveToPositionCoroutine(Vector3 targetPos, float speed)
    {
        stopMoving = false;
        lastInputVector = targetPos;
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);

            if(playerController.isColliding)
            {
                isMoving = false;
                break;
                //yield return null;
            }

            SetMoveDirection(new Vector2(direction.x, direction.y));
            yield return null;
        }
        SetMoveDirection(Vector2.zero);
        moveCoroutine = null;
        playerController.currentTargetClick.SetActive(false);

        playerController.ColliderEnd();
    }

    /*
    private void UpdateAnimatorParameters(Vector3 direction)
    {
        if (direction.magnitude > 0)
        {
            animator.SetFloat("xDir", direction.x);
            animator.SetFloat("yDir", direction.y);
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }*/

    public void StopMovingCoroutine()
    {
        stopMoving = true;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    public void StopAllCoroutines()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

}