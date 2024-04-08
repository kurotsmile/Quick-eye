using System.Collections.Generic;
using UnityEngine;

public class Data_cache : MonoBehaviour
{
    public Carrot.Carrot carrot;
    private int length_pic = 0;

    public void on_load()
    {
        this.length_pic = PlayerPrefs.GetInt("length_pic", 0);
        Debug.Log("length_pic:" + this.length_pic);
    }

    public void Add_cache(string s_id,string s_type,Texture2D pic)
    {
        PlayerPrefs.SetString("id_"+this.length_pic, s_id);
        PlayerPrefs.SetString("type_"+this.length_pic, s_type);
        this.carrot.get_tool().PlayerPrefs_Save_texture2D("pic_"+s_id, pic);
        this.length_pic++;
        PlayerPrefs.SetInt("length_pic", this.length_pic);
    }

    public Sprite check_pic(string s_id)
    {
        return this.carrot.get_tool().get_sprite_to_playerPrefs("pic_" + s_id);
    }

    public List<Sprite> get_list_by_type(string s_type)
    {
        List<Sprite> list_pic = new List<Sprite>();
        for(int y = 0; y <= length_pic; y++)
        {
            if(PlayerPrefs.GetString("type_"+y)== s_type)
            {
                string id_pic = PlayerPrefs.GetString("id_" +y);
                Sprite sp = this.carrot.get_tool().get_sprite_to_playerPrefs("pic_" + id_pic);
                list_pic.Add(sp);
            }
        }
        return list_pic;
    }

    public List<Sprite> Get_list_type_length(string s_type,int leng_get)
    {
        List<Sprite> list_pic_new = new();
        List<Sprite> list_pic = this.get_list_by_type(s_type);
        this.Shuffle(list_pic);

        for (int i = 0; i < leng_get; i++)
        {
            list_pic_new.Add(list_pic[i]);
        }
        return list_pic_new;
    }

    public bool Check_length_by_type(string leng_type,int leng_check)
    {
        List<Sprite> list_pic = this.get_list_by_type(leng_type);
        if (list_pic.Count > leng_check)
            return true;
        else
            return false;
    }

    void Shuffle(List<Sprite> a)
    {
        for (int i = a.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i);
            Sprite temp = a[i];
            a[i] = a[rnd];
            a[rnd] = temp;
        }
    }
}
