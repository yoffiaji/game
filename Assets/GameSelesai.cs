using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelesai : MonoBehaviour
{
    public Text TeksScore, TeksTotalScore;

    public void Start()
    {
        // Pastikan Data.DataScore adalah properti atau field statis yang valid
        if (Data.DataScore >= PlayerPrefs.GetInt("score", 0)) // 0 adalah default value jika "score" belum ada
        {
            PlayerPrefs.SetInt("score", Data.DataScore);
        }

        TeksScore.text = Data.DataScore.ToString();
        TeksTotalScore.text = PlayerPrefs.GetInt("score").ToString();
    }   
}
