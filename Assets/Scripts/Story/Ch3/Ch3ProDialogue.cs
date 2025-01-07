using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch3ProDialogue
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

    public Ch3ProDialogue(int day, string location, string speaker, string line, string screenEffect, string backgroundMusic, string expression, string note, string quest, string questContent = "")
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
