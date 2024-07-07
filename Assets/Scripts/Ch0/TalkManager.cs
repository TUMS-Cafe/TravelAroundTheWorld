using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�

// ProDialogue Ŭ���� ���� (���ѷα� ��� ����)
public class ProDialogue
{
    public int day; // ����
    public string location; // ���
    public string speaker; // �ι�
    public string line; // ���
    public string screenEffect; // ȭ�� ����
    public string backgroundMusic; // �������
    public string expression; // ǥ��
    public string note; // ���
    public string quest; // ����Ʈ
    public string questContent; // ����Ʈ ����

    public ProDialogue(int day, string location, string speaker, string line, string screenEffect, string backgroundMusic, string expression, string note, string quest, string questContent)
    {
        this.day = day;
        this.location = location;
        this.speaker = speaker;
        this.line = line;
        this.screenEffect = screenEffect;
        this.backgroundMusic = backgroundMusic;
        this.expression = expression;
        this.note = note;
        this.quest = quest;
        this.questContent = questContent;
    }
}

public class TalkManager : MonoBehaviour
{
    // ������ ������ ����Ʈ
    private List<ProDialogue> proDialogue;

    public GameObject opening;
    public TextMeshProUGUI openingText; // TextMeshPro UI �ؽ�Ʈ ���

    public GameObject narration;
    public TextMeshProUGUI narrationText; // TextMeshPro UI �ؽ�Ʈ ���

    public GameObject dialogue;
    public GameObject imageObj; // �ʻ�ȭ �̹��� ���
    public GameObject nameObj; // �̸� ���
    public TextMeshProUGUI nameText; // TextMeshPro UI �ؽ�Ʈ ���
    public TextMeshProUGUI descriptionText; // TextMeshPro UI �ؽ�Ʈ ���

    public GameObject invitation; // �ʴ��� ȭ��
    public TextMeshProUGUI invitationText; // TextMeshPro UI �ؽ�Ʈ ���

    public GameObject forest; // �� ȭ��

    public GameObject trainStation; // ������ ȭ��
    public GameObject train; // ���� ȭ��

    private int currentDialogueIndex = 0; // ���� ��� �ε���
    private bool isActivated = false; // TalkManager�� Ȱ��ȭ�Ǿ����� ����

    void Awake()
    {
        proDialogue = new List<ProDialogue>();
        LoadDialogueFromCSV(); // CSV���� �����͸� �ε��ϴ� �Լ� ȣ��
    }

    void Start()
    {
        ActivateTalk(); // ������Ʈ Ȱ��ȭ
    }

    void Update()
    {
        if (isActivated && currentDialogueIndex == 0)
        {
            PrintProDialogue(currentDialogueIndex);
        }
        if (isActivated && Input.GetKeyDown(KeyCode.Space))
        {
            currentDialogueIndex++;
            PrintProDialogue(currentDialogueIndex);
        }
    }

    void LoadDialogueFromCSV()
    {
        List<Dictionary<string, object>> data_Dialog = Ch0CSVReader.Read("Travel Around The World - CH0");

        foreach(var row in data_Dialog)
        {
            int day = int.Parse(row["����"].ToString().Replace("����", "").Trim());
            string location = row["���"].ToString();
            string speaker = row["�ι�"].ToString();
            string line = row["���"].ToString();
            string screenEffect = row["ȭ�� ����"].ToString();
            string backgroundMusic = row["�������"].ToString();
            string expression = row["ǥ��"].ToString();
            string note = row["���"].ToString();
            string quest = row["����Ʈ"].ToString();
            string questContent = row["����Ʈ ����"].ToString();

            proDialogue.Add(new ProDialogue(day, location, speaker, line, screenEffect, backgroundMusic, expression, note, quest, questContent));
        }
    }

    void PrintProDialogue(int index)
    {
        if (index >= proDialogue.Count)
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            return; // ��� ����Ʈ�� ����� ������Ʈ ��Ȱ��ȭ �� ����
        }

        ProDialogue currentDialogue = proDialogue[index];

        if (index < 2)
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            opening.SetActive(true);
            openingText.text = currentDialogue.line;
        }
        //������ ��� ���ĺ��� �ι��� ���� ���/�����̼�/�ؽ�Ʈ â Ȱ��ȭ
        else if (currentDialogue.speaker == "�ʴ���")
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            opening.SetActive(false);
            if (!string.IsNullOrEmpty(invitationText.text))
            {
                invitationText.text += "\n"; // ���� ������ ������ �� �� ���� �߰�
            }
            invitationText.text += currentDialogue.line; // ���ο� ���� �߰�
        }
        else if (currentDialogue.speaker == "�����̼�")
        {
            narration.SetActive(true);
            dialogue.SetActive(false);
            opening.SetActive(false);
            narrationText.text = currentDialogue.line;
        }
        else
        {
            narration.SetActive(false);
            dialogue.SetActive(true);
            opening.SetActive(false);
            nameText.text = currentDialogue.speaker;
            descriptionText.text = currentDialogue.line;
        }

        CheckTalk(currentDialogue.location);
    }

    public void ActivateTalk()
    {
        this.gameObject.SetActive(true);
        isActivated = true;
    }

    void DeactivateTalk()
    {
        this.gameObject.SetActive(false);
        isActivated = false;
    }

    void CheckTalk(string location)
    {
        invitation.SetActive(false);
        forest.SetActive(false);
        trainStation.SetActive(false);
        train.SetActive(false);

        switch (location)
        {
            case "��":
                if (currentDialogueIndex >= 3 && currentDialogueIndex <= 22)
                {
                    invitation.SetActive(true);
                    if (currentDialogueIndex >= 3 && currentDialogueIndex <= 5)
                    {
                        invitationText.gameObject.SetActive(false);
                    }
                    else if (currentDialogueIndex >= 6)
                    {
                        invitationText.gameObject.SetActive(true);
                    }
                }
                break;
            case "��":
                forest.SetActive(true);
                break;
            case "������":
                trainStation.SetActive(true);
                if (currentDialogueIndex >= 32)
                {
                    train.SetActive(true);
                }
                break;
        }
        if (currentDialogueIndex > proDialogue.Count)
        {
            DeactivateTalk();
        }
    }
}
