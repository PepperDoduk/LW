using UnityEngine;

public class EnemyTank_125mm : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        Fire125mm();
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        Vector3 movement = new Vector3(-30.0f, 0.0f, 0.0f);
        transform.Translate(movement* Time.deltaTime);
    }

    void Fire125mm()
    {

        Destroy(gameObject, 0.7f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Unit"))
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.TankHit);
            Destroy(gameObject);
        }
    }
}
