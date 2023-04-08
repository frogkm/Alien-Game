using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Shootable : MonoBehaviour
{

    public static GameObject[] defaultShrapnelPrefabs;

    [SerializeField] private float hitAnimationDuration = 0.2f;
    [SerializeField] private Renderer renderer;
    [SerializeField] private GameObject[] shrapnelPrefabs;
    [SerializeField] private float maxHealth = 15f;
    [SerializeField] private int numShrapnel = 50;
    

    private Color color; 
    private float hitTimer;
    private float health;


    void Start() {
        color = renderer.material.color;
        health = maxHealth;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullets")) {
            Hit();
            Destroy(collision.gameObject);
        }
    }


    private void Damage(float damage) {
        health -= damage;
        if (health <= 0f) {
            for (int i = 0; i < numShrapnel; i++) {
                GameObject shrapnel = Instantiate(shrapnelPrefabs[Random.Range(0, shrapnelPrefabs.Length)], renderer.transform.position, Random.rotation);
                shrapnel.GetComponent<Rigidbody>().velocity = (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized * 5f * Random.value;
                Destroy(shrapnel, 2f);
            }
            Destroy(gameObject, 0.2f);
        }
    }

    private void Hit() {
        renderer.material.color = Color.red;
        hitTimer = hitAnimationDuration;
        Damage(1f);
    }

    void Update() {
        if (hitTimer > 0) {
            hitTimer -= Time.deltaTime;
            
            renderer.material.color = Color.Lerp(color, Color.red, hitTimer / hitAnimationDuration);

            if (hitTimer < 0) {
                hitTimer = 0;
                renderer.material.color = color;
            }
            
        }
    }
}
