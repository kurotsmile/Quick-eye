using UnityEngine;

public class Game_Rank : MonoBehaviour
{
    public Carrot.Carrot carrot;
    public Sprite icon_list_rank;
    public Sprite icon_list_history;
    public GameObject item_rank_prefab;
    public GameObject item_history_prefab;
    private int length_history = 0;
    public Color32 color_me_rank;

    void Start()
    {
        this.length_history = PlayerPrefs.GetInt("length_history", 0);
    }

    public void show_list_rank()
    {
        this.carrot.game.Show_List_Top_player();
    }

    public void add_history(int level,bool type_player,int type_box)
    {
        PlayerPrefs.SetInt("history_level_" + this.length_history, level);
        if (type_player)
        {
            PlayerPrefs.SetInt("history_player_" + this.length_history, 0);
            this.upload_rank(level, type_box);
        }
        else
            PlayerPrefs.SetInt("history_player_" + this.length_history, 1);
        PlayerPrefs.SetInt("history_box_" + this.length_history, type_box);
        PlayerPrefs.SetString("history_date_" + this.length_history, System.DateTime.Now.ToString("MM/dd/yyyy"));
        this.length_history++;
        PlayerPrefs.SetInt("length_history", this.length_history);
    }

    public void show_list_history()
    {
        if (this.length_history == 0)
        {
            this.carrot.Show_msg("Your play history", "You have not passed any level", Carrot.Msg_Icon.Alert);
        }
        else
        {
            Carrot.Carrot_Box box_history=this.carrot.Create_Box("box_history");
            box_history.set_icon(this.icon_list_history);
            box_history.set_title("Your play history");

            for (int i = this.length_history - 1; i >= 0; i--)
            {
                Carrot.Carrot_Box_Item item_history=box_history.create_item("item_history_" + i);
                string s_date = PlayerPrefs.GetString("history_date_"+i);

                if (PlayerPrefs.GetInt("history_player_" + i, 0) == 0)
                {
                    item_history.set_icon_white(this.GetComponent<Games>().icon_one_player);
                    item_history.set_title("Level " + PlayerPrefs.GetInt("history_level_" + i));
                    item_history.set_tip("Single player mode ("+ s_date+")");
                }
                else
                {
                    item_history.set_icon_white(this.GetComponent<Games>().icon_two_player);
                    if (PlayerPrefs.GetInt("history_level_" + i) == 0)
                        item_history.set_title("Player 1 wins");
                    else
                        item_history.set_title("Player 2 wins");
                    item_history.set_tip("Two-player mode (" + s_date + ")");
                }

                Carrot.Carrot_Box_Btn_Item btn_rank=item_history.create_item();
                btn_rank.set_icon(this.GetComponent<Games>().icon_item_box[PlayerPrefs.GetInt("history_box_" + i)]);
            }
        }
    }

    public void upload_rank(int level,int type_level)
    {
        this.carrot.game.update_scores_player(level, type_level);
    }


    public void delete_rank()
    {
        this.length_history=0;
        PlayerPrefs.SetInt("length_history", this.length_history);
    }
}
