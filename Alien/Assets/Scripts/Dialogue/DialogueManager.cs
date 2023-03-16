using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;



public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextAsset jsonFile;
    [SerializeField] private List<NPCLinker> npcLinkers;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text continueText;
    [SerializeField] private float charWriteDelay;
    [SerializeField] private KeyCode continueKey;

    private Dictionary<int, Dialogue> dialogueDict;
    private bool currentlyWriting = false;
    private bool waitingForContinue = false;
    private bool continueDown = false;
    private int currentId;
    private IEnumerator writeTextCoroutine;

    void Start()
    {
        continueText.text = "";
        dialogueText.text = "";
        
        dialogueDict = DialogueLoader.loadDialogue(jsonFile);
        StartConversation(1);   
    }

    private void printDict() {
        foreach (KeyValuePair<int, Dialogue> keyVal in dialogueDict) {
            Debug.Log(string.Format("{0}: {1}, by {2}", keyVal.Key, keyVal.Value.message, keyVal.Value.speaker));
        }
    }

    public void StartConversation(int id) {
        dialogueBox.SetActive(true);
        currentlyWriting = true;
        currentId = id;
        dialogueText.text = dialogueDict[currentId].message;
        writeTextCoroutine = WriteText();
        StartCoroutine(writeTextCoroutine);
    }
    void Update()
    {

        if (Input.GetKeyDown(continueKey)) {
            continueDown = true;
        }
        else {
            continueDown = false;
        }


        if (dialogueBox.activeSelf && !currentlyWriting && !waitingForContinue) {
            waitingForContinue = true;
            continueText.text = string.Format("Hit {0} to continue...", continueKey.ToString());
        }
        else if (waitingForContinue && continueDown) {
            waitingForContinue = false;
            continueText.text = "";
            DialogueMessage dialogueMessage = (DialogueMessage) dialogueDict[currentId];
            if (dialogueMessage.next_id != -1) {
                currentlyWriting = true;
                currentId = dialogueMessage.next_id;
                dialogueText.text = dialogueDict[currentId].message;
                writeTextCoroutine = WriteText();
                StartCoroutine(writeTextCoroutine);
            }
            else {
                dialogueBox.SetActive(false);
            }

        }
        else if(currentlyWriting && continueDown) {
            StopCoroutine(writeTextCoroutine);
            dialogueText.text = "";
            dialogueText.text = dialogueDict[currentId].message;
            currentlyWriting = false;
            continueText.text = string.Format("Hit {0} to continue...", continueKey.ToString());
            waitingForContinue = true;
            dialogueText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }

    IEnumerator WriteText() {
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
    }
}
