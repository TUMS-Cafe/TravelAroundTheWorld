using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteract : MonoBehaviour
{
    public GameObject interactionButton;
    public GameObject dialogueUI;
    public Button dialogueButton;
    public Transform playerTransform;
    public float interactionDistance = 1.0f;
    private bool isTalking = false;
    private bool isFound = false;

    public HideandSeek gameManager;

    private void Start()
    {
        interactionButton.SetActive(false);
        dialogueUI.SetActive(false);

        Button button = interactionButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(StartDialogue);
        }

        if (dialogueButton != null)
        {
            dialogueButton.onClick.AddListener(EndDialogue);
        }
    }

    private void Update()
    {
        if (isTalking || isFound)
        {
            return;
        }

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (distance <= interactionDistance)
        {
            interactionButton.SetActive(true);
        }
        else
        {
            interactionButton.SetActive(false);
        }

        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            EndDialogue();
        }
    }

    private void StartDialogue()
    {
        if (isFound) return;

        isTalking = true;
        interactionButton.SetActive(false);
        dialogueUI.SetActive(true);
    }

    public void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);

        if (!isFound)
        {
            isFound = true;
            if (gameManager != null)
            {
                gameManager.FindNpc();
            }
        }
    }
}