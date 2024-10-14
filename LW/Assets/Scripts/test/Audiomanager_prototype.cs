using UnityEngine;
using UnityEngine.Audio;

public class Audiomanager_prototype : MonoBehaviour
{
    public static Audiomanager_prototype instance;

    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;  // 0 ~ 200 ����
    AudioSource bgmPlayer;
    int currentBgmIndex;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float[] sfxVolumes;  // ȿ�������� ���� ���� ����
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;  // Audio Mixer ����
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

        // ȿ���� ���� �迭 �ʱ�ȭ (ȿ���� ������ �����ؾ� ��)
        if (sfxVolumes == null || sfxVolumes.Length != sfxClips.Length)
        {
            sfxVolumes = new float[sfxClips.Length];
            for (int i = 0; i < sfxVolumes.Length; i++)
            {
                sfxVolumes[i] = 100f;  // �⺻������ ��� ȿ������ 100%�� ����
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

                // sfxVolumes �迭���� �ش� ȿ������ ������ �����ͼ� ����
                float volume = Mathf.Clamp(sfxVolumes[(int)sfx], 0f, 200f);
                sfxPlayers[index].volume = volume / 100f;  // 0~200 ���� 0~1�� ��ȯ

                sfxPlayers[index].Play();
                break;
            }
        }
    }

    // BGM ������ 0 ~ 200 ������ �����ϰ� Audio Mixer�� ���� ����
    public void SetBgmVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0f, 200f);

        // Audio Mixer�� 0 ~ 200 ������ ���� dB�� ��ȯ (0 dB �̻��� �Ҹ� ����)
        float mixerVolume = Mathf.Log10(bgmVolume / 100f) * 20f;  // dB�� ��ȯ
        audioMixer.SetFloat(bgmMixerGroupName, mixerVolume);
    }

    // ȿ�������� ���� ���� ����
    public void SetSfxVolume(Sfx sfx, float volume)
    {
        sfxVolumes[(int)sfx] = Mathf.Clamp(volume, 0f, 200f);
    }
}
