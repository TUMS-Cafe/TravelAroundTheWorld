using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Managers : MonoBehaviour
{
    public DialogueManager dialogueManager;
    static Managers s_instance;
    public static Managers Instance
    {
        get
        {
            Init();
            return s_instance;
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {

    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject obj = GameObject.Find("@Managers");
            if (obj == null)
            {
                obj = new GameObject { name = "@Managers" };
                obj.AddComponent<Managers>();
            }

            DontDestroyOnLoad(obj);
            s_instance = obj.GetComponent<Managers>();
            s_instance.dialogueManager = DialogueManager.Instance;
        }
    }

    SceneManagerEx _scene = new SceneManagerEx();
    public static SceneManagerEx Scene { get { return Instance._scene; } }

    int talkIndex = 0; // ��ȭ �ε���
    bool isAction = false; // ��ȭ ������ ����
    public Text talkText; // UI �ؽ�Ʈ ������Ʈ

    public void Dialogue(string npcID, bool isNpc)
    {
        string talkData = dialogueManager.GetTalk(npcID, talkIndex);

        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            return;
        }

        talkText.text = talkData;

        isAction = true;
        talkIndex++;
    }
}
