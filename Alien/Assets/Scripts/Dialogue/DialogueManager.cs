using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text continueText;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private Image portraitImage;
    [SerializeField] private float charWriteDelay;
    [SerializeField] private KeyCode continueKey;
    [SerializeField] private GameObject responseBoxPrefab;
    [SerializeField] private Transform responseBoxParent;
    [SerializeField] private InteractManager interactManager;
    [SerializeField] private GameEvent UINeedsMouse;
    [SerializeField] private GameEvent dialogueStart;
    [SerializeField] private GameEvent UIDoneWithMouse;
    [SerializeField] private GameEvent dialogueOver;

    private bool currentlyWriting = false;
    private bool waitingForContinue = false;
    private bool waitingForResponse = false;
    private bool continueDown = false;
    private IEnumerator writeTextCoroutine;
    private Dialogue dialogue;
    private int lineIdx;
    private UnityAction action;

    private void GetInput() {
        if (Input.GetKeyDown(continueKey)) {
            continueDown = true;
        }
        else {
            continueDown = false;
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        dialogueStart.TriggerEvent();
        this.dialogue = dialogue;
        dialogueBox.SetActive(true);
        lineIdx = -1;
        NextLine();
    }

    public void EndDialogue() {
        dialogueOver.TriggerEvent();
        UIDoneWithMouse.TriggerEvent();
        dialogueBox.SetActive(false);
        interactManager.SetIsInteracting(false);

        dialogue.SetFlags();   

        for (int i = 0; i < responseBoxParent.childCount; i++) {
            Destroy(responseBoxParent.GetChild(i).gameObject);
        }
    }

    public void NextDialogue() {

        dialogue.SetFlags();

        
        for (int i = 0; i < responseBoxParent.childCount; i++) {
            Destroy(responseBoxParent.GetChild(i).gameObject);
        }
    }

    private void ResponseSelected(Dialogue dialogue) {
        NextDialogue();
        //interactManager.SetIsInteracting(true);
        StartDialogue(dialogue);
    }

    private void ShowResponses() {
        RectTransform rectTrans = dialogueBox.GetComponent<RectTransform>();
        Vector3[] fourCornersArray = new Vector3[4];
        rectTrans.GetWorldCorners(fourCornersArray);
        float margin = 150;
        float offset = (fourCornersArray[2].x - fourCornersArray[0].x - margin * 2) / (dialogue.getDialogueResponses().Count - 1);
        //Debug.Log("get");
        
        for (int i = 0; i < dialogue.getDialogueResponses().Count; i++) {
            GameObject responseBoxInstance = Instantiate(responseBoxPrefab, responseBoxParent);
            responseBoxInstance.transform.position = new Vector3(fourCornersArray[0].x + margin + offset * i,  fourCornersArray[1].y + 150, responseBoxInstance.transform.position.z);
            responseBoxInstance.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = dialogue.getDialogueResponses()[i].getResponse();
            //UnityAction<DialogueScriptable> action;
            //action += ResponseSelected;
            //responseBoxInstance.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(action);
            Dialogue response = dialogue.getDialogueResponses()[i].getResponseDialogue();
            responseBoxInstance.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate{ResponseSelected(response);});
            //delegate{ResponseSelected(dialogueScriptable.getDialogueResponses()[i].getResponseDialogue());}
            //ResponseSelected(DialogueScriptable dialogue)
        }
    }

    private void NextLine() {
        lineIdx += 1;
        waitingForContinue = false;

        dialogueText.text = "";
        speakerText.text = "";
        continueText.text = ""; 

        if (lineIdx >= dialogue.getDialogueLines().Count) {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogue.getDialogueLines()[lineIdx].getMessage();
        speakerText.text = dialogue.getDialogueLines()[lineIdx].getSpeaker().displayName;
        portraitImage.sprite = dialogue.getDialogueLines()[lineIdx].getSpeaker().portrait;
        writeTextCoroutine = WriteText();
        StartCoroutine(writeTextCoroutine);

        waitingForResponse = lineIdx == dialogue.getDialogueLines().Count - 1 && dialogue.getDialogueResponses().Count > 0;
        if (waitingForResponse) {
            ShowResponses();
            UINeedsMouse.TriggerEvent();
            continueText.text = "Answer question to continue...";
        }
    }

    void Start()
    {
        //action += EndDialogue;
        dialogueText.text = "";
        speakerText.text = "";
        continueText.text = "";
    }

    void Update()
    {
        GetInput();

        if (waitingForResponse) {}
        else if (dialogueBox.activeSelf && !currentlyWriting && !waitingForContinue) {
            waitingForContinue = true;
            continueText.text = string.Format("Hit {0} to continue...", continueKey.ToString());
        }
        else if (waitingForContinue && continueDown) {
            NextLine();
        }
        else if(currentlyWriting && continueDown) {
            //Debug.Log("Skip animation");
            StopCoroutine(writeTextCoroutine);
            //Debug.Log("Done writing text");
            dialogueText.text = "";
            dialogueText.text = dialogue.getDialogueLines()[lineIdx].getMessage();
            currentlyWriting = false;
            continueText.text = string.Format("Hit {0} to continue...", continueKey.ToString());
            waitingForContinue = true;
        }
    }

    IEnumerator WriteText() {
        currentlyWriting = true;
        string message = dialogueText.text;

        for (int i = 0; i < message.Length + 1; i++) {
            dialogueText.text = message.Substring(0, i) + "<alpha=#00>" + message.Substring(i, message.Length - i);
            
            yield return new WaitForSeconds(charWriteDelay);
        }
        currentlyWriting = false;
    }
    
    /*
    IEnumerator WriteText() {
        //Debug.Log("Writing text");
        currentlyWriting = true;
        dialogueText.ForceMeshUpdate();
        TMP_TextInfo textInfo = dialogueText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++) {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = dialogueText.textInfo.meshInfo[meshIndex].colors32;

            vertexColors[vertexIndex].a = 0;
            vertexColors[vertexIndex + 1].a = 0;
            vertexColors[vertexIndex + 2].a = 0;
            vertexColors[vertexIndex + 3].a = 0;
        }
        //Debug.Log("Invis");

        dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        string builder = "";

        for (int i = 0; i < textInfo.characterCount; i++) {
            //Debug.Log(string.Format("{0}/{1}", i, textInfo.characterCount));
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = dialogueText.textInfo.meshInfo[meshIndex].colors32;

            vertexColors[vertexIndex].a = 255;
            vertexColors[vertexIndex + 1].a = 255;
            vertexColors[vertexIndex + 2].a = 255;
            vertexColors[vertexIndex + 3].a = 255;

            dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            //Debug.Log();
            //builder += dialogueText.m_input_CharArray[i];
            //Debug.Log(dialogueText.m_input_CharArray[i]);
            yield return new WaitForSeconds(charWriteDelay);
        }
        //Debug.Log(builder);
        currentlyWriting = false;
        //Debug.Log("Done writing text");
    }
    */
}
