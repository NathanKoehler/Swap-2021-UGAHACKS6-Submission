using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController_S : MonoBehaviour
{
    // Start is called before the first frame update
    public static MasterController_S self;
    public int curLvl;
    public static List<Player_S> players;
    
    public static bool redReady = false;
    public static bool blueReady = false;
    //public int playersReady;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (redReady && blueReady)
        {
            this.curLvl++;
            Debug.Log("Level " + this.curLvl);
            SceneManager.LoadScene("Level " + this.curLvl, LoadSceneMode.Single);
            redReady = false;
            blueReady = false;
        }
    }

    private void Awake()
    {
        if (self == null)
        {
            self = this;
            DontDestroyOnLoad(gameObject); // Basic method to remain even after scene load
        }
        else Destroy(gameObject);

        players = new List<Player_S>();
    }

    public static void resetPosition()
    {
        foreach (Player_S player in players) {
            player.tpBack();
        }
    }

    public static void changeColor()
    {
        Color a = players[0].playerLight.color;
        players[0].playerLight.color = players[1].playerLight.color;
        players[1].playerLight.color = a;
    }

}