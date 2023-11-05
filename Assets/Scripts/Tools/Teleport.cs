using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform destination;
    GameObject player;
    Rigidbody2D playerRb;
    [SerializeField] Animator transitionAim;

    private AudioSource teleportAudio; // Add this line
    [SerializeField] private AudioClip newBackgroundAudio;
    [SerializeField] private AudioSource backgroundAudioSource;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();
        teleportAudio = GetComponent<AudioSource>(); // Initialize the AudioSource
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Vector2.Distance(player.transform.position, transform.position) > 0.3f)
            {
                StartCoroutine(PortalIn());
            }
        }
    }

    IEnumerator PortalIn()
    {
        // Pause the existing background audio.
        backgroundAudioSource.Pause();

        // Disable player movement and trigger the closing transition effect.
        playerRb.simulated = false;
        transitionAim.SetTrigger("End");
        StartCoroutine(MoveInPortal());
        yield return new WaitForSeconds(0.5f);

        // Change the background audio clip.
        backgroundAudioSource.clip = newBackgroundAudio;

        // Resume playing the background audio with the new clip.
        backgroundAudioSource.Play();

        // Trigger the opening transition effect and teleport the player.
        transitionAim.SetTrigger("Start");
        player.transform.position = destination.position;
        yield return new WaitForSeconds(0.5f);
        playerRb.simulated = true;
    }
    IEnumerator MoveInPortal()
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
    }
}
