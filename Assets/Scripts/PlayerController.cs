using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int highscore, coins, skin;
    private int[] skinsUnlocked;
    private bool reward;
    public int Highscore { get { return highscore; } }
    public int Coins { get { return coins; } }
    public int Skin { get { return skin; } }
    public int[] SkinsUnlocked { get { return skinsUnlocked; } }
    public bool Reward { set { reward = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
