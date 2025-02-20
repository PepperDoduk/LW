using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Audiomanager_prototype : MonoBehaviour
{
    public static Audiomanager_prototype instance;

    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume; 
    AudioSource bgmPlayer;
    int currentBgmIndex;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float[] sfxVolumes;  
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;  
    public string bgmMixerGroupName = "BGMVolume";
    public string sfxMixerGroupName = "SFXVolume";

    public Slider sfxVolumeSlider;
    public Slider bgmVolumeSlider;

    public enum Sfx
    {
        TankHit,
        TankDestroy,
        TankMove,
        EAGLE,
        EAGLEBGM,
        EAGLE_EXPLOSION,
        TitanFire,
        Titan35mm,
        TitanMissle,
        PKM,
        striker_explosion,
        Kord,
        Gun30mm,
        explosion30mm,
        MouseEnter,
        MouseClick,
        RadioOn,
        RadioEnd,
    }

    void Awake()
    {
        instance = this;
        Init();
    }

    void Start()
    {
        PlayBgm(0, true);
    }

    private void Update()
    {
        if(Time.frameCount % 20 == 0)
        {
            for (int i = 0; i < sfxVolumes.Length; i++)
            {
                sfxVolumes[i] = 100*sfxVolumeSlider.value;
                bgmVolume = 100*bgmVolumeSlider.value;
                SetBgmVolume(bgmVolume);
            }
        }
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
        }

        if (sfxVolumes == null || sfxVolumes.Length != sfxClips.Length)
        {
            sfxVolumes = new float[sfxClips.Length];
            for (int i = 0; i < sfxVolumes.Length; i++)
            {
                //sfxVolumes[i] = 100f; 
            }
            //sfxVolumes[9] = 20;
            //sfxVolumes[8] = 70;
            //sfxVolumes[7] = 50;
            //sfxVolumes[10] = 60;
            //sfxVolumes[11] = 60;
            //sfxVolumes[12] = 60;
            //sfxVolumes[15] = 80;
            //sfxVolumes[16] = 200;
        }

        SetBgmVolume(bgmVolume);
    }

    public void PlayBgm(int bgmIndex, bool isplay)
    {
        if (bgmIndex < 0 || bgmIndex >= bgmClips.Length)
        {
            Debug.LogWarning("Invalid BGM index.");
            return;
        }

        currentBgmIndex = bgmIndex;
        bgmPlayer.clip = bgmClips[bgmIndex];

        if (isplay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void StopBgm()
    {
        bgmPlayer.Stop();
    }

    public void PlaySfx(Sfx sfx)
    {
        bool isPlayed = false;

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            if (!sfxPlayers[index].isPlaying)
            {
                sfxPlayers[index].clip = sfxClips[(int)sfx];

                float volume = Mathf.Clamp(sfxVolumes[(int)sfx], 0f, 200f);
                sfxPlayers[index].volume = volume / 100f;

                sfxPlayers[index].Play();
                isPlayed = true;
                break;
            }
        }

        if (!isPlayed)
        {
            channelIndex = (channelIndex + 1) % sfxPlayers.Length;
            sfxPlayers[channelIndex].clip = sfxClips[(int)sfx];

            float volume = Mathf.Clamp(sfxVolumes[(int)sfx], 0f, 200f);
            sfxPlayers[channelIndex].volume = volume / 100f;

            sfxPlayers[channelIndex].Play();
        }
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0f, 200f);

        // AudioSource 볼륨을 직접 설정
        bgmPlayer.volume = bgmVolume / 100f;

        // 오디오 믹서의 볼륨도 설정
        float mixerVolume = Mathf.Log10(bgmVolume / 100f) * 20f;
        audioMixer.SetFloat(bgmMixerGroupName, mixerVolume);
    }


    public void SetSfxVolume(Sfx sfx, float volume)
    {
        sfxVolumes[(int)sfx] = Mathf.Clamp(volume, 0f, 200f);
    }
}
