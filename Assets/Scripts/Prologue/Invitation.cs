using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invitation : MonoBehaviour
{
    public TalkManager talkManager; // TalkManager ������Ʈ
    private bool talkActivated = false; // TalkManager�� �̹� Ȱ��ȭ�Ǿ����� ����

    void Update()
    {
        if (!talkActivated)
        {
            TalkManagerOn();
        }
    }

    void TalkManagerOn()
    {
        talkManager.ActivateTalk();
        talkActivated = true;
    }
}
