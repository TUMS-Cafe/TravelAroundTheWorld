using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public string csvFileName; // Resources ���� ���� CSV ���� �̸�

    private Dictionary<string, List<string>> dialogues = new Dictionary<string, List<string>>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadDialoguesFromCSV(); // CSV ���Ͽ��� ��� ��������
    }

    void LoadDialoguesFromCSV()
    {
        var data = CSVReader.Read(csvFileName);
        foreach (var row in data)
        {
            if (!row.ContainsKey("npc_id") || !row.ContainsKey("dialogue"))
            {
                Debug.LogWarning("����");
                continue;
            }

            string npcID = row["npc_id"].ToString();
            string dialogue = row["dialogue"].ToString();

            if (!dialogues.ContainsKey(npcID))
            {
                dialogues[npcID] = new List<string>();
            }
            dialogues[npcID].Add(dialogue);
        }
    }

    public void StartDialogue(string npcID)
    {
        if (dialogues.ContainsKey(npcID))
        {
            List<string> npcDialogues = dialogues[npcID];
            // ��� ui�� ��� �Ұ��� ���⿡ �߰��ϱ�
            foreach (string dialogue in npcDialogues)
            {
                Debug.Log(dialogue); // �ϴ� �ֿܼ� ��� ���
            }
        }
        else
        {
            Debug.LogWarning("��� ����" + npcID); // �ӽ� Ȯ�ο� ���߿� ����
        }
    }
}
