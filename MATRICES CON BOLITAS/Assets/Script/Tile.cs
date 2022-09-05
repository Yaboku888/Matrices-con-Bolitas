using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Tile incial;
    Tile final;
    public int indiceX;
    public int indiceY;
   public Cuboo board;


    public  void Inicializar (int x, int y)
    {
        indiceX = x;
        indiceY = y;
    }


    private void OnMouseDown()
    {
       board.SetInicialMause(this);
    }

    private void OnMouseEnter()
    {
        board.SetFinalTile(this);
    }

    private void OnMouseUp()
    {
        board.ReleaseTile();
    }


}
