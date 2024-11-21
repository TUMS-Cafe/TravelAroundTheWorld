using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float speed = 4f;
    Vector2 inputVector;
    Rigidbody2D rigid;
    public bool canMove = true;

    public Camera mainCamera;

    public PlayerAnimationController playerAnimationController;
    public GameObject targetClickPrefab;
    public GameObject currentTargetClick;

    public Animator currentTargetClickAnimator;

    public CameraFollow cameraFollow;

    public bool isColliding = false;

    //필요한 오브젝트 세팅
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerAnimationController = gameObject.GetComponent<PlayerAnimationController>();

        cameraFollow = FindObjectOfType<CameraFollow>();

        currentTargetClick = Instantiate(targetClickPrefab);
        currentTargetClick.SetActive(false);
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        currentTargetClickAnimator = currentTargetClick.GetComponent<Animator>();
        currentTargetClickAnimator.SetBool("isTargeted", true);
    }

    //Move관련 체크
    void Update()
    {
        if (!UIManager.Instance.IsUIActive() && canMove)// && !isColliding)
        {
            Move();
        }
    }

    //키보드 이동
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
            isColliding = false;
            StartMove();
        }
        
    }

    //마우스 클릭시 이동
    void OnMouseMove()
    {
        if (!UIManager.Instance.IsUIActive() && canMove && !isColliding)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = cameraFollow.mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            if (currentTargetClick != null)
            {
                currentTargetClick.transform.position = worldPos;
                currentTargetClick.SetActive(true);
                currentTargetClickAnimator.SetBool("isTargeted", true);
            }
            //클릭으로 이동할 때 마우스보다 움직임 빠른 이유 해결해야 함
            playerAnimationController.MoveToPosition(worldPos, speed * 0.5f);
        }
        else if (isColliding)
        {
            isColliding = false;
            StartMove();
        }
    }

    //이동 관련 처리
    void Move()
    {
        if (UIManager.Instance.IsUIActive())
            inputVector = Vector2.zero;

        if (inputVector != Vector2.zero)
        {
            Vector2 moveVector = inputVector.normalized * speed * Time.deltaTime;
            rigid.MovePosition(rigid.position + moveVector);
        }

        else if (inputVector == Vector2.zero)
        {
            playerAnimationController.SetMoveDirection(Vector2.zero);
        }

    }

    //이동 중지
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

    //이동 재개
    public void StartMove()
    {
        canMove = true;
    }

    void OnInteraction()
    {

    }

    void InteractionWithObject()
    {

    }

    void OnMouseClickObject()
    {

    }

    void OnShowGroupUI()
    {
        Debug.Log("Show GroupUI");
        if(canMove)
        {
            UIManager.Instance.ToggleUI("Group");
        }
    }


    public void ColliderStart()
    {
        isColliding = true;

        if (cameraFollow != null)
        {
            cameraFollow.SetCameraFollow(false);
        }
    }

    public void ColliderEnd()
    {
        isColliding = false;
        StartMove();

        if (cameraFollow != null)
        {
            cameraFollow.SetCameraFollow(true);
        }
    }

    /*
세팅창 활성화
void OnSetting()
{
    if (canMove)
    {
        UIManager.Instance.ToggleUI("Setting");
    }
}

    /*
    맵 활성화
    void OnMap()
    {
        if (canMove)
        {
            UIManager.Instance.ToggleUI("Map");
        }
    }
    */

}