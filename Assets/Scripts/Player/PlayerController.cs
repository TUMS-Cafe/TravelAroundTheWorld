using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float speed = 15.0f;
    Vector2 inputVector;
    Rigidbody2D rigid;
    public bool canMove = true;

    public Camera mainCamera;

    public PlayerAnimationController playerAnimationController;
    public GameObject targetClickPrefab;
    public GameObject currentTargetClick;

    public Animator currentTargetClickAnimator;

    public bool isColliding = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerAnimationController = gameObject.GetComponent<PlayerAnimationController>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        currentTargetClick = Instantiate(targetClickPrefab);
        currentTargetClick.SetActive(false);
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        currentTargetClickAnimator = currentTargetClick.GetComponent<Animator>();
        currentTargetClickAnimator.SetBool("isTargeted", true);
    }

    void Update()
    {
        if (!UIManager.Instance.IsUIActive() && canMove && !isColliding)
        {
            Move();
        }
    }

    void OnMove(InputValue inputValue)
    {
        if (canMove && !isColliding)
        {
            inputVector = inputValue.Get<Vector2>();
            playerAnimationController.SetMoveDirection(inputVector);

            if (currentTargetClick != null && currentTargetClick.activeSelf)
            {
                currentTargetClick.SetActive(false);
                currentTargetClickAnimator.SetBool("isTargeted", true);
            }
        }

        else if (isColliding)
        {
            // ?? ???? ?? ??? ???? ???? ??? ??? ?
            isColliding = false;
            StartMove();
        }
    }

    void Move()
    {
        if (UIManager.Instance.IsUIActive())
            inputVector = Vector2.zero;

        if (inputVector != Vector2.zero)
        {
            Vector2 moveVector = inputVector.normalized * speed * Time.deltaTime;
            rigid.MovePosition(rigid.position + moveVector);
        }
    }

    void OnInventory()
    {
        if (canMove)
        {
            UIManager.Instance.ToggleUI("Inventory");
        }
    }

    void OnSetting()
    {
        if (canMove)
        {
            UIManager.Instance.ToggleUI("Setting");
        }
    }

    /*
    void OnMap()
    {
        if (canMove)
        {
            UIManager.Instance.ToggleUI("Map");
        }
    }
    */


    void OnMouseMove()
    {
        if (!UIManager.Instance.IsUIActive() && canMove && !isColliding)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            if (currentTargetClick != null)
            {
                currentTargetClick.transform.position = worldPos;
                currentTargetClick.SetActive(true);
                currentTargetClickAnimator.SetBool("isTargeted", true);
            }

            playerAnimationController.MoveToPosition(worldPos, speed * 0.5f);
        }
        else if (isColliding)
        {
            // ?? ???? ?? ??? ???? ???? ??? ??? ?
            isColliding = false;
            StartMove();
        }
    }

    public void StopMove()
    {
        canMove = false;
        inputVector = Vector2.zero;
        playerAnimationController.SetMoveDirection(Vector2.zero);
        playerAnimationController.StopMovingCoroutine(); 
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        currentTargetClick.SetActive(false);
        currentTargetClickAnimator.SetBool("isTargeted", false);
    }

    public void StartMove()
    {
        canMove = true;
    }


    public void ColliderStart()
    {
        isColliding = true;
        StopMove();
        //마우스 클릭으로 설정해놓은 MoveToPosition 함수 
    }

    public void ColliderEnd()
    {
        isColliding = false;
        StartMove();
    }

    
}