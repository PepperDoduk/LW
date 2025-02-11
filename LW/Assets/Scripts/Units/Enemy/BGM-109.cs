using UnityEngine;

public class BGM_109 : MonoBehaviour
{
    public float moveSpeed;

    [SerializeField] private float duration = 2f;
    private Quaternion targetRotation;

    [SerializeField] private GameObject smokePrefab;
    void Start()
    {
        
    }

    void Update()
    {
        if(Time.frameCount % 30 == 0)
        {
            GameObject smoke = ObjectPoolManager.Instance.GetObjectFromPool(this.smokePrefab, Quaternion.identity, new Vector3(1,1,1));
            smoke.transform.position = transform.position;
        }

        Vector3 moveDirection = transform.up;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * duration);
    }

    void Launch()
    {
        moveSpeed = 50;
        targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
