using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientController : MonoBehaviour
{
    private Vector3 defaultPos;
    private Vector3 hotCupdefaultPos = new Vector3(-1.59f, -2.46f, -1f);
    private Vector3 iceCupdefaultPos = new Vector3(-1.56f, 0.58f, -1f);
    private Vector3 offset;
    private Vector3 makePos = new Vector3(3.5f, -2.5f, -1f);
    private bool isDragging = false;
    private GameObject makeArea;
    private GameObject trashcan;
    private CafeMakeController cafeMakeController;
    private TrashController trashController;

    public GameObject HotCup;
    public GameObject IceCup;



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
        if (makeArea != null && makeArea.GetComponent<Collider2D>().bounds.Contains(transform.position))
        {
            Debug.Log(gameObject.name + " dropped on MakeArea");
            cafeMakeController.HandleMakeArea(gameObject);
        }
        if (gameObject.name == "IceCup" || gameObject.name == "HotCup")
        {
            transform.position = makePos;
        }
        else
            transform.position = defaultPos;

    }

    private void CheckForTrashCan()
    {
        if (trashcan != null && trashcan.GetComponent<Collider2D>().bounds.Contains(transform.position))
        {
            Debug.Log(gameObject.name + " dropped on trashcan");
            trashController.HandleTrashCan(gameObject);
        }

    }

    public void CupPos(string cupname)
    {
        if (cupname == "HotCup")
        {
            HotCup.transform.position = hotCupdefaultPos;
        }
        else if (cupname == "IceCup")
            IceCup.transform.position = iceCupdefaultPos;
    }


    private Vector3 GetMouseWorldPosition()
    {
        // 마우스의 월드 좌표를 반환합니다.
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z; // 오브젝트의 Z 축 유지
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

}
