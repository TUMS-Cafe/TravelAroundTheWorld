using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogueRefactoring : MonoBehaviour
{
    Dictionary<int, ProDialogue> dialogues = new Dictionary<int, ProDialogue>();

    void Start()
    {
        //CSV 데이터 읽어오기 및 노드 생성 
        LoadDialogueFromCSV();

        //트리 생성
        SetDialogueTree();

        //대화 시작
        StartDialogue();

        //현재 노드 출력
        DisplayCurrentNode();

        //선택한 자식 노드로 이동
        SelectAndGoNode();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Press Key Z");
        }

        Debug.Log("State is in Update");
    }


    void LoadDialogueFromCSV()
    {
        List<Dictionary<string, object>> data_Dialog = TestCSVReader.Read("Travel Around The World - CH0");

        //csv에 대사별 id 추가하면 CSVReader에서 수정하는 것 권장. 임시로 대사 id 설정
        int idx = 0;
        foreach (var row in data_Dialog)
        {
            string dayString = row["일자"].ToString();
            int day = int.Parse(System.Text.RegularExpressions.Regex.Match(dayString, @"\d+").Value);
            string location = row["장소"].ToString();
            string speaker = row["인물"].ToString();
            string line = row["대사"].ToString();
            string screenEffect = row["화면 연출"].ToString();
            string backgroundMusic = row["배경음악"].ToString();
            string expression = row["표정"].ToString();
            string note = row["비고"].ToString();
            string quest = row["퀘스트"].ToString();
            string questContent = row["퀘스트 내용"].ToString();

            dialogues.Add(idx, new ProDialogue(day, location, speaker, line, screenEffect, backgroundMusic, expression, note, quest, questContent));
            idx += 1;
        }
    }

    void SetDialogueTree()
    {

    }

    void StartDialogue()
    {

    }

    void DisplayCurrentNode()
    {

    }

    void SelectAndGoNode()
    {

    }

}
