using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public int indiceY;
    public int indiceX;

    public Cuboo board;


    private void Start()
    {

    }

    public void Inicio(int cambioX, int cambioY)
    {
        indiceX = cambioX;
        indiceY = cambioY;
    }

    private void OnMouseDown()
    {
        board.SetInicialMouse(this);
    }

    private void OnMouseEnter()
    {
        board.SetEndMouse(this);
    }

    private void OnMouseUp()
    {
        board.ReleaseTile();
    }

}

