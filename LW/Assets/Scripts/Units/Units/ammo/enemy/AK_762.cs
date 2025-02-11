using UnityEngine;

public class AK_762 : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        FireAK();
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        Vector3 movement = new Vector3(40.0f, 0.0f, 0.0f);
        transform.Translate(movement* Time.deltaTime);
    }

    void FireAK()
    {

        Destroy(gameObject, 0.6f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //AudioManager.instance.PlaySfx(AudioManager.Sfx.TankHit);
            Destroy(gameObject);
        }
    }
}
