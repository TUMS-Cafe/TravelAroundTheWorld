using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunc : MonoBehaviour
{
    [SerializeField] TalkManager talkManager;

    public void OnAnimationEnd()
    {
        talkManager.isAnimationPlaying = false;

        //talkManager.currentDialogueIndex += 1;
        SoundManager.Instance.PlaySFX("twinkle");
        talkManager.invitationText.gameObject.SetActive(true);
    }

    public void OnTrainAnimationEnd()
    {
        talkManager.isAnimationPlaying = false;
    }
}
