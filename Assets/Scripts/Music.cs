using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music instance;
    private static AudioSource source;
    private bool playing = false;

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

Screen.SetResolution(1920,1080,true);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (playing)
            {
                source.Stop();
                playing = false;
            }
            else
            {
                source.Play();
                playing = true;
            }
        }
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
        playing = true;
    }

	public void PlayOneShot(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

	public void PlayOneShot(AudioClip clip, float volume)
	{
		source.PlayOneShot(clip, volume);
	}
}
