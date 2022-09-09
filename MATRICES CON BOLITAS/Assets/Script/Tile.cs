using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public int indiceX;
    public int indiceY;
    public Cuboo board;

    private void Start()
    {

    }
    public void Inicializar(int x, int y)
    {
        indiceX = x;
        indiceY = y;
    }
    public void OnMouseDown()
    {
        board.SetInicialTile(this);
    }
    public void OnMouseEnter()
    {
        board.SetFinalTile(this);
    }
    public void OnMouseUp()
    {
        board.ReleaseTile();

    }
}

