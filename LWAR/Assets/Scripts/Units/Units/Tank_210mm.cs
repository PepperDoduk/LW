using UnityEngine;

public class Tank_210mm : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        Fire210mm();
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        Vector3 movement = new Vector3(65.0f, 0.0f, 0.0f);
        transform.Translate(movement* Time.deltaTime);
    }

    void Fire210mm()
    {

        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.TankHit);
            Destroy(gameObject);
        }
    }
}
