using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Opening : MonoBehaviour
{
    private List<string> dialogues; // ������ ������ ����Ʈ
    public TextMeshProUGUI openingText; // TextMeshPro UI �ؽ�Ʈ ���

    public GameObject invitation; // ��� �ʴ��� ������Ʈ
    public TextMeshProUGUI invitationText; // TextMeshPro UI �ؽ�Ʈ ���

    private int currentDialogueIndex = 0; // ���� ��� �ε���

    void Start()
    {
        dialogues = new List<string>();
        GenerateDialogue(); // ��� ���� �Լ� ȣ��
        PrintDialogue(); // ù ��° ��� ���
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrintDialogue(); // �����̽��ٸ� ������ �� ���� ��� ���
        }
    }

    void GenerateDialogue()
    {
        dialogues.Add("��� ���� �� ��н����� �ʴ������κ��� ���۵Ǿ���.");
        dialogues.Add("���� �� ������ ���� ���� ���� �ʷ� ���ĸ��� ���� ���� �����ϴ� �����̾���.");
    }

    void PrintDialogue()
    {
        if (currentDialogueIndex < dialogues.Count)
        {
            openingText.text = dialogues[currentDialogueIndex];
            currentDialogueIndex++;
        }
        else
        {
            this.gameObject.SetActive(false); // Opening ������Ʈ ��Ȱ��ȭ
            invitation.SetActive(true); // Invitation ������Ʈ Ȱ��ȭ
            invitationText.gameObject.SetActive(false); // Invitation ������Ʈ Ȱ��ȭ
            return;
        }
    }
}
