using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    //Variabel isntance membuat sebuah kelas dapat di panggil oleh kelas lainnya secara mudah
    public static GameData instance;

    public bool isSinglePlayer;
    public float gameTimer;


    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

            DontDestroyOnLoad(gameObject); //menyimpan variabel walau berbeda scene
    }
}
