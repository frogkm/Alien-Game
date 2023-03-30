using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractManager : MonoBehaviour
{

    [SerializeField] private GameObject interactBox;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private float interactCooldown;
    [SerializeField] private float interactRange;

    [SerializeField] private CapsuleCollider interactCollider;
    [SerializeField] private Transform playerTrans;
    [SerializeField] private Transform camTrans;
    
    private bool anyInteracts = false;
    private bool isInteracting = false;
    private bool canInteract = true;
    private float cooldownTimer;

    private RaycastHit[] interactHits;

    private float realRange;



    public void Interact(Interactable interactable) {
        if (!isInteracting) {
            SetIsInteracting(true);
            interactBox.SetActive(false);
            interactable.Interact();
            canInteract = false;
        }
    }

    public void SetIsInteracting(bool isInteracting) {
        if (!isInteracting && this.isInteracting) {
            cooldownTimer = interactCooldown;
        }
        this.isInteracting = isInteracting;
        //interactBox.SetActive(!isInteracting);
        //canInteract = !isInteracting;
        //cooldownTimer = 0f;
        //canInteract = !isInteracting;
        
    }

    public void PromptInteract(Interactable interactable) {
        interactText.text = interactable.getInteractText(interactKey);
        interactBox.SetActive(true);
        anyInteracts = true;

        if (!isInteracting && Input.GetKeyDown(interactKey)) {
            Interact(interactable);
        }
    }

    void Update() {
        if (!anyInteracts) {
            interactBox.SetActive(false);
            interactText.text = "";
        }

        anyInteracts = false;

        if (cooldownTimer > 0) {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0) {
                cooldownTimer = 0;
                canInteract = true;
            } 
        }

        for (int i = 0; i < interactHits.Length; i++)
        {
           // Debug.Log("hit");
            RaycastHit hit = interactHits[i];
            Interactable interactable = hit.transform.GetComponent<Interactable>();
            if (interactable != null && canInteract) {
                PromptInteract(interactable);
                //interactableSpotted.TriggerEvent();
            }
        }
        
    }

    void Start() {
        SetupInteractMath();
    }

    private void SetupInteractMath() {
        realRange = interactRange - interactCollider.radius * 2f;
        if (realRange < 0) {
            realRange = 0;
        }
    }

    private void CheckInteracts() {
        Vector3 castDir = (interactCollider.transform.position - camTrans.position).normalized;
        Vector3 p1 = camTrans.position;
        Vector3 p2 = camTrans.position + castDir * realRange;

        interactHits = Physics.CapsuleCastAll(p1, p2, interactCollider.radius, p2 - p1, realRange);
        //Debug.DrawRay(p1, castDir * interactRange, Color.red);
    }
    

    void FixedUpdate() {
        CheckInteracts();
    }
}
