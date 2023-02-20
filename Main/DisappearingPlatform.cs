using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private float duration = 3;
    [SerializeField] private float waitTime = 15;
    // frames needed to swap colours
    [SerializeField] private int framePassed = 60;
    private Color[] colours = {Color.green, Color.yellow, Color.red};
    private Color[] deathColours = {Color.white, Color.black};
    private Renderer obj;
    private int index = 0;
    private int i = 0;
    private bool once = false;
    private bool start = true;
    private bool canHit = true;
    private float startTime = -1;
    private void Start() {
        obj = GetComponent<Renderer>();
        StartCoroutine(resetHit());
    }
    private void Update() {
        if(index >= health) {
            if(!once) {
                startTime = Time.time;
                obj.material.color = deathColours[i];
                once = true;
            }
            
            // after death switch between black and white
            if(Time.frameCount % framePassed == 0){
                i = 1 - i;
            }
            obj.material.color = deathColours[i];
            // after x seconds remove the platform
            if(Time.time > startTime + duration){
                Destroy(gameObject);
            }
        }
    }
    // when landed on change colour to show stages of hits
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" && !InputManager.Instance.jumpHeld && canHit){ // other.gameObject.layer == LayerMask.NameToLayer("Player")  
            StartCoroutine(resetHit());
            obj.material.color = colours[++index >= colours.Length ? colours.Length-1 : index];
        }
    }

    IEnumerator resetHit(){
        canHit = false;
        if(start){
            yield return new WaitForSeconds(waitTime);
            start = false;
            canHit = true;
            yield break;
        }
        yield return new WaitForSeconds(0.16f);
        canHit = true;
    }
}
