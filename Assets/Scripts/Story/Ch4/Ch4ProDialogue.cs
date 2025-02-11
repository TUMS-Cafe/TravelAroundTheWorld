using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch4ProDialogue
{
    public string time;
    public string location;
    public string speaker;
    public string line;
    public string screenEffect;
    public string backgroundMusic;
    public string expression;
    public string note;

    public Ch4ProDialogue(string time, string location, string speaker, string line, string screenEffect, string backgroundMusic, string expression, string note)
    {
        this.time = time;
        this.location = location;
        this.speaker = speaker;
        this.line = line;
        this.screenEffect = screenEffect;
        this.backgroundMusic = backgroundMusic;
        this.expression = expression;
        this.note = note;
    }
}

