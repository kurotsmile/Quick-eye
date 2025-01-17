using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_img_play : MonoBehaviour
{
    public int id_i;
    public Image img;
    public Image img_border;
    public Image img_question;
    public GameObject panel_question;
    public bool is_open = false;
    public int types = 0;
    public void click()
    {
        if (this.is_open == false)
        {
            if (this.types == 0)
            {
                GameObject.Find("Game").GetComponent<Games>().select_item_play(this);
            }

            if (this.types == 1)
            {
                GameObject.Find("Game").GetComponent<Games>().select_item_player_1(this);
            }

            if (this.types == 2)
            {
                GameObject.Find("Game").GetComponent<Games>().select_item_player_2(this);
            }
        }
    }
}


