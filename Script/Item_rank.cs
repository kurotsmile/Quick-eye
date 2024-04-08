using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_rank : MonoBehaviour
{
    public Image img_rank_index;
    public Image img_avatar;
    public Text txt_name;
    public Text txt_level;
    public Text txt_date;
    public string s_user_id;
    public string s_user_lang;
    
    public void click()
    {
        GameObject.Find("Game").GetComponent<Games>().show_user_buy_id(s_user_id, s_user_lang);
    }
}
