using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvaspengaturan : MonoBehaviour
{
    public Slider Slider_sfx,Slider_BGM;

    private void OnEnable ()
    {
        Slider_sfx.value = kumpulansuara.instance.source_sfx.volume;
        Slider_BGM.value = kumpulansuara.instance.source_bgm.volume;
    }

    public void UbahVolume(bool SFX) 
    {
        if (SFX)
        {
            kumpulansuara.instance.source_sfx.volume = Slider_sfx.value;
        }
        else
        {
            kumpulansuara.instance.source_bgm.volume = Slider_BGM.value;
        }
    }



}
