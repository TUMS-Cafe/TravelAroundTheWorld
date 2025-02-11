using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    private Vector3 defaultPos;
    private Vector3 offset;
    private bool isDragging = false;
    private GameObject makeArea;
    private GameObject trashcan;
    private CafeMakeController cafeMakeController;
    private TrashController trashController;

    public GameObject makeHotCup;
    public GameObject makeIceCup;

    public GameObject Espresso;
    public GameObject IceAmericano;
    public GameObject HotAmericano;
    public GameObject IceLatte;
    public GameObject HotLatte;
    public GameObject HibiscusTea;
    public GameObject ChamomileTea;
    public GameObject RooibosTea;
    public GameObject GreenTea;

    public GameObject Affogato;


    void Start()
    {
        defaultPos = this.transform.position;
        makeArea = GameObject.Find("MakeArea");
        trashcan = GameObject.Find("TrashCan");
        cafeMakeController = FindObjectOfType<CafeMakeController>();
        trashController = FindObjectOfType<TrashController>();
    }

    void OnMouseDown()
    {
        defaultPos = transform.position;
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckForMakeArea();
        CheckForTrashCan();
    }

    
     private void CheckForMakeArea()
    {
        if (makeArea.GetComponent<Collider2D>().OverlapPoint(transform.position))
        {
            if (!Espresso.activeSelf && !IceAmericano.activeSelf && !HotAmericano.activeSelf && !IceLatte.activeSelf && !HotLatte.activeSelf &&
                !GreenTea.activeSelf && !HibiscusTea.activeSelf && !ChamomileTea.activeSelf && !RooibosTea.activeSelf)
            {
                if (!makeHotCup.activeSelf && gameObject.name == "IceCup")
                {
                    makeIceCup.SetActive(true);
                }
                else if (!makeIceCup.activeSelf && gameObject.name == "HotCup")
                {
                    makeHotCup.SetActive(true);
                }
                Debug.Log(gameObject.name + " dropped on MakeArea");
                if (!(makeHotCup.activeSelf && gameObject.name == "IceCup") &&
                !(makeIceCup.activeSelf && gameObject.name == "HotCup"))
                {
                    Debug.Log(gameObject.name + " dropped on MakeArea");
                    cafeMakeController.HandleMakeArea(gameObject);
                }
            }
        }

        transform.position = defaultPos;
    }

    private void CheckForTrashCan()
    {
        Collider2D trashCollider = trashcan.GetComponent<Collider2D>();
        Collider2D thisCollider = GetComponent<Collider2D>();

        if (trashCollider != null && thisCollider != null && thisCollider.bounds.Intersects(trashCollider.bounds))
        {
            Debug.Log(gameObject.name + " dropped on trashcan");
            //trashController.HandleTrashCan(gameObject);
            SoundManager.Instance.PlaySFX("trash");
        }
    }


private Vector3 GetMouseWorldPosition()
    {
        // 마우스의 월드 좌표를 반환합니다.
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z; // 오브젝트의 Z 축 유지
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

}
