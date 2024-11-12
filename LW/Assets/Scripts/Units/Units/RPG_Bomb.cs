using UnityEngine;

public class Rpg_Bomb : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        FireRPG();
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        Vector3 movement = new Vector3(25.0f, 0.0f, 0.0f);
        transform.Translate(movement* Time.deltaTime);
    }

    void FireRPG()
    {

        Destroy(gameObject, 0.7f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankHit);
            Destroy(gameObject);
        }
    }
}
