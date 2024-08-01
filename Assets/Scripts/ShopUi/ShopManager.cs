using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject shopPrefab; // ���� UI ������

    // ��ư Ŭ�� �� ȣ��� �޼���
    public void ShowShop()
    {
        Transform parentTransform = transform.parent;

        GameObject newObject = Instantiate(shopPrefab, parentTransform);
        newObject.SetActive(true);
    }

    // ���� UI�� ����� �޼���
    public void HideShop()
    {
        //Transform parentTransform = transform.parent;
        Destroy(transform.parent.gameObject);
    }

}

