using UnityEngine;
using UnityEngine.Audio;

public class Audiomanager_prototype : MonoBehaviour
{
    public static Audiomanager_prototype instance;

    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;  // 0 ~ 200 범위
    AudioSource bgmPlayer;
    int currentBgmIndex;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float[] sfxVolumes;  // 효과음마다 개별 볼륨 저장
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;  // Audio Mixer 연결
    public string bgmMixerGroupName = "BGMVolume";
    public string sfxMixerGroupName = "SFXVolume";

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
        BULLET
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

        // 효과음 볼륨 배열 초기화 (효과음 개수와 동일해야 함)
        if (sfxVolumes == null || sfxVolumes.Length != sfxClips.Length)
        {
            sfxVolumes = new float[sfxClips.Length];
            for (int i = 0; i < sfxVolumes.Length; i++)
            {
                sfxVolumes[i] = 100f;  // 기본적으로 모든 효과음을 100%로 설정
            }
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
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            if (!sfxPlayers[index].isPlaying)
            {
                sfxPlayers[index].clip = sfxClips[(int)sfx];

                // sfxVolumes 배열에서 해당 효과음의 볼륨을 가져와서 설정
                float volume = Mathf.Clamp(sfxVolumes[(int)sfx], 0f, 200f);
                sfxPlayers[index].volume = volume / 100f;  // 0~200 값을 0~1로 변환

                sfxPlayers[index].Play();
                break;
            }
        }
    }

    // BGM 볼륨을 0 ~ 200 범위로 조정하고 Audio Mixer를 통해 증폭
    public void SetBgmVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0f, 200f);

        // Audio Mixer에 0 ~ 200 범위의 값을 dB로 변환 (0 dB 이상은 소리 증폭)
        float mixerVolume = Mathf.Log10(bgmVolume / 100f) * 20f;  // dB로 변환
        audioMixer.SetFloat(bgmMixerGroupName, mixerVolume);
    }

    // 효과음마다 볼륨 설정 가능
    public void SetSfxVolume(Sfx sfx, float volume)
    {
        sfxVolumes[(int)sfx] = Mathf.Clamp(volume, 0f, 200f);
    }
}
