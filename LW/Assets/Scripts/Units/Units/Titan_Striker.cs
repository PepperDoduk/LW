using UnityEngine;

public class Titan_Striker : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        Fire35mm();
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        Vector3 movement = new Vector3(80.0f, 0.0f, 0.0f);
        transform.Translate(movement* Time.deltaTime);
    }

    void Fire35mm()
    {
        Destroy(gameObject, 1f);
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
