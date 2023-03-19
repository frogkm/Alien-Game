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
    

    private bool anyInteracts = false;
    private bool isInteracting = false;
    private bool canInteract = true;

    private float cooldownTimer;


    private void Interact(Interactable interactable) {
        if (!isInteracting) {
            SetIsInteracting(true);
            interactBox.SetActive(false);
            interactable.Interact();
            canInteract = false;
        }
    }

    public void SetIsInteracting(bool interacting) {
        if (!interacting && isInteracting) {
            cooldownTimer = interactCooldown;
        }
        isInteracting = interacting;
        
    }



    public void PromptInteract(Interactable interactable) { 
        if (canInteract) {
            interactText.text = string.Format("Hit {0} to interact", interactKey.ToString());
            interactBox.SetActive(true);
            anyInteracts = true;

            if (Input.GetKeyDown(interactKey)) {
                Interact(interactable);
            }
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
        
    }

    

    void FixedUpdate() {
        
    }
}
