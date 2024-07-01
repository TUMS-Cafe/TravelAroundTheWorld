using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�

// ProDialogue Ŭ���� ���� (���ѷα� ��� ����)
public class ProDialogue
{
    public int id; // ���̵�
    public string speaker; // ��ȭ��
    public string line; // ���

    public ProDialogue(int id, string speaker, string line)
    {
        this.id = id;
        this.speaker = speaker;
        this.line = line;
    }
}

public class TalkManager : MonoBehaviour
{
    // ������ ������ ����Ʈ
    private List<ProDialogue> proDialogue;

    public GameObject narration;
    public TextMeshProUGUI narrationText; // TextMeshPro UI �ؽ�Ʈ ���

    public GameObject dialogue;
    public GameObject imageObj; // �ʻ�ȭ �̹��� ���
    public GameObject nameObj; // �̸� ���
    public TextMeshProUGUI nameText; // TextMeshPro UI �ؽ�Ʈ ���
    public TextMeshProUGUI descriptionText; // TextMeshPro UI �ؽ�Ʈ ���

    public GameObject invitation; // �ʴ��� ������Ʈ
    public TextMeshProUGUI invitationText; // TextMeshPro UI �ؽ�Ʈ ���

    private int currentDialogueIndex = 0; // ���� ��� �ε���
    private bool isActivated = false; // TalkManager�� Ȱ��ȭ�Ǿ����� ����

    void Awake()
    {
        proDialogue = new List<ProDialogue>();
        GenerateData(); // ������ ���� �Լ� ȣ��
    }

    void Start()
    {
        // ó���� TalkManager ������Ʈ ��Ȱ��ȭ
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckTalk();
        if (isActivated && Input.GetKeyDown(KeyCode.Space))
        {
            PrintProDialogue(currentDialogueIndex);
            currentDialogueIndex++;
        }
    }

    void GenerateData()
    {
        proDialogue.Add(new ProDialogue(1, "��", "���� �������̳�.."));
        proDialogue.Add(new ProDialogue(2, "�����̼�", "�ʴ��忡�� ��¦�Ÿ��� ���� �ν��� ���������� �Ǹ� �Ǿ� �־���."));
        proDialogue.Add(new ProDialogue(3, "��", "��� �ͼ��ѵ�.."));
        proDialogue.Add(new ProDialogue(4, "�����̼�", "õõ�� ������ ������ ����� ���� ������ ��Ⱑ ������ ���տ� ���ڵ��� ��Ÿ����."));
        proDialogue.Add(new ProDialogue(5, "�����̼�", "������ ó�� ������ �Ǹ��� �ΰ� ��� �� ������ ����� ����."));
        proDialogue.Add(new ProDialogue(5, "�����̼�", "ī�信�� �մ��� ���� ������ �ٶ� TV���� ����ؼ� �����ϴ� �糪 �ͽ��������� �ΰ���."));
        proDialogue.Add(new ProDialogue(5, "�����̼�", "���� �־ �� �� ����, ������ �߼۵� �ʴ����� ���ؼ��� ������ �� �ִٴ� �װ�."));
        proDialogue.Add(new ProDialogue(5, "�����̼�", "�ʴ��忡 ���� ��н����� ������ �������� ž���� �����ϰ�,"));
        proDialogue.Add(new ProDialogue(5, "�����̼�", "�� ��Ҵ� �Ź� �ٲ�� �ƹ��� ž�¿��� ����� �𸥴ٴ� �̽��͸��� ���̾���."));
        proDialogue.Add(new ProDialogue(6, "��", "�����...?"));
        proDialogue.Add(new ProDialogue(7, "�����̼�", "���� �; �� �� ���� ������� ���ٴ� �̰�."));
        proDialogue.Add(new ProDialogue(7, "�����̼�", "��°�� ������ �߼��� �� �������� ���� �� ���� ������."));
        proDialogue.Add(new ProDialogue(8, "��", "��񸸡� ���� �������ݾ�??"));
        proDialogue.Add(new ProDialogue(9, "�����̼�", "�޷��� ���鼭 �� ���� �ǽ�������, �ٲ�� ���� �ƹ��͵� ������."));
        proDialogue.Add(new ProDialogue(9, "�����̼�", "ó������ ���ϵ� ��ؾ� �ϴ� ī�䰡 �����̾�����,"));
        proDialogue.Add(new ProDialogue(9, "�����̼�", "�մԵ� ���� �ʰ� Ư���� �� ���� �Ȱ��� ��Ǵ� ī�信 �����ִ� ��Ȳ�� �� �Ǿ��ٴ� ������ �����."));
        proDialogue.Add(new ProDialogue(9, "�����̼�", "�� ������ ����, �ٷ� ���� �α� �����ߴ�."));
        proDialogue.Add(new ProDialogue(9, "�����̼�", "������ �ð��� ������."));
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

        if (currentDialogue.speaker == "�����̼�")
        {
            narration.SetActive(true);
            dialogue.SetActive(false);
            narrationText.text = currentDialogue.line;
        }
        else
        {
            narration.SetActive(false);
            dialogue.SetActive(true);
            nameText.text = currentDialogue.speaker;
            descriptionText.text = currentDialogue.line;
        }
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

    void CheckTalk()
    {
        //�ʴ��� �ؽ�Ʈ �� �ִϸ��̼� ������ ����
        if (currentDialogueIndex == 4)
        {
            DeactivateTalk();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                invitationText.gameObject.SetActive(true);
            }
            ActivateTalk();
        }
        /*
        // �� ȭ�� ��ȯ �� �����ǰ� ���δ� ����
        else if (currentDialogueIndex == 18)
        {
            invitation.SetActive(false);
            DeactivateTalk();
            narration.SetActive(false);
            dialogue.SetActive(false);
        }
        */
        // ������ ������ ��µǾ��� ��
        else if (currentDialogueIndex > proDialogue.Count)
        {
            DeactivateTalk();
        }
    }
}
