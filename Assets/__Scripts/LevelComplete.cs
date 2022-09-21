using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{


    // Start is called before the first frame update
    private AudioSource audioSource;
    private ThirdPersonController player;
    private ProgressHandler progress;
    public string scene;
    public enum myEnum // your custom enumeration
    {
        Drum,
        Piano,
        Trumpet,
        Violin
    };

    public myEnum dropDown;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<ThirdPersonController>();
        progress = FindObjectOfType<ProgressHandler>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("eeeeeeeeeeeeeeeeeeeeee");
            audioSource.Play(0);
            StartCoroutine(SceneTransition());
        }
           

    }

    private IEnumerator SceneTransition()
    {
        if(dropDown == myEnum.Drum)
        {
            progress.hasDrumkit = true;
        }
        if (dropDown == myEnum.Piano)
        {
            progress.hasKeyboard = true;
        }
        if (dropDown == myEnum.Violin)
        {
            progress.hasElectricGuitar = true;
        }
        if (dropDown == myEnum.Trumpet)
        {
            progress.hasBassGuitar = true;
        }
        player.canvasTransition.CloseBlackScreen();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("Hub");
    }
}
