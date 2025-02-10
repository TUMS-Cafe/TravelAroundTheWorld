using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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

    [Header("Dialogue Settings")]
    public TextMeshProUGUI dialogueText;
    public DialogueData dialogueData;

    [Header("Player Reference")]
    public PlayerInput playerInput;

    private int currentDialogueIndex = 0;
    private bool isTyping = false;

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
            dialogueButton.onClick.AddListener(OnDialogueButtonClick);
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
    }

    private void StartDialogue()
    {
        if (isFound || dialogueData == null || dialogueData.dialogues.Count == 0) return;

        isTalking = true;
        interactionButton.SetActive(false);
        dialogueUI.SetActive(true);

        if (playerInput != null)
        {
            playerInput.DeactivateInput();
        }

        currentDialogueIndex = 0;
        StartCoroutine(TypeDialogue());
    }

    private IEnumerator TypeDialogue()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogueData.dialogues[currentDialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
    }

    private void OnDialogueButtonClick()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = dialogueData.dialogues[currentDialogueIndex];
            isTyping = false;
        }
        else
        {
            currentDialogueIndex++;

            if (currentDialogueIndex < dialogueData.dialogues.Count)
            {
                StartCoroutine(TypeDialogue());
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);

        if (playerInput != null)
        {
            playerInput.ActivateInput();
        }

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
