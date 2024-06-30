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
    public GameObject dialogue;
    public TextMeshProUGUI NarrationText; // TextMeshPro UI �ؽ�Ʈ ���
    public TextMeshProUGUI DescriptionText; // TextMeshPro UI �ؽ�Ʈ ���
    public TextMeshProUGUI NameText; // TextMeshPro UI �ؽ�Ʈ ���

    private int currentDialogueIndex = 0; // ���� ��� �ε���

    void Awake()
    {
        proDialogue = new List<ProDialogue>();
        GenerateData(); // ������ ���� �Լ� ȣ��
    }

    void GenerateData()
    {
        proDialogue.Add(new ProDialogue(1, "�����̼�", "��� ���� �� ��н����� �ʴ������κ��� ���۵Ǿ���."));
        proDialogue.Add(new ProDialogue(1, "�����̼�", "���� �� ������ ���� ���� ���� �ʷ� ���ĸ��� ���� ���� �����ϴ� �����̾���."));
        proDialogue.Add(new ProDialogue(2, "��", "���� �������̳�.."));
        proDialogue.Add(new ProDialogue(3, "�����̼�", "�ʴ��忡�� ��¦�Ÿ��� ���� �ν��� ���������� �Ǹ� �Ǿ� �־���."));
        proDialogue.Add(new ProDialogue(4, "��", "��� �ͼ��ѵ�.."));
        proDialogue.Add(new ProDialogue(5, "�����̼�", "õõ�� ������ ������ ����� ���� ������ ��Ⱑ ������ ���տ� ���ڵ��� ��Ÿ����."));
        proDialogue.Add(new ProDialogue(6, "�����̼�", "������ ó�� ������ �Ǹ��� �ΰ� ��� �� ������ ����� ����."));
        proDialogue.Add(new ProDialogue(6, "�����̼�", "ī�信�� �մ��� ���� ������ �ٶ� TV���� ����ؼ� �����ϴ� �糪 �ͽ��������� �ΰ���."));
        proDialogue.Add(new ProDialogue(6, "�����̼�", "���� �־ �� �� ����, ������ �߼۵� �ʴ����� ���ؼ��� ������ �� �ִٴ� �װ�."));
        proDialogue.Add(new ProDialogue(6, "�����̼�", "�ʴ��忡 ���� ��н����� ������ �������� ž���� �����ϰ�,"));
        proDialogue.Add(new ProDialogue(6, "�����̼�", "�� ��Ҵ� �Ź� �ٲ�� �ƹ��� ž�¿��� ����� �𸥴ٴ� �̽��͸��� ���̾���."));
        proDialogue.Add(new ProDialogue(7, "��", "�����...?"));
        proDialogue.Add(new ProDialogue(8, "�����̼�", "���� �; �� �� ���� ������� ���ٴ� �̰�."));
        proDialogue.Add(new ProDialogue(8, "�����̼�", "��°�� ������ �߼��� �� �������� ���� �� ���� ������."));
        proDialogue.Add(new ProDialogue(9, "��", "��񸸡� ���� �������ݾ�??"));
        proDialogue.Add(new ProDialogue(10, "�����̼�", "�޷��� ���鼭 �� ���� �ǽ�������, �ٲ�� ���� �ƹ��͵� ������."));
        proDialogue.Add(new ProDialogue(10, "�����̼�", "ó������ ���ϵ� ��ؾ� �ϴ� ī�䰡 �����̾�����,"));
        proDialogue.Add(new ProDialogue(10, "�����̼�", "�մԵ� ���� �ʰ� Ư���� �� ���� �Ȱ��� ��Ǵ� ī�信 �����ִ� ��Ȳ�� �� �Ǿ��ٴ� ������ �����."));
        proDialogue.Add(new ProDialogue(10, "�����̼�", "�� ������ ����, �ٷ� ���� �α� �����ߴ�."));
        proDialogue.Add(new ProDialogue(10, "�����̼�", "������ �ð��� ������."));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrintProDialogue(currentDialogueIndex);
            currentDialogueIndex++;
        }
    }

    void PrintProDialogue(int index)
    {
        if (index >= proDialogue.Count) return; // ��� ����Ʈ�� ����� ����

        ProDialogue currentDialogue = proDialogue[index];

        if (currentDialogue.speaker == "�����̼�")
        {
            narration.SetActive(true);
            dialogue.SetActive(false);
            NarrationText.text = currentDialogue.line;
        }
        else
        {
            narration.SetActive(false);
            dialogue.SetActive(true);
            NameText.text = currentDialogue.speaker;
            DescriptionText.text = currentDialogue.line;
        }
    }
}
