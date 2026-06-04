using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIcontrol : MonoBehaviour
{
    public bool IsTransisi, IsTidakPerlu;
    private string savenamascene;
    private Animator animator;

    private void Awake()
    {
        if (IsTransisi && IsTidakPerlu)
        {
            gameObject.SetActive(false);
        }
    }



    public void btn_suara(int id)
    {
        kumpulansuara.instance.panggil_sfx(0);
    }

   public void btn_pindah(string nama)
{
    savenamascene = nama;
        GetComponent<Animator>().Play("end");
}


    public void btn_restart()
    {
        savenamascene = SceneManager.GetActiveScene().name;
        GetComponent<Animator>().Play("end");
    }


private IEnumerator WaitForAnimationEnd()
{
    // Tunggu hingga animasi selesai
    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animator.IsInTransition(0));

    // Lanjutkan ke langkah berikutnya setelah animasi selesai
    pindah();
}


    private IEnumerator WaitForAnimationEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        pindah();
    }

    public void pindah()
    {
        SceneManager.LoadScene(savenamascene);
    }

    public void btn_keluar ()
    {
        Application.Quit();
    }
}
