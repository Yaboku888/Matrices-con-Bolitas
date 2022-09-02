using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuboo : MonoBehaviour
{
    public int alto;
    public int ancho; 
    public float borde;
    public Tile[,] board;
    public GameObject prefTile;
    public Camera cameraPlayer;
    public GameObject[] prefpuntos;
    public GamePeace[,] pieza;
   
    public Tile inicial;
    public Tile Final;
    void Start()
    {
        llenarMatriz();
        CreateBoard();
        organizar();
    }

    void CreateBoard()
    {
        board = new Tile[ancho, alto];

        for (int i = 0; i < ancho; i++) //i para x
        {
            for (int j = 0; j < alto; j++) //j para y
            {
                GameObject go = Instantiate(prefTile);
                go.name = "Tile(" + i + ", " + j + ")";
                go.transform.position = new Vector3(i, j, 0);
                Tile tile = go.GetComponent<Tile>();
                tile.inicialization(i, j);
                board[i, j] = tile;
                tile.g = this;

                cameraPlayer.transform.localPosition = new Vector3(i / 2, j / 2, -1);
                cameraPlayer.orthographicSize = (float)alto / 2;

            }
        }
    }


    void organizar()
    {
        float b = ((float)ancho / 2f + borde) / ((float)Screen.width / (float)Screen.height);
        float a = ((float)alto / 2f + borde);

        if (a > b)
        {
            cameraPlayer.orthographicSize = a;
        }

        else
        {
            cameraPlayer.orthographicSize = b;

        }
    }

    GameObject PiezaAleatoria()
    {
        int numeroA = Random.Range(0, prefpuntos.Length);
        GameObject go = Instantiate(prefpuntos[numeroA]);
        return go;
    }

    void UbicarPieza(GamePeace gp, int x, int y)
    {
        gp.Inicializar(x, y);
    }

    void llenarMatriz()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int g = 0; g < alto; g++)
            {
                GameObject go = PiezaAleatoria();
                go.transform.position = new Vector3(i, g, 0f);
            }
        }
    }

    public void SetInicialMause(Tile ini)
    {
        if (inicial = null)
        {
            inicial = ini;
        }

    }
    public void SetFinalTile(Tile fin)
    {
        if (inicial != null)
        {
            Final = fin;
        }
    }
    public void ReleaseTile()
    {
        if(inicial != null && Final != null)
        {
            cambioPieza(inicial, Final);
            //inicial = null;
            //Final = null;
        }
    }


    void cambioPieza(Tile initi, Tile and)
    {
        GamePeace Goin = pieza[initi.indicex, initi.indicey];
        GamePeace GoEnd = pieza[and.indicex, and.indicey];
    }
}



