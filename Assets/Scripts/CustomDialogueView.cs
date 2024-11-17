using System;
using System.Collections;
using UnityEngine;
using TMPro; // For TextMeshPro
using UnityEngine.UI; // For Button UI
using Yarn.Unity; // Yarn Spinner namespace

public class CustomDialogueView : DialogueViewBase
{
    DialogueRunner runner;
    
    [Header("Dialogue UI Elements")]
    public TMP_Text dialogueText; // The text field to display dialogue
    public GameObject choicesPanel; // Panel to hold choice buttons
    public Button choiceButtonPrefab; // Button prefab for dialogue choices

    private bool isWaitingForContinue = false;

    private void Start()
    {
        runner.StartDialogue("100");
        
    }


    // Override RunLine to display dialogue lines
    public override void RunLine(LocalizedLine dialogueLine, System.Action onComplete)
    {
        // Start displaying the dialogue line with a typewriter effect
        StartCoroutine(DisplayLine(dialogueLine.Text.Text, onComplete));
    }

    private IEnumerator DisplayLine(string line, System.Action onComplete)
    {
        dialogueText.text = ""; // Clear the dialogue text
        foreach (char letter in line)
        {
            dialogueText.text += letter; // Add one character at a time
            yield return new WaitForSeconds(0.05f); // Delay for typewriter effect
        }

        // Wait for the player to continue (e.g., by pressing Space)
        isWaitingForContinue = true;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        isWaitingForContinue = false;

        // Invoke the onComplete callback to continue dialogue
        onComplete?.Invoke();
    }

    // Override RunOptions to handle choices
    public override void RunOptions(DialogueOption[] dialogueOptions, System.Action<int> onOptionSelected)
    {
        // Display the choices
        choicesPanel.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create a button for each choice
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            var option = dialogueOptions[i];
            var button = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            button.GetComponentInChildren<TMP_Text>().text = option.Line.Text.Text;

            // Capture the index of the option
            int optionIndex = i;
            button.onClick.AddListener(() =>
            {
                onOptionSelected(optionIndex); // Notify Yarn Spinner of the selected option
                choicesPanel.SetActive(false); // Hide the choices panel
            });
        }
    }

    // Override DialogueComplete to handle end of dialogue
    public override void DialogueComplete()
    {
        Debug.Log("Dialogue is complete!");
        // Optionally hide the dialogue UI
    }

    // Override DialogueStarted to handle when dialogue begins
    public override void DialogueStarted()
    {
        Debug.Log("Dialogue started!");
        dialogueText.text = ""; // Clear the text field
        choicesPanel.SetActive(false); // Hide choices at the start
    }

  
}
