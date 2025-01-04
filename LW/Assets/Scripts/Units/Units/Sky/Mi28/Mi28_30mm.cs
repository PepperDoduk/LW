using UnityEngine;
using System.Collections;

public class Mi28_30mm : MonoBehaviour
{
    public Mi28 mi28;

    private bool is30mmFireing;
    public IEnumerator Fire30mm()
    {
        if (!is30mmFireing)
        {
            is30mmFireing = true;
            Audiomanager_prototype.instance.PlaySfx(Audiomanager_prototype.Sfx.Gun30mm);
            for (int i = 0; i < 20; ++i)
            {
                mi28.ammo--;

                GameObject muzzleFlash = ObjectPoolManager.Instance.GetObjectFromPool(mi28.muzzleFlashPrefab, Quaternion.identity, mi28.sizeOfMuzzleFlash);
                muzzleFlash.transform.position = transform.position + mi28.locationOfMuzzleFlash;
                muzzleFlash.transform.rotation = transform.rotation;

                GameObject bullet = ObjectPoolManager.Instance.GetObjectFromPool(mi28.bulletPrefab, Quaternion.identity, mi28.sizeOfBullet);
                bullet.transform.position = transform.position + mi28.locationOfBullet;
                bullet.transform.rotation = transform.rotation;
                yield return new WaitForSeconds(0.085f);
            }
            yield return new WaitForSeconds(5f);
            is30mmFireing = false;
        }
    }
}
