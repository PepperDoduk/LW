using UnityEngine;
using System.Collections;

public class AH6430mm : MonoBehaviour
{
    public AH64 ah64;
    public GameObject AH64;

    private bool is30mmFireing;
    public IEnumerator Fire30mm()
    {
        if (!is30mmFireing)
        {
            is30mmFireing = true;
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.Gun30mm);
            for (int i = 0; i < 20; ++i)
            {
                ah64.ammo--;

                GameObject muzzleFlash = ObjectPoolManager.Instance.GetObjectFromPool(ah64.muzzleFlashPrefab, Quaternion.identity, ah64.sizeOfMuzzleFlash);
                muzzleFlash.transform.position = transform.position + ah64.locationOfMuzzleFlash;
                muzzleFlash.transform.rotation = Quaternion.Euler(0, 180, transform.rotation.z);

                GameObject bullet = ObjectPoolManager.Instance.GetObjectFromPool(ah64.bulletPrefab, Quaternion.identity, ah64.sizeOfBullet);
                bullet.transform.position = transform.position + ah64.locationOfBullet;
                bullet.transform.rotation = transform.rotation;
                yield return new WaitForSeconds(0.085f);
            }
            yield return new WaitForSeconds(6f);
            is30mmFireing = false;
        }
    }
}
