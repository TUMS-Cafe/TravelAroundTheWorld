using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDialogue
{
    public int day;
    public string location;
    public string speaker;
    public string line;
    public string screenEffect;
    public string backgroundMusic;
    public string expression;
    public string note;
    public string quest;
    public string questContent;
    public int nodeId;
    public int nextNodeId;
    public string placeTag;
    public string dayTag;

    public StoryDialogue(int day, string location, string speaker, string line, string screenEffect, string backgroundMusic, string expression, string note, string quest, string questContent = "", int nodeId = 0, int nextNodeId = 0, string placeTag = "", string dayTag = "")
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
        this.nodeId = nodeId;
        this.nextNodeId = nextNodeId;
        this.placeTag = placeTag;
        this.dayTag = dayTag;
    }
}
