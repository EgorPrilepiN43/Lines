using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;




public class Game : MonoBehaviour
{
    Button[,] buttons;
    Image[] images;
    public AudioSource audio;
    Lines lines;


    void Start()
    {
        lines = new Lines(ShowBox, PlayCut);
        InitButtons();
        InitImages();
        lines.Start();
    }

    public void ShowBox(int x, int y, int ball)
    {
        buttons[x, y].GetComponent<Image>().sprite = images[ball].sprite;
    }

    public void PlayCut()
    {
        audio.Play();
    }

    public void Click()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int nr = GetNumber(name);
        int x = nr % Lines.Size;
        int y = nr / Lines.Size;
        Debug.Log($"Clicked {name} {x} {y}");
        lines.Click(x, y);
        
    }



    private void InitButtons()
    {
        buttons = new Button[Lines.Size, Lines.Size];
        for (int nr = 0; nr < Lines.Size * Lines.Size; nr++)
            buttons[nr % Lines.Size, nr / Lines.Size] =
            GameObject.Find($"Button ({nr})").GetComponent<Button>();
    }

    private int GetNumber(string name)
    {
        Regex regex = new Regex("\\((\\d+)\\)");
        Match match = regex.Match(name);
        if (!match.Success)
            throw new Exception("Unrecognized obgect name");
        Group group = match.Groups[1];
        string number = group.Value;
        return Convert.ToInt32(number);
    }

    private void InitImages()
    {
        images = new Image[Lines.Balls];
        for (int j = 0; j < Lines.Balls; j++)
            images[j] = GameObject.Find($"Image ({j})").GetComponent<Image>();
    }

}