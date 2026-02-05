using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public AudioSource mainThrusterAudio;
    public AudioSource sideThruster;
    public AudioSource crashAudio;
    public AudioSource collectionBeamAudio;
    public AudioSource uiAudio;

    [Header("Audio clips")]
    public AudioClip[] crashes;
    public AudioClip shipMainThruster;
    public AudioClip gasSideThruster;
    public AudioClip collectionBeam;
    public AudioClip sellingItem;
    public AudioClip upgradeBuy;
    public AudioClip refillFuel;
    public AudioClip repair;
    public AudioClip deny;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mainThrusterAudio != null)
        {
            mainThrusterAudio.playOnAwake = false;
        }

        if (crashAudio != null)
        {
            crashAudio.playOnAwake = false;
        }

        if (collectionBeamAudio != null)
        {
            collectionBeamAudio.playOnAwake = false;
        }

        if (uiAudio != null)
        {
            uiAudio.playOnAwake = false;
        }

        mainThrusterAudio.volume = mainThrusterAudio.volume * (GameManager.Instance.volume / 100);
        sideThruster.volume = sideThruster.volume * (GameManager.Instance.volume / 100);
        crashAudio.volume = crashAudio.volume * (GameManager.Instance.volume / 100);
        collectionBeamAudio.volume = collectionBeamAudio.volume * (GameManager.Instance.volume / 100);
        uiAudio.volume = uiAudio.volume * (GameManager.Instance.volume / 100);
    }

    public void PlayCrashSound()
    {
        if (crashAudio != null && crashes.Length > 0)
        {
            int index = Random.Range(0, crashes.Length);
            crashAudio.PlayOneShot(crashes[index]);
        }
    }

    public void PlayThruster()
    {
        if (mainThrusterAudio != null && shipMainThruster != null)
        {
            mainThrusterAudio.clip = shipMainThruster;
            mainThrusterAudio.Play();
            mainThrusterAudio.loop = true;
        }
    }

    public void StopThruster()
    {
        if (mainThrusterAudio != null)
        {
            mainThrusterAudio.Stop();
            mainThrusterAudio.clip = null;
            mainThrusterAudio.loop = false;
        }
    }

    public void PlaySideThruster()
    {
        if (sideThruster != null && gasSideThruster != null && !sideThruster.isPlaying)
        {
            sideThruster.clip = gasSideThruster;
            sideThruster.Play();
            sideThruster.loop = true;
        }
    }

    public void StopSideThruster()
    {
        if (sideThruster != null)
        {
            sideThruster.Stop();
            sideThruster.clip = null;
            sideThruster.loop = false;
        }
    }

    public void PlayCollectionBeam()
    {
        if (collectionBeamAudio != null && collectionBeam != null)
        {
            collectionBeamAudio.clip = collectionBeam;
            collectionBeamAudio.Play();
            collectionBeamAudio.loop = true;
        }
    }

    public void StopCollectionBeam()
    {
        if (collectionBeamAudio != null)
        {
            collectionBeamAudio.clip = null;
            collectionBeamAudio.Stop();
            collectionBeamAudio.loop = false;
        }
    }

    public void PlayerSellingItem()
    {
        if (uiAudio != null && sellingItem != null)
        {
            uiAudio.PlayOneShot(sellingItem);
        }
    }

    public void PlayUpgradeBuy()
    {
        if (uiAudio != null && upgradeBuy != null)
        {
            uiAudio.PlayOneShot(upgradeBuy);
        }
    }

    public void PlayFuelBuy()
    {
        if (uiAudio != null && refillFuel != null)
        {
            uiAudio.clip = refillFuel;
            uiAudio.Play();
            uiAudio.loop = false;
        }
    }

    public void StopFuelBuy()
    {
        if (uiAudio != null)
        {
            uiAudio.Stop();
            uiAudio.clip = null;
            uiAudio.loop = false;
        }
    }

    public void PlayerRepair()
    {
        if (uiAudio != null && repair != null)
        {
            uiAudio.PlayOneShot(repair);
        }
    }

    public void PlayDeny()
    {
        if (uiAudio != null && deny != null)
        {
            uiAudio.PlayOneShot(deny);
        }
    }

}
