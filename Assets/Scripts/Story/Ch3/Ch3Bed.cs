
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ch3Bed : MonoBehaviour
{
    private GameObject player;
    private PlayerAnimationController playerAnimationController;

    public GameObject bedNarration;

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
            //UIManager.Instance.ToggleUI("Bed");
            player.GetComponent<PlayerController>().StopMove();
        }
    }
}
