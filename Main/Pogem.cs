using UnityEngine.Audio;
using UnityEngine;

public class Pogem : MonoBehaviour
{
    Transform tf;
    float dy = 0;
    private int value = 1;
    private int maxValue = 50;
    float angle = 0;
    Vector3 rot;
    public AudioClip collect;
    public PogoStickPhysics player;
    private float spawnYPos;
    public float speed = 0.03f;
    public float height = 0.15f;
    ParticleSystem pSystem;
    ParticleSystemRenderer particleSystemRenderer;
    Renderer pogemColour;
    public bool hardToGet;
    public Material[] materials;
    public Material maxMat;

    private void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
        particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
        // transform of pogem
        tf = transform;

        if (value > maxValue)
        {
            value = maxValue;
        }
        // rotation speed
        rot = new Vector3(0, 0, 90);
        spawnYPos = transform.position.y;
        player = FindObjectOfType<PogoStickPhysics>();
        
        value = Random.Range(10, 30);
        pogemColour = GetComponent<Renderer>();
        int mappedVal = (int)Mathf.Round(Map(value, 10, 30, 0, materials.Length-1));
        pogemColour.material = materials[mappedVal];
        particleSystemRenderer.material = materials[mappedVal];
        if(hardToGet){
            value = maxValue;
            pogemColour.material = maxMat;
            particleSystemRenderer.material = maxMat;
        }
    }
    private void Update()
    {
        // depending on value change the colour 
        // rotate 
        tf.Rotate(rot * Time.deltaTime);
        // bob up and down
        dy = height * Mathf.Sin(angle);
        tf.position = new Vector3(tf.position.x, dy + spawnYPos, tf.position.z);
        angle += speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // make it invisible before destroying to play effect
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            // play effect
            AudioSource s = GetComponent<AudioSource>();
            s.PlayOneShot(collect);
            // add value 
            player.total += value;
            pSystem.Play();
            Destroy(gameObject, 2f);
        }
    }
    private float Map(float value, float low1, float high1, float low2, float high2) { return low2 + (value - low1) * (high2 - low2) / (high1 - low1); }
}
