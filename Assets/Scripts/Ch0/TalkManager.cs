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

    public ProDialogue(int day, string location, string speaker, string line)
    {
        this.day = day;
        this.location = location;
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
        GenerateData(); // ������ ���� �Լ� ȣ��
    }

    void Start()
    {
        // ó���� ������Ʈ ��Ȱ��ȭ
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        //CheckTalk();
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

    void GenerateData()
    {
        proDialogue.Add(new ProDialogue(0, "��", "��", "���� �������̳�.."));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "�ʴ��忡�� ��¦�Ÿ��� ���� �ν��� ���������� �Ǹ� �Ǿ� �־���."));
        proDialogue.Add(new ProDialogue(0, "��", "��", "��� �ͼ��ѵ�.."));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "õõ�� ������ ������ ����� ���� ������ ��Ⱑ ������ ���տ� ���ڵ��� ��Ÿ����."));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "������ ó�� ������ �Ǹ��� �ΰ� ��� �� ������ ����� ����."));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "ī�信�� �մ��� ���� ������ �ٶ� TV���� ����ؼ� �����ϴ� �糪 �ͽ��������� �ΰ���."));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "���� �־ �� �� ����, ������ �߼۵� �ʴ����� ���ؼ��� ������ �� �ִٴ� �װ�."));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "�ʴ��忡 ���� ��н����� ������ �������� ž���� �����ϰ�,"));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "�� ��Ҵ� �Ź� �ٲ�� �ƹ��� ž�¿��� ����� �𸥴ٴ� �̽��͸��� ���̾���."));
        proDialogue.Add(new ProDialogue(0, "��", "��", "�����...?"));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "���� �; �� �� ���� ������� ���ٴ� �̰�."));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "��°�� ������ �߼��� �� �������� ���� �� ���� ������."));
        proDialogue.Add(new ProDialogue(0, "��", "��", "��񸸡� ���� �������ݾ�??"));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "�޷��� ���鼭 �� ���� �ǽ�������, �ٲ�� ���� �ƹ��͵� ������."));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "ó������ ���ϵ� ��ؾ� �ϴ� ī�䰡 �����̾�����,"));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "�մԵ� ���� �ʰ� Ư���� �� ���� �Ȱ��� ��Ǵ� ī�信 �����ִ� ��Ȳ�� �� �Ǿ��ٴ� ������ �����."));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "�� ������ ����, �ٷ� ���� �α� �����ߴ�."));
        proDialogue.Add(new ProDialogue(0, "��", "�����̼�", "������ �ð��� ������."));

        proDialogue.Add(new ProDialogue(1, "��", "�����̼�", "������ ���� ���� ���� ��Ÿ� ����."));
        proDialogue.Add(new ProDialogue(1, "��", "��", "��ø�... ���� ���Ⱑ �´� �ž�??"));
        proDialogue.Add(new ProDialogue(1, "��", "�����̼�", "ó������ ������ �߸��� �� �˾�����, ��ġ�� �и��� �� ���� ����Ű�� �־���."));
        proDialogue.Add(new ProDialogue(1, "��", "�����̼�", "�ݽŹ����ϸ� ���� ��ġ�� 15�� ���� ������, �� ���տ� ���� �� ���� ������ ��Ÿ����."));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "��ġ ������ ĥ�س��� �Ͱ� ���� ��¦�̴� �ٴٻ��� �������� �� ���տ� ��Ÿ����."));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "���� ������ �����ϰ� �ִ� ������ ������ �ձ�� �����ϰ� �� ����ϰ� �Ĵٺ� ���ۿ� ���� �������."));
        proDialogue.Add(new ProDialogue(1, "������", "��", "�̷��� �Ƹ��ٿ� ���� ��°�� �̿�ǰ� ���� �ʾҴ� ����?"));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "�Ƹ��ٿ� �ձ�� �ٵ���� �������� ���Ӱ�, ���� ���� ����� �־�, ���� ��Ǯ�� �Բ� ��췯�� �ִ� ����� ���� �ݴ�����ν� ���� �Ƹ��ٿ� �� ������."));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "���� 3��, ȣ���� �Ҹ��� �Բ� ������ �̲������� �������� �������Դ�."));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "�״ÿ��� ��� ������ �ݺ� âƲ�� �� �����̰� �޻��� �޾� ��¦��¦ ������."));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "£�� Ǫ���� ��ü�� �� ������ �̰� �ִ� ������ �� �����԰� �䵵����� �� ���õ� �Ż� ���Ҵ�."));
        proDialogue.Add(new ProDialogue(1, "������", "��", "�̰������� ���ۿ� �� Ÿ�� �ǰ�??"));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "������ ������������, �� �̿� �ٸ� �°����� ������ �ʾҴ�."));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "������, ������ õõ�� �� ���� ���� �����."));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "�ٷ� ���� ����ĭ ���� ������, ���ٴ����� ���� �� ������ ���� ���� �����ϰ�� �ݰ��� ������ ������."));
        proDialogue.Add(new ProDialogue(1, "������", "???", "�� ��ħ �� �Գ�!"));
        proDialogue.Add(new ProDialogue(1, "������", "???", "����� ���ú��� �� ī�並 ����� �ٸ���Ÿ���??"));
        proDialogue.Add(new ProDialogue(1, "������", "���̿÷�", "���� ���̿÷�! �ݰ���~"));
        proDialogue.Add(new ProDialogue(1, "������", "��", "��?? ���� ī���� �մ����� �ʴ밡 �� �� �˾Ҵµ���???"));
        proDialogue.Add(new ProDialogue(1, "������", "���̿÷�", "���� �Ҹ��� �޸�"));
        proDialogue.Add(new ProDialogue(1, "������", "���̿÷�", "���� ������ ��� �̸����� ������ ������� �ִ� ��?"));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "��忡 �������� ������ ���� �̸��� �޺��� �ݻ�Ǿ� ���� ����."));
        proDialogue.Add(new ProDialogue(1, "������", "�����̼�", "�̸� �Ʒ����� LUNAR EXPRESS BARISTA��� ���� �𸣴� ������ ��õǾ� �־���."));
        proDialogue.Add(new ProDialogue(1, "������", "���̿÷�", "�޸� �̷� �ð��� ����! ���� �մԵ��� ��ħ���� Ŀ�Ǹ� ���� ���ؼ� �Ҹ��� ������� �ִٱ�~"));
        proDialogue.Add(new ProDialogue(1, "������", "���̿÷�", "������ �� ���� �˷��� �״� �մԵ� ���� �ذ��� ��!!"));
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
                if (currentDialogueIndex >= 1 && currentDialogueIndex <= 16)
                {
                    invitation.SetActive(true);
                    if (currentDialogueIndex >= 1 && currentDialogueIndex <= 3)
                    {
                        invitationText.gameObject.SetActive(false);
                    }
                    else if (currentDialogueIndex >= 4)
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
                if (currentDialogueIndex >= 26)
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
