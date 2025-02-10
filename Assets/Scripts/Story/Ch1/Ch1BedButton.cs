using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch1BedButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Ch1TalkManager ch1TalkManager;
    public GameObject bedUi;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickYesButton()
    {
        bedUi.SetActive(false);
        ch1TalkManager.player.SetActive(false);
        ch1TalkManager.map.SetActive(false);
        ch1TalkManager.cafe.SetActive(false);
        ch1TalkManager.Npc_Rayviyak.SetActive(false);
        ch1TalkManager.Npc_MrHam.SetActive(false);
        ch1TalkManager.Npc_Rusk.SetActive(false);
        ch1TalkManager.Npc_Violet.SetActive(false);
        ch1TalkManager.isWaitingForPlayer = false;

        if (ch1TalkManager.currentDialogueIndex == 69)
        {
            ch1TalkManager.currentDialogueIndex = 109;
            ch1TalkManager.PrintCh1ProDialogue(ch1TalkManager.currentDialogueIndex);
        }
        else if (ch1TalkManager.currentDialogueIndex == 169)
        {
            ch1TalkManager.currentDialogueIndex = 181;
            ch1TalkManager.PrintCh1ProDialogue(ch1TalkManager.currentDialogueIndex);
        }
        else if (ch1TalkManager.currentDialogueIndex == 226)
        {
            ch1TalkManager.currentDialogueIndex = 240;
            ch1TalkManager.PrintCh1ProDialogue(ch1TalkManager.currentDialogueIndex);
        }
        else if (ch1TalkManager.currentDialogueIndex == 302)
        {
            ch1TalkManager.currentDialogueIndex = 315;
            ch1TalkManager.PrintCh1ProDialogue(ch1TalkManager.currentDialogueIndex);
        }
        else if (ch1TalkManager.currentDialogueIndex == 363)
        {
            ch1TalkManager.currentDialogueIndex = 375;
            ch1TalkManager.PrintCh1ProDialogue(ch1TalkManager.currentDialogueIndex);
        }
        else if (ch1TalkManager.currentDialogueIndex == 582)
        {
            ch1TalkManager.currentDialogueIndex = 595;
            ch1TalkManager.PrintCh1ProDialogue(ch1TalkManager.currentDialogueIndex);
        }
    }
}
