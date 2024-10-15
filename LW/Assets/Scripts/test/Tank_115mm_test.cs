using UnityEngine;

public class Tank_115mm_test : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        Fire115mm();
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        Vector3 movement = new Vector3(30.0f, 0.0f, 0.0f);
        transform.Translate(movement* Time.deltaTime);
    }

    void Fire115mm()
    {

        Destroy(gameObject, 0.7f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ÇÇÅº");
        if (other.gameObject.CompareTag("Enemy"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.TankHit);
            Destroy(gameObject);
        }
    }
}
