using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Tile incial;
    Tile final;
    public int indicex;
    public int indicey;
   public Cuboo g;


    public  void inicialization (int x, int y)
    {
        indicex = x;
        indicey = y;
    }


    private void OnMouseDown()
    {
        g.SetInicialMause(this);
    }

    private void OnMouseEnter()
    {
        g.SetFinalTile(this);
    }

    private void OnMouseUp()
    {
        g.ReleaseTile();
    }


}
