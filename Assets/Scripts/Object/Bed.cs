
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bed : MonoBehaviour
{
    private GameObject player;
    private PlayerAnimationController playerAnimationController;

    public GameObject bedNarration;
    public TalkManager talkManager;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerAnimationController = player.GetComponent<PlayerAnimationController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bedNarration.SetActive(true);
            UIManager.Instance.ToggleUI("Bed");
            if(!talkManager.isAllNPCActivated && talkManager.currentDialogueIndex == 173)
            {
                player.GetComponent<PlayerController>().StartMove();
            }
            else
            {
                player.GetComponent<PlayerController>().StopMove();
            }
        }
    }

}
