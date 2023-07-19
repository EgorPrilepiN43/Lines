using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using UnityEngine;

public delegate void ShowBox(int x, int y, int ball);
public delegate void PlayCut();


public class Lines
{
    public const int Size = 9;
    public const int Balls = 8;
    public int Add_Balls = 3;
    System.Random random = new System.Random();
    ShowBox showBox;
    PlayCut playCut;
    

    int[,] map;
    int fromX, fromY;
    bool isBallSelected;



    public Lines(ShowBox showBox, PlayCut playCut)
    {
        this.showBox = showBox;
        this.playCut = playCut;
        map = new int[Size, Size];
    }


    public void Start()
    {
        
        ClearMap();
        AddRadomBalls();
        isBallSelected = false;
    }







    public void Click(int x, int y)
    {

        if (IsGameOver()) Start();
        else
        {
            if (map[x, y] > 0)
                TakeBall(x, y);
            else
                MoveBall(x, y);
        }
    }






    private void TakeBall(int x, int y)
    {
        fromX = x;
        fromY = y;
        isBallSelected = true;
    }





    private void MoveBall(int x, int y)
    {
        if (!isBallSelected) return;
        if (!CanMove(x, y)) return;
        SetMap(x, y, map[fromX, fromY]);
        SetMap(fromX, fromY, 0);
        isBallSelected = false;
        if (!CutLines())
        {
            
            AddRadomBalls();
            CutLines();
        }
        
    }








    

 


    private void ClearMap()
    {
        for (int x = 0; x < Size; x++)
            for (int y = 0; y < Size; y++)
                SetMap(x, y, 0);
    }



    private bool OnMap(int x, int y)
    {
        return x >= 0 && x < Size && y >= 0 && y < Size;
    }





    private int GetMap(int x, int y)
    {
        if (!OnMap(x, y)) return 0;
        return map[x, y];
    }



    private void SetMap(int x, int y, int ball)
    {
        map[x, y] = ball;
        showBox(x, y, ball);
        Debug.Log($"{x} {y} {ball}");
    }





    private void AddRadomBalls()
    {
        for (int j = 0; j < Add_Balls; j++)
            AddRandomBall();
    }


    private void AddRandomBall()
    {
        int x, y;
        int loop = Size * Size;
        do {
            x = random.Next(Size);
            y = random.Next(Size);
            if (--loop <= 0) return;
        } while (map[x, y] > 0);
        int ball = 1 + random.Next(Balls - 1);
        SetMap(x, y, ball);
    }




    private bool[,] mark;

    private bool CutLines()
    {
        int balls = 0;
        mark = new bool[Size, Size];
        for (int x = 0; x < Size; x++)
            for (int y = 0; y < Size; y++)
            {
                balls += CutLine(x, y, 0, 1);
                balls += CutLine(x, y, 1, 0);
                balls += CutLine(x, y, 1, 1);
                balls += CutLine(x, y, -1, 1);

            }
        if (balls > 0)
        {
            
            for (int x1 = 0; x1 < 9; x1++)
                for (int y1 = 0; y1 < 9; y1++)
                    if (mark[x1, y1])
                    {
                        SetMap(x1, y1, 0);
                    }

             return true;
                    
        }
        
        return false;
        
    }


    private bool IsGameOver()
    {
        for (int x = 0; x < Size; x++)
            for (int y = 0; y < Size; y++)
                if (map[x, y] == 0) return false;
        return true;
    }



        private int CutLine(int x0, int y0, int sx, int sy)
    {
        int ball = map[x0, y0];
        if (ball == 0) return 0;
        int count = 0;
        for (int x = x0, y = y0; GetMap(x, y) == ball; x += sx, y += sy)
            count++; 
        if (count < 3)
            return 0;
        for (int x = x0, y = y0; GetMap(x, y) == ball; x += sx, y += sy)
            mark[x, y] = true;
        Debug.Log($"work bitch");

        return count;
    }

    private bool[,] used;

    private bool CanMove(int toX, int toY)
    {
        used = new bool[Size, Size];
        Walk(fromX, fromY, true);
        return used[toX, toY];
    }

    private bool Walk(int x, int y, bool start = false)
    {
        if (!start)
        {
            if (!OnMap(x, y)) return false;
            if (map[x, y] > 0) return false;
            if (used[x, y]) return false;
        }
        used[x, y] = true;
        Walk(x + 1, y);
        Walk(x - 1, y);
        Walk(x, y + 1);
        Walk(x, y - 1);
        return true;

    }



}