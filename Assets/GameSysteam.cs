using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Data
{
    public static int DataLevel, DataScore, DataWaktu, DataDarah;
}

public class GameSysteam : MonoBehaviour
{
    public static GameSysteam instance;
    int MaxLevel = 5;

    [Header("Informasi Bar Game")]
    public bool game_aktif;
    public bool game_selesai;
    [Space]
    public bool SistemAcak;

    public int Target, datasaatini;

    [Header("Komponen UI")]
    public Text Teks_Level;
    public Text Teks_Score, Teks_Waktu;
    public RectTransform UI_Darah;

    [Header("Object GUI")]
    public GameObject GUI_PAUSE;
    public GameObject GUI_TRANSISI;

    [System.Serializable]
    public class DataGame
    {
        public string Nama;
        public Sprite Gambar;
    }

    [Header("Setingan standar")]
    public DataGame[] DataPermainan;
    [Space]
    public obj_tempatdrop[] DropTempat;
    public ObjDrag[] DragObj;

    private Vector3[] initialDragPositions;
    private Vector3[] initialDropPositions;

    private void Awake()
    {
        instance = this;

        // Simpan posisi awal dari DragObj dan DropTempat
        initialDragPositions = new Vector3[DragObj.Length];
        for (int i = 0; i < DragObj.Length; i++)
        {
            initialDragPositions[i] = DragObj[i].transform.position;
        }

        initialDropPositions = new Vector3[DropTempat.Length];
        for (int i = 0; i < DropTempat.Length; i++)
        {
            initialDropPositions[i] = DropTempat[i].transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (DataPermainan.Length == 0 || DropTempat.Length == 0 || DragObj.Length == 0)
        {
            Debug.LogError("Pastikan semua data permainan, tempat drop, dan objek drag telah diisi di Inspector.");
            return;
        }

        game_aktif = false;
        game_selesai = false;
        ResetData();
        Target = DropTempat.Length;
        if (SistemAcak)
            AcakSoal();
        datasaatini = 0;
        game_aktif = true;
    }

    void ResetData()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game0")
        {
            Data.DataLevel = 0;
            Data.DataScore = 0;
            Data.DataWaktu = 60 * 3;
            Data.DataDarah = 5;
        }
    }

    float s;
    void Update()
    {

        if (game_aktif && !game_selesai)
        {
            if (Data.DataWaktu > 0)
            {
                s += Time.deltaTime;
                if (s >= 1)
                {
                    Data.DataWaktu--;
                    s = 0;
                }
            }

            if (Data.DataWaktu <= 0)
            {
                game_aktif = false;
                game_selesai = true;

                // Game kalah
                Debug.Log("Game over: Time's up");
                kumpulansuara.instance.panggil_sfx(6);
                GUI_TRANSISI.GetComponent<UIcontrol>().btn_pindah("GameSelesai");
            }
            if (Data.DataDarah <= 0)
            {
              game_selesai = true;
              game_aktif = false;  

              //fungsi kalah
              kumpulansuara.instance.panggil_sfx(6);
              GUI_TRANSISI.GetComponent<UIcontrol>().btn_pindah("GameSelesai");
            }
            if (datasaatini >= Target)
            {
                game_selesai = true;
                game_aktif = false;

                // Game menang
                if (Data.DataLevel < MaxLevel)
                {
                    Data.DataLevel++;
                    // Pindah ke next level
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Game" + Data.DataLevel);
                    Debug.Log("Level complete. Moving to next level: " + Data.DataLevel);
                    //GUI_TRANSISI.GetComponent<UIcontrol>().btn_pindah("Game" + Data.DataLevel);
                    kumpulansuara.instance.panggil_sfx(4);
                }
                else
                {
                    // Pindah ke menu selesai
                    kumpulansuara.instance.panggil_sfx(5);
                    Debug.Log("Game complete. Moving to the final menu.");
                    GUI_TRANSISI.GetComponent<UIcontrol>().btn_pindah("GameSelesai");
                }
            }
        }

        SetInfoUI();
    }

    [HideInInspector]
    public List<int> _AcakSoal = new List<int>();
    [HideInInspector]
    public List<int> _AcakPos = new List<int>();
    int rand;
    int rand2;

    public void AcakSoal()
    {
        _AcakPos.Clear();
        _AcakSoal.Clear();

        // Pengisian list dengan indeks acak untuk DragObj
        for (int i = 0; i < DragObj.Length; i++)
        {
            rand = Random.Range(0, DataPermainan.Length);
            while (_AcakSoal.Contains(rand))
                rand = Random.Range(0, DataPermainan.Length);

            _AcakSoal.Add(rand);

            DragObj[i].ID = rand;
            DragObj[i].Teks.text = DataPermainan[rand].Nama;
        }

        // Pengisian list dengan indeks acak untuk DropTempat
        for (int i = 0; i < DropTempat.Length; i++)
        {
            rand2 = Random.Range(0, DropTempat.Length);
            while (_AcakPos.Contains(rand2))
                rand2 = Random.Range(0, DropTempat.Length);

            _AcakPos.Add(rand2);

            DropTempat[i].drop.ID = _AcakSoal[rand2];
            DropTempat[i].gambar.sprite = DataPermainan[DropTempat[i].drop.ID].Gambar;
        }

        // Mengacak posisi DragObj
        List<Vector3> tempPositions = new List<Vector3>(initialDragPositions);
        for (int i = 0; i < DragObj.Length; i++)
        {
            int randomIndex = Random.Range(0, tempPositions.Count);
            DragObj[i].transform.position = tempPositions[randomIndex];
            tempPositions.RemoveAt(randomIndex);
        }

        // Mengacak posisi DropTempat
        tempPositions = new List<Vector3>(initialDropPositions);
        for (int i = 0; i < DropTempat.Length; i++)
        {
            int randomIndex = Random.Range(0, tempPositions.Count);
            DropTempat[i].transform.position = tempPositions[randomIndex];
            tempPositions.RemoveAt(randomIndex);
        }
    }

    public void SetInfoUI()
    {
        if (Teks_Level == null || Teks_Score == null || Teks_Waktu == null || UI_Darah == null)
        {
            Debug.LogError("Pastikan semua referensi UI telah diatur di Inspector.");
            return;
        }

        Teks_Level.text = (Data.DataLevel + 1).ToString();
        int Menit = Mathf.FloorToInt(Data.DataWaktu / 60);
        int Detik = Mathf.FloorToInt(Data.DataWaktu % 60);
        Teks_Waktu.text = Menit.ToString("00") + ":" + Detik.ToString("00");

        Teks_Score.text = Data.DataScore.ToString();

        UI_Darah.sizeDelta = new Vector2(36f * Data.DataDarah, 35f);
    }

    public void btn_pause(bool pause)
    {
        if (pause)
        {
            game_aktif = false;
            GUI_PAUSE.SetActive(true);
        }
        else
        {
            game_aktif = true;
            GUI_PAUSE.SetActive(false);
        }
    }
}
