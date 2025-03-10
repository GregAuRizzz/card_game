using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // R�f�rences pour l'AudioSource et les clips
    private AudioSource audioSource;
    public AudioClip backgroundMusic; // Musique de fond pour GameScene
    public AudioClip buttonClickSound; // Son des boutons
    public AudioClip[] attackSounds; // Sons al�atoires pour les attaques de cartes

    private static AudioManager instance; // Singleton pour l'AudioManager

    void Awake()
    {
        // Si une instance existe d�j�, d�truire le nouvel objet
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Sinon, assigner cette instance � l'objet statique et emp�cher sa destruction entre les sc�nes
        instance = this;
        DontDestroyOnLoad(gameObject);

        // R�cup�rer le composant AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // Jouer la musique de fond uniquement sur la sc�ne "GameScene"
    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.volume = 0.2f;
            audioSource.loop = true; // La musique de fond doit �tre en boucle
            audioSource.Play();
        }
    }

    // Arr�ter la musique de fond
    public void StopBackgroundMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Jouer le son d'un bouton
    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {

            audioSource.PlayOneShot(buttonClickSound); // On joue le son sans affecter la musique
        }
    }


    // Acc�der � l'instance du AudioManager depuis n'importe quel script
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("AudioManager instance not found.");
            }
            return instance;
        }
    }
}
