using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kumpulansuara : MonoBehaviour
{

    public static kumpulansuara instance;

    public AudioClip[] clip;

    public AudioSource source_sfx;
    public AudioSource source_bgm;

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    public void panggil_sfx(int id)
    {
        source_sfx.PlayOneShot(clip[id]);
    }
  
}
