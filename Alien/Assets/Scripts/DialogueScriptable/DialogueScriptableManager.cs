using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogueScriptableManager : MonoBehaviour
{

    [SerializeField] private List<NPCLinker> npcLinkers;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text continueText;
    [SerializeField] private TMP_Text speakerText;
    
    [SerializeField] private float charWriteDelay;
    [SerializeField] private KeyCode continueKey;

    [SerializeField] private GameObject responseBoxPrefab;
    [SerializeField] private Transform responseBoxParent;
    //[SerializeField] private Button responseButton;
    //[SerializeField] private TMP_Text responseButtonText;

    [SerializeField] private InteractManager interactManager;

    [SerializeField] private GameEvent UINeedsMouse;
    [SerializeField] private GameEvent dialogueStart;
    [SerializeField] private GameEvent UIDoneWithMouse;
    [SerializeField] private GameEvent dialogueOver;

    [SerializeField] private UnityEvent stopDialogue;


    //private Dictionary<int, Dialogue> dialogueDict;
    private bool currentlyWriting = false;
    private bool waitingForContinue = false;
    private bool waitingForResponse = false;
    private bool continueDown = false;
    //private int currentId;
    private IEnumerator writeTextCoroutine;
    private DialogueScriptable dialogueScriptable;
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

    public void StartDialogue(DialogueScriptable dialogue) {
        dialogueStart.TriggerEvent();
        dialogueScriptable = dialogue;
        //Debug.Log("set");
        dialogueBox.SetActive(true);
        lineIdx = -1;
        NextLine();
        //dialogueText.text = dialogueDict[currentId].message;
        //speakerText.text = dialogueDict[currentId].speaker;
        //writeTextCoroutine = WriteText();
        //StartCoroutine(writeTextCoroutine);
    }

    public void EndDialogue() {
        //Debug.Log("END");
        dialogueOver.TriggerEvent();
        UIDoneWithMouse.TriggerEvent();
        dialogueBox.SetActive(false);  
        interactManager.SetIsInteracting(false); 

        for (int i = 0; i < responseBoxParent.childCount; i++) {
            Destroy(responseBoxParent.GetChild(i).gameObject);
        }
    }

    private void ShowResponses() {
        RectTransform rectTrans = dialogueBox.GetComponent<RectTransform>();
        Vector3[] fourCornersArray = new Vector3[4];
        rectTrans.GetWorldCorners(fourCornersArray);
        float margin = 150;
        float offset = (fourCornersArray[2].x - fourCornersArray[0].x - margin * 2) / (dialogueScriptable.getDialogueResponses().Count - 1);
        //Debug.Log("get");
        
        for (int i = 0; i < dialogueScriptable.getDialogueResponses().Count; i++) {
            GameObject responseBoxInstance = Instantiate(responseBoxPrefab, responseBoxParent);
            responseBoxInstance.transform.position = new Vector3(fourCornersArray[0].x + margin + offset * i,  fourCornersArray[1].y + 150, responseBoxInstance.transform.position.z);
            responseBoxInstance.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = dialogueScriptable.getDialogueResponses()[i].getResponse();
            
            responseBoxInstance.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(action);
            //Debug.Log(rectTrans.rect.x);
        }
    }

    private void NextLine() {
        //Debug.Log("Called");
        lineIdx += 1;
        waitingForContinue = false;

        dialogueText.text = "";
        speakerText.text = "";
        continueText.text = "";

        if (lineIdx >= dialogueScriptable.getDialogueLines().Count) {
            EndDialogue();
            return;
        }

        

        dialogueText.text = dialogueScriptable.getDialogueLines()[lineIdx].getMessage();
        speakerText.text = dialogueScriptable.getDialogueLines()[lineIdx].getSpeakerId();
        writeTextCoroutine = WriteText();
        //StopCoroutine(writeTextCoroutine);
        StartCoroutine(writeTextCoroutine);

        waitingForResponse = lineIdx == dialogueScriptable.getDialogueLines().Count - 1 && dialogueScriptable.getDialogueResponses().Count > 0;

        if (waitingForResponse) {
            //Debug.Log("Showing responses");
            ShowResponses();
            UINeedsMouse.TriggerEvent();
            continueText.text = "Answer question to continue...";
        }

    }


    void Start()
    {
        action += EndDialogue;
        dialogueText.text = "";
        speakerText.text = "";
        continueText.text = "";


    }

    void Update()
    {
        GetInput();

        if (waitingForResponse) {

        }
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
            Debug.Log("Done writing text");
            dialogueText.text = "";
            dialogueText.text = dialogueScriptable.getDialogueLines()[lineIdx].getMessage();
            currentlyWriting = false;
            continueText.text = string.Format("Hit {0} to continue...", continueKey.ToString());
            waitingForContinue = true;
        }
    }
    

    IEnumerator WriteText() {
        Debug.Log("Writing text");
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

        dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        for (int i = 0; i < textInfo.characterCount; i++) {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = dialogueText.textInfo.meshInfo[meshIndex].colors32;

            vertexColors[vertexIndex].a = 255;
            vertexColors[vertexIndex + 1].a = 255;
            vertexColors[vertexIndex + 2].a = 255;
            vertexColors[vertexIndex + 3].a = 255;
            dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return new WaitForSeconds(charWriteDelay);
        }
        currentlyWriting = false;
        Debug.Log("Done writing text");
    }
}
