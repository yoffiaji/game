using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ObjDrag : MonoBehaviour
{
    [HideInInspector]public Vector2 SavePosisi;
    [HideInInspector]public bool IsDiatasObj;
    private Transform saveobj;
    public int ID;
    public  Text Teks;
    public int Target,datasaatini;

    [Space]

    public UnityEvent OnDragBenar;

    // Start dipanggil sebelum frame update pertama
    void Start()
    {
        SavePosisi = transform.position;
    }

    // Update dipanggil sekali per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        if (IsDiatasObj)
        {
            int ID_tempat_drop = saveobj.GetComponent<tempat_drop>().ID;
            if (ID == ID_tempat_drop)
            {
            transform.SetParent(saveobj);
            transform.localPosition = Vector3.zero;
            transform.localScale = new Vector2(1f, 1.2f);
            saveobj.GetComponent<SpriteRenderer>().enabled = false;
            saveobj.GetComponent<Rigidbody2D>().simulated = false;
            saveobj.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            OnDragBenar.Invoke();

            //benar
            GameSysteam.instance.datasaatini++;
            Data.DataScore += 100;
            kumpulansuara.instance.panggil_sfx(1);

            }
            else
            {
                transform.position = SavePosisi;
                //salah
                Data.DataDarah--;
                 kumpulansuara.instance.panggil_sfx(2);

            }
          
        }
        else
        {
            transform.position = SavePosisi;
        }
    }   

    private void OnMouseDown()
    {
        SavePosisi = transform.position;
        kumpulansuara.instance.panggil_sfx(0);
    }

    private void OnMouseDrag()
    {
        if (GameSysteam.instance.game_aktif)
        {
                Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = Pos;
        }
      
    }

    private void OnTriggerStay2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("drop"))
        {
            IsDiatasObj = true;
            saveobj = trig.gameObject.transform;
        }        
    }

    private void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("drop"))
        {
            IsDiatasObj = false;
        }        
    }
}
