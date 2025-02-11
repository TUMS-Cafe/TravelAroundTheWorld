using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string 일자;
    public string 장소;
    public string 인물;
    public string 대사;
    public string 화면_연출;
    public string 배경음악;
    public string 표정;
    public string 비고;
    public string 퀘스트;
    public string 퀘스트_내용;
    public string NodeID;
    public string NextNodeID;
    public string PlaceTag;
    public string DayTag;
}

[System.Serializable]
public class DialogueList
{
    public List<Dialogue> dialogues;
}
