using System.Collections;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    [SerializeField] private Transform startSpawn;
    [SerializeField] private PlayerController player;
    [SerializeField] private Animator fadeAnimator;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("GameManager is already present in the Scene!");
            return;
        }
        instance = this;
    }

    void Start()
    {
        player.transform.position = startSpawn.position;
    }

    public void Respawn()
    {
        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        yield return new WaitForSeconds(1.1f);
        fadeAnimator.SetTrigger("fadeIn");
        yield return new WaitForSeconds(1f);
        player.transform.position = startSpawn.position;
    }

}
