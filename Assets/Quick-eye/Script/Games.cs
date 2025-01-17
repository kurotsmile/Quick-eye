using Carrot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Games : MonoBehaviour
{
    [Header("Obj Game")]
    public Carrot.Carrot carrot;
    public Carrot.Carrot_DeviceOrientationChange device_rotate;
    public Data_cache data_cache;

    public IronSourceAds ads;

    [Header("Ui emp")]
    public GameObject panel_main_menu;
    public GameObject panel_select_box;
    public GameObject panel_game_over;
    public GameObject panel_play_pause;

    public Image Image_btn_status;
    public Sprite icon_btn_play;
    public Sprite icon_btn_pause;

    public Transform area_play_contain;
    public GameObject prefab_item_play;
    public Color32 color_nomal;
    public Color32 color_select;
    public AudioSource[] sound;

    private bool is_one_play;
    private int sel_index_item_box;
    public Sprite[] icon_item_box;
    public Sprite[] arr_icon_offline;
    public string[] s_name_category;

    [Header("Game Select Box")]
    public Sprite icon_one_player;
    public Sprite icon_two_player;
    public Image img_icon_model_play;

    [Header("Game player one")]
    public GameObject panel_game_1;
    private int count_select = 0;
    private float timer_check = 0;
    private float count_bar_timer = 446.1f;
    private IList<Item_img_play> list_player;
    private Item_img_play game1_player_select_one;
    private Item_img_play game1_player_select_two;

    [Header("Game player two")]
    private float game2_player1_timer_check = 0;
    private float game2_player2_timer_check = 0;
    public GameObject panel_game_2;
    public Transform area_body_player_1_done;
    public Transform area_body_player_2_done;

    public Transform area_body_player_1;
    public Transform area_body_player_2;

    public Transform area_body_player_1_portrait;
    public Transform area_body_player_2_portrait;

    public Transform area_body_player_1_landscape;
    public Transform area_body_player_2_landscape;

    public GameObject game2_icon_player1_win;
    public GameObject game2_icon_player2_win;
    public GameObject game2_icon_player1_fail;
    public GameObject game2_icon_player2_fail;

    public Slider game2_slider_status_player1_portrait;
    public Slider game2_slider_status_player2_portrait;

    public Slider game2_slider_status_player1_landscape;
    public Slider game2_slider_status_player2_landscape;

    private Item_img_play game2_player1_select_one;
    private Item_img_play game2_player1_select_two;

    private Item_img_play game2_player2_select_one;
    private Item_img_play game2_player2_select_two;

    private int count_select_player_1 = 0;
    private int count_select_player_2 = 0;

    private IList<Item_img_play> game2_list_player1;
    private IList<Item_img_play> game2_list_player2;

    private IList<Sprite> list_img = new List<Sprite>();

    private int leng_img = 0;
    private int cur_img = 0;

    public Image Timer_bar;

    private int level = 1;
    private int leng_item_cur = 0;

    public Text txt_level;

    private bool is_play = false;


    public GameObject effect_level_up;

    void Start()
    {
        this.carrot.Load_Carrot(check_exit_app);
        this.ads.On_Load();

        this.carrot.act_buy_ads_success=this.ads.RemoveAds;
        this.carrot.game.act_click_watch_ads_in_music_bk=this.ads.ShowRewardedVideo;
        this.ads.onRewardedSuccess=this.carrot.game.OnRewardedSuccess;
        this.carrot.shop.onCarrotPaySuccess += this.onBuySuccessPayCarrot;
        this.carrot.act_after_delete_all_data = delete_all_data;

        this.carrot.game.load_bk_music(this.sound[5]);

        this.panel_game_1.SetActive(false);
        this.panel_game_2.SetActive(false);
        this.effect_level_up.SetActive(false);
        this.panel_game_over.SetActive(false);
        this.panel_play_pause.SetActive(false);
        this.area_body_player_1_done.gameObject.SetActive(false);
        this.area_body_player_2_done.gameObject.SetActive(false);
        this.panel_main_menu.SetActive(true);
        this.panel_select_box.SetActive(false);

        this.data_cache.on_load();

        this.check_scene();
    }

    public void check_exit_app()
    {
        if (this.panel_select_box.activeInHierarchy)
        {
            this.btn_back_home();
            this.carrot.set_no_check_exit_app();
        }else if (this.panel_game_1.activeInHierarchy)
        {
            this.btn_back_home();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_game_2.activeInHierarchy)
        {
            this.btn_back_home();
            this.carrot.set_no_check_exit_app();
        }
    }

    public void btn_play_one()
    {
        this.ads.show_ads_Interstitial();
        this.play_sound(4);
        this.is_one_play = true;
        this.show_select_box();
    }

    public void btn_play_two()
    {
        this.play_sound(4);
        this.is_one_play = false;
        this.show_select_box();
    }

    private void show_select_box()
    {
        if (this.is_one_play)
            this.img_icon_model_play.sprite = this.icon_one_player;
        else
            this.img_icon_model_play.sprite = this.icon_two_player;
        this.panel_main_menu.SetActive(false);
        this.panel_select_box.SetActive(true);
    }

    public void btn_sel_box(int index_box)
    {
        this.panel_select_box.SetActive(false);
        this.play_sound(4);
        this.sel_index_item_box = index_box;
        if (this.is_one_play)
            this.panel_game_1.SetActive(true);
        else
            this.panel_game_2.SetActive(true);
        this.load_level();
    }

    public void btn_back_home()
    {
        this.level=1;
        this.play_sound(4);
        this.panel_play_pause.SetActive(false);
        this.panel_game_1.SetActive(false);
        this.panel_game_2.SetActive(false);
        this.panel_main_menu.SetActive(true);
        this.panel_select_box.SetActive(false);
        this.panel_game_over.SetActive(false);
        this.is_play = false;
        this.ads.show_ads_Interstitial();
    }

    private void load_level()
    {
        this.carrot.clear_contain(this.area_play_contain);
        this.carrot.clear_contain(this.area_body_player_1);
        this.carrot.clear_contain(this.area_body_player_2);

        StructuredQuery q = new("icon");
        q.Add_where("category", Query_OP.EQUAL, this.s_name_category[this.sel_index_item_box]);
        if (this.is_one_play)
            leng_item_cur = 3 + (this.level * 2);
        else
            leng_item_cur = 9;
        q.Set_limit(this.leng_item_cur);

        this.reset_bar_timer();
        this.txt_level.text = "Level " + this.level;
        this.list_img = new List<Sprite>();
        this.list_player = new List<Item_img_play>();
        this.game2_list_player1 = new List<Item_img_play>();
        this.game2_list_player2 = new List<Item_img_play>();

        this.count_select = 0;
        if (this.is_one_play)
        {
            this.game1_player_select_one = null;
            this.game1_player_select_two = null;
        }
        else
        {
            this.game2_player1_select_one = null;
            this.game2_player1_select_two = null;

            this.game2_player2_select_one = null;
            this.game2_player2_select_two = null;
        }

        if (this.data_cache.Check_length_by_type(this.sel_index_item_box.ToString(),this.leng_item_cur))
        {
            this.leng_img = this.leng_item_cur;
            this.list_img = this.data_cache.Get_list_type_length(this.sel_index_item_box.ToString(), this.leng_img);
            this.load_level_play();
            this.carrot.hide_loading();
            this.GetComponent<Game_Rank>().upload_rank(this.level, this.sel_index_item_box);
        }
        else 
        {
            this.carrot.server.Get_doc(q.ToJson(), act_get_list_image);
        }
    }


    void Update()
    {
        if (this.is_play)
        {
            if (this.is_one_play)
            {
                if (this.count_select >= 2)
                {
                    this.timer_check += 1f * Time.deltaTime;
                    if (this.timer_check >= 1f)
                    {
                        this.timer_check = 0f;
                        if (this.game1_player_select_one.id_i == this.game1_player_select_two.id_i)
                        {
                            this.game1_player_select_one.is_open = true;
                            this.game1_player_select_two.is_open = true;
                            this.play_sound(1);
                            this.reset_bar_timer();
                            this.check_next_level();
                        }
                        else
                        {
                            this.play_sound(6);
                        }
                        this.game1_reset_all_player();
                    }
                }

                this.count_bar_timer -= 10f * Time.deltaTime;
                this.Timer_bar.rectTransform.sizeDelta = new Vector2(this.count_bar_timer, this.Timer_bar.rectTransform.sizeDelta.y);

                if (this.count_bar_timer < 0)
                {
                    this.carrot.play_vibrate();
                    this.panel_game_over.SetActive(true);
                    this.play_sound(2);
                    this.GetComponent<Game_Rank>().add_history(this.level, this.is_one_play, this.sel_index_item_box);
                    this.is_play = false;
                }
            }
            else
            {
                if (this.count_select_player_1 >= 2)
                {
                    this.game2_player1_timer_check += 1f * Time.deltaTime;
                    if (this.game2_player1_timer_check >= 1f)
                    {

                        this.game2_player1_timer_check = 0f;
                        if (this.game2_player1_select_one.id_i == this.game2_player1_select_two.id_i)
                        {
                            this.game2_player1_select_one.is_open = true;
                            this.game2_player1_select_two.is_open = true;
                            this.play_sound(1);
                            this.game2_check_win_player1();
                        }
                        else
                        {
                            this.play_sound(6);
                        }
                        this.game2_reset_all_player1();
                    }
                }

                if (this.count_select_player_2 >= 2)
                {
                    this.game2_player2_timer_check += 1f * Time.deltaTime;
                    if (this.game2_player2_timer_check >= 1f)
                    {

                        this.game2_player2_timer_check = 0f;
                        if (this.game2_player2_select_one.id_i == this.game2_player2_select_two.id_i)
                        {
                            this.game2_player2_select_one.is_open = true;
                            this.game2_player2_select_two.is_open = true;
                            this.play_sound(1);
                            this.game2_check_win_player2();
                        }
                        else
                        {
                            this.play_sound(6);
                        }
                        this.game2_reset_all_player2();
                    }
                }
            }
        }
    }

    private void check_next_level()
    {
        int count_open = 0;
        foreach (Item_img_play i_play in this.list_player)
        {
            if (i_play.is_open)
            {
                count_open++;
            }
        }
        if (count_open >= (this.list_player.Count))
        {
            this.effect_level_up.SetActive(false);
            this.effect_level_up.SetActive(true);
            this.level++;
            this.play_sound(3);
            this.list_player = new List<Item_img_play>();
            this.load_level();
            this.ads.show_ads_Interstitial();
        }
    }

    private void game2_check_win_player1()
    {
        int count_open = 0;
        foreach (Item_img_play i_play in this.game2_list_player1)
        {
            if (i_play.is_open)
            {
                count_open++;
            }
        }
        if (count_open >= (this.game2_list_player1.Count))
        {
            this.effect_level_up.SetActive(false);
            this.effect_level_up.SetActive(true);
            this.play_sound(3);
            this.area_body_player_1_done.gameObject.SetActive(true);
            this.area_body_player_2_done.gameObject.SetActive(true);
            this.ads.show_ads_Interstitial();
            this.show_game_success(0);
        }
        else
        {
            this.game2_slider_status_player1_portrait.value = count_open;
            this.game2_slider_status_player1_landscape.value = count_open;
        }
    }

    private void game2_check_win_player2()
    {
        int count_open = 0;
        foreach (Item_img_play i_play in this.game2_list_player2)
        {
            if (i_play.is_open)
            {
                count_open++;
            }
        }
        if (count_open >= (this.game2_list_player2.Count))
        {
            this.effect_level_up.SetActive(false);
            this.effect_level_up.SetActive(true);
            this.play_sound(3);
            this.area_body_player_2_done.gameObject.SetActive(true);
            this.area_body_player_1_done.gameObject.SetActive(true);
            this.ads.show_ads_Interstitial();
            this.show_game_success(1);
        }
        else
        {
            this.game2_slider_status_player2_landscape.value = count_open;
            this.game2_slider_status_player2_portrait.value = count_open;
        }
    }


    private void show_game_success(int palyer_win)
    {
        this.game2_icon_player1_fail.SetActive(false);
        this.game2_icon_player1_win.SetActive(false);
        this.game2_icon_player2_fail.SetActive(false);
        this.game2_icon_player2_win.SetActive(false);

        if (palyer_win == 0)
        {
            this.game2_icon_player1_win.SetActive(true);
            this.game2_icon_player2_fail.SetActive(true);
        }
        else
        {
            this.game2_icon_player2_win.SetActive(true);
            this.game2_icon_player1_fail.SetActive(true);
        }

        this.GetComponent<Game_Rank>().add_history(palyer_win, this.is_one_play, this.sel_index_item_box);
        this.carrot.play_vibrate();
        this.reset_bar_timer();
    }

    public void game2_play_again()
    {
        this.reset_bar_timer();
        this.area_body_player_1_done.gameObject.SetActive(false);
        this.area_body_player_2_done.gameObject.SetActive(false);
        this.load_level();
        this.ads.show_ads_Interstitial();
    }

    public void game_play_again()
    {
        this.level = 1;
        this.panel_game_over.SetActive(false);
        this.reset_bar_timer();
        this.load_level();
        this.ads.show_ads_Interstitial();
    }

    public void game_play_next()
    {
        this.panel_game_over.SetActive(false);
        this.reset_bar_timer();
        this.is_play = true;
    }

    private void reset_bar_timer()
    {
        this.count_bar_timer = 387.3f;
        this.Timer_bar.rectTransform.sizeDelta = new Vector2(this.count_bar_timer, this.Timer_bar.rectTransform.sizeDelta.y);
        this.game2_slider_status_player1_portrait.value = 0;
        this.game2_slider_status_player2_portrait.value = 0;
        this.game2_slider_status_player1_landscape.value = 0;
        this.game2_slider_status_player2_landscape.value = 0;
    }

    public void load_level_play()
    {
        IList<int> arr_i = new List<int>();
        for (int i = 0; i < this.list_img.Count; i++)
        {
            arr_i.Add(i);
            arr_i.Add(i);
        }

        for (int i = 0; i < arr_i.Count; i++)
        {
            int temp = arr_i[i];
            int randomIndex = Random.Range(i, arr_i.Count);
            arr_i[i] = arr_i[randomIndex];
            arr_i[randomIndex] = temp;
        }

        this.list_player = new List<Item_img_play>();
        this.game2_list_player1 = new List<Item_img_play>();
        this.game2_list_player2 = new List<Item_img_play>();

        foreach (int i_id in arr_i)
        {
            if (this.is_one_play)
            {
                GameObject item_play = Instantiate(this.prefab_item_play);
                item_play.transform.SetParent(this.area_play_contain);
                item_play.transform.localPosition = new Vector3(item_play.transform.localPosition.x, item_play.transform.localPosition.y, 0f);
                item_play.transform.localRotation = Quaternion.Euler(Vector3.zero);
                item_play.transform.localScale = new Vector3(1f, 1f, 1f);
                item_play.GetComponent<Item_img_play>().id_i = i_id;
                item_play.GetComponent<Item_img_play>().img.sprite = this.list_img[i_id];
                item_play.GetComponent<Item_img_play>().types = 0;
                item_play.GetComponent<Item_img_play>().img_question.sprite = this.icon_item_box[this.sel_index_item_box];
                this.list_player.Add(item_play.GetComponent<Item_img_play>());
            }
            else
            {
                GameObject item_play_p1 = Instantiate(this.prefab_item_play);
                item_play_p1.transform.SetParent(this.area_body_player_1);
                item_play_p1.transform.localPosition = new Vector3(item_play_p1.transform.localPosition.x, item_play_p1.transform.localPosition.y, 0f);
                item_play_p1.transform.localRotation = Quaternion.Euler(new Vector3(0,0,180f));
                item_play_p1.transform.localScale = new Vector3(1f, 1f, 1f);
                item_play_p1.GetComponent<Item_img_play>().id_i = i_id;
                item_play_p1.GetComponent<Item_img_play>().img.sprite = this.list_img[i_id];
                item_play_p1.GetComponent<Item_img_play>().types = 1;
                item_play_p1.GetComponent<Item_img_play>().img_question.sprite = this.icon_item_box[this.sel_index_item_box];
                this.game2_list_player1.Add(item_play_p1.GetComponent<Item_img_play>());

                GameObject item_play_p2 = Instantiate(this.prefab_item_play);
                item_play_p2.transform.SetParent(this.area_body_player_2);
                item_play_p2.transform.localPosition = new Vector3(item_play_p2.transform.localPosition.x, item_play_p2.transform.localPosition.y, 0f);
                item_play_p2.transform.localRotation = Quaternion.Euler(Vector3.zero);
                item_play_p2.transform.localScale = new Vector3(1f, 1f, 1f);
                item_play_p2.GetComponent<Item_img_play>().id_i = i_id;
                item_play_p2.GetComponent<Item_img_play>().img.sprite = this.list_img[i_id];
                item_play_p2.GetComponent<Item_img_play>().types = 2;
                item_play_p2.GetComponent<Item_img_play>().img_question.sprite = this.icon_item_box[this.sel_index_item_box];
                this.game2_list_player2.Add(item_play_p2.GetComponent<Item_img_play>());
            }

        }

        if (!this.is_one_play)
        {
            
            this.game2_slider_status_player1_portrait.maxValue = this.game2_list_player1.Count;
            this.game2_slider_status_player2_portrait.maxValue = this.game2_list_player2.Count;
            this.game2_slider_status_player1_landscape.maxValue = this.game2_list_player1.Count;
            this.game2_slider_status_player2_landscape.maxValue = this.game2_list_player2.Count;
        }
        this.is_play = true;
    }

    public void select_item_play(Item_img_play item_play)
    {
        this.play_sound(0);
        if (this.count_select < 2)
        {
            if (this.count_select == 0)
            {
                this.game1_player_select_one = item_play;
            }

            if (this.count_select == 1)
            {
                this.game1_player_select_two = item_play;
            }

            item_play.img_border.color = this.color_select;
            item_play.panel_question.SetActive(false);
            item_play.GetComponent<Button>().enabled = false;
            this.count_select++;
        }
    }

    public void select_item_player_1(Item_img_play item_play)
    {
        this.play_sound(0);
        if (this.count_select_player_1 < 2)
        {
            if (this.count_select_player_1 == 0)
            {
                this.game2_player1_select_one = item_play;
            }

            if (this.count_select_player_1 == 1)
            {
                this.game2_player1_select_two = item_play;
            }

            item_play.img_border.color = this.color_select;
            item_play.panel_question.SetActive(false);
            item_play.GetComponent<Button>().enabled = false;
            this.count_select_player_1++;
        }
    }

    public void select_item_player_2(Item_img_play item_play)
    {
        this.play_sound(0);
        if (this.count_select_player_2 < 2)
        {
            if (this.count_select_player_2 == 0)
            {
                this.game2_player2_select_one = item_play;
            }

            if (this.count_select_player_2 == 1)
            {
                this.game2_player2_select_two = item_play;
            }

            item_play.img_border.color = this.color_select;
            item_play.panel_question.SetActive(false);
            item_play.GetComponent<Button>().enabled = false;
            this.count_select_player_2++;
        }
    }

    private void game1_reset_all_player()
    {
        foreach (Item_img_play item_i in this.list_player)
        {
            item_i.img_border.color = this.color_nomal;
            if (!item_i.is_open)
            {
                item_i.panel_question.SetActive(true);
                item_i.GetComponent<Button>().enabled = true;
            }
        }
        this.count_select = 0;
    }


    private void game2_reset_all_player1()
    {
        foreach (Item_img_play item_i in this.game2_list_player1)
        {
            item_i.img_border.color = this.color_nomal;
            if (item_i.is_open == false)
            {
                item_i.panel_question.SetActive(true);
                item_i.GetComponent<Button>().enabled = true;
            }
        }
        this.count_select_player_1 = 0;
    }

    private void game2_reset_all_player2()
    {
        foreach (Item_img_play item_i in this.game2_list_player2)
        {
            item_i.img_border.color = this.color_nomal;
            if (item_i.is_open == false)
            {
                item_i.panel_question.SetActive(true);
                item_i.GetComponent<Button>().enabled = true;
            }
        }
        this.count_select_player_2 = 0;
    }

    private void act_get_list_image(string s_data)
    {
        Fire_Collection fc = new(s_data);
        if (!fc.is_null)
        {
            IList all_image = (IList)Json.Deserialize("[]");
            for(int i = 0; i < fc.fire_document.Length; i++)
            {
                all_image.Add(fc.fire_document[i].Get_IDictionary());
            }

            this.leng_img = all_image.Count;
            this.list_img = new List<Sprite>();
            this.cur_img = 0;
            for (int i = 0; i < all_image.Count; i++)
            {
                IDictionary data_icon = (IDictionary)all_image[i];
                Debug.Log("ID:" + data_icon["id"] + " type:" + data_icon["category"].ToString());
                Sprite sp_pic = this.data_cache.check_pic(data_icon["id"].ToString());
                if (sp_pic != null)
                {
                    this.list_img.Add(sp_pic);
                    this.check_full_pic_load_game();
                }
                else
                    StartCoroutine(this.LoadPictureToList(data_icon["icon"].ToString(), data_icon["id"].ToString(), data_icon["category"].ToString()));
            }
        }
    }


    private IEnumerator LoadPictureToList(string url,string id,string type)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result==UnityWebRequest.Result.Success)
        {
            Texture2D profilePic = ((DownloadHandlerTexture)www.downloadHandler).texture;
            this.list_img.Add(Sprite.Create(profilePic, new Rect(0, 0, profilePic.width, profilePic.height), new Vector2(0, 0)));
            this.data_cache.Add_cache(id, type, profilePic);
            this.check_full_pic_load_game();
        }
    }

    private void check_full_pic_load_game()
    {
        this.cur_img++;
        if (this.cur_img >= this.leng_img)
        {
            this.load_level_play();
            this.carrot.hide_loading();
            this.GetComponent<Game_Rank>().upload_rank(this.level, this.sel_index_item_box);
        }
    }

    public void open_setting()
    {
        this.is_play = false;
        Carrot.Carrot_Box box_setting=this.carrot.Create_Setting();
        box_setting.set_act_before_closing(close_setting);
        this.check_show_img_status();
    }

    public void close_setting()
    {
        this.play_sound(4);
        if (this.panel_game_1.activeInHierarchy) this.is_play = true;
        if (this.panel_game_2.activeInHierarchy) this.is_play = true;
        this.check_show_img_status();
    }

    public void btn_play_and_pause()
    {
        this.play_sound(4);
        if (this.is_play)
        {
            this.is_play = false;
            this.sound[5].Pause();
            this.panel_play_pause.SetActive(true);
        }
        else
        {
            this.is_play = true;
            this.sound[5].UnPause();
            this.panel_play_pause.SetActive(false);
        }
        this.check_show_img_status();
    }

    private void check_show_img_status()
    {
        if (this.is_play)
            this.Image_btn_status.sprite = this.icon_btn_pause;
        else
            this.Image_btn_status.sprite = this.icon_btn_play;
    }
    public void share_app()
    {
        this.carrot.show_share();
    }

    public void rate_app()
    {
        this.carrot.show_rate();
    }

    public void delete_all_data()
    {
        this.GetComponent<Game_Rank>().delete_rank();
        this.carrot.delay_function(1f, this.Start);
    }

    private void onBuySuccessPayCarrot(string id_product)
    {
        if (id_product == this.carrot.shop.get_id_by_index(1))
        {
            this.panel_game_over.SetActive(false);
            this.reset_bar_timer();
            this.is_play = true;
            this.carrot.Show_msg("Continue screen play", "Purchase successfully, you can continue your game screen!", Carrot.Msg_Icon.Success);
        }
    }

    public void btn_share()
    {
        this.carrot.show_share();
    }

    public void btn_login_user()
    {
        this.carrot.show_login();
    }

    public void btn_show_list_app_carrot()
    {
        this.carrot.show_list_carrot_app();
    }


    public void play_sound(int index)
    {
        if(this.carrot.get_status_sound()) this.sound[index].Play();
    }

    public void show_user_buy_id(string s_id,string s_lang)
    {
        this.play_sound(4);
        this.carrot.user.show_user_by_id(s_id, s_lang);
    }

    public void btn_show_list_rank()
    {
        this.play_sound(4);
        this.GetComponent<Game_Rank>().show_list_rank();
    }
    
    public void btn_show_list_history()
    {
        this.play_sound(4);
        this.GetComponent<Game_Rank>().show_list_history();
    }

    public void check_scene()
    {
        this.carrot.delay_function(1f, check_roation_scene);
    }

    private void check_roation_scene()
    {
        bool is_portrait = this.device_rotate.Get_status_portrait();
        if (is_portrait)
        {
            this.area_body_player_1.SetParent(this.area_body_player_1_portrait);
            this.set_rect_default(this.area_body_player_1);
            this.area_body_player_1.localScale = new Vector3(1f, 1f, 1f);

            this.area_body_player_2.SetParent(this.area_body_player_2_portrait);
            this.set_rect_default(this.area_body_player_2);
            this.area_body_player_2.localScale = new Vector3(1f, 1f, 1f);

            this.area_body_player_1_done.SetParent(this.area_body_player_1_portrait);
            this.set_rect_default(this.area_body_player_1_done);
            this.area_body_player_2_done.SetParent(this.area_body_player_2_portrait);
            this.set_rect_default(this.area_body_player_2_done);
        }
        else
        {
            this.area_body_player_1.SetParent(this.area_body_player_1_landscape);
            this.set_rect_default(this.area_body_player_1);
            this.area_body_player_1.localScale = new Vector3(0.6f, -0.6f, 0f);

            this.area_body_player_2.SetParent(this.area_body_player_2_landscape);
            this.set_rect_default(this.area_body_player_2);
            this.area_body_player_2.localScale = new Vector3(0.6f, 0.6f, 0f);

            this.area_body_player_1_done.SetParent(this.area_body_player_1_landscape);
            this.set_rect_default(this.area_body_player_1_done);
            this.area_body_player_2_done.SetParent(this.area_body_player_2_landscape);
            this.set_rect_default(this.area_body_player_2_done);
        }
    }

    private void set_rect_default(Transform tr_set)
    {
        RectTransform r = tr_set.GetComponent<RectTransform>();
        r.pivot = new Vector2(0.5f, 0.5f);
        r.anchorMin = new Vector2(0f, 0f);
        r.anchorMax = new Vector2(1f, 1f);
        r.offsetMin = new Vector2(0, r.offsetMin.y);
        r.offsetMax = new Vector2(0, r.offsetMax.y);
        r.offsetMax = new Vector2(r.offsetMax.x, 0f);
        r.offsetMin = new Vector2(r.offsetMin.x, 0f);
    }
}
