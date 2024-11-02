using UnityEngine;

public class Tank_115mm : MonoBehaviour
{
    public AudioSource audioSource;
    public ObjectPool objectPool;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        Vector3 movement = new Vector3(30.0f, 0.0f, 0.0f);
        transform.Translate(movement* Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.TankHit);
            //Destroy(gameObject);
            objectPool.ReturnToPoolAfterDelay(0);
            objectPool.Deactivate();
        }
    }
}
