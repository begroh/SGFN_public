using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music instance;
    private static AudioSource source;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            source = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        AudioClip clip = (AudioClip) Resources.Load("Audio/Sounds/Background", typeof(AudioClip));
        instance.Play(clip);
    }

    public void Play(AudioClip clip)
    {
        if (source == null)
        {
            return;
        }

        source.Stop();
        source.clip = clip;
        source.loop = true;
        source.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
