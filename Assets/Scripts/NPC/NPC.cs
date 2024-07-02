using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcID;
    public float interactionDistance = 3.0f; // �÷��̾�� ��ȣ�ۿ��� �Ÿ�

    private GameObject player; // �ӽ�
    private DialogueManager dialogueManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueManager = DialogueManager.instance;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < interactionDistance)
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        // NPC ���
        dialogueManager.StartDialogue(npcID);
    }
}