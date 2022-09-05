using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuboo : MonoBehaviour
{
    public int alto;
    public int ancho; 
    public float borde;
    public GameObject prefTile;
    public Camera cameraPlayer;
    public GameObject[] prefpuntos;
    public GamePeace[,] gamePiece;
    public Tile[,] board;
   
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

        for (int i = 0; i< ancho; i++)
        {
            for (int j= 0; j < alto; j++)
            {
                GameObject go = Instantiate(prefTile);
                go.name = "Tile(" + i + "," + j + ")";
                go.transform.position = new Vector3(i, j, 0);
                go.transform.parent = transform;

                Tile tile = go.GetComponent<Tile>();
                tile.board = this;
                tile.Inicializar(i, j);
                board[i, j] = tile;


                cameraPlayer.transform.localPosition = new Vector3(i / 2f, j / 2f, -5);
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
        go.GetComponent<GamePeace>().board = this;
        return go;
    }

   public void UbicarPieza(GamePeace gp, int x, int y)
    {
        gp.transform.position = new Vector3(x, y, 0f);
        gp.cordenadas(x, y);
        gamePiece[x, y] = gp;
    }

    void llenarMatriz()
    {
        gamePiece = new GamePeace[ancho, alto];
        for (int i = 0; i < ancho; i++)
        {
            for (int g = 0; g < alto; g++)
            {
                GameObject go = PiezaAleatoria();
                UbicarPieza(go.GetComponent<GamePeace>(), i, g);
                go.transform.parent = transform;
                go.name = "Gamep(" + i + "," + g + ")";

                GamePeace ga = go.GetComponent<GamePeace>();
                ga.cordenadas(i, g);
            }
        }
    }

    public void SetInicialMause(Tile ini)
    {
        if (inicial = null)
        {
            this.inicial = ini;
        }

    }
    public void SetFinalTile(Tile fin)
    {
        if (inicial != null)
        {
            this.Final = fin;
        }
    }
    public void ReleaseTile()
    {
        if(inicial != null && Final != null)
        {
            cambioPieza(inicial, Final);
            inicial = null;
            Final = null;
        }
    }


    void cambioPieza(Tile initi, Tile and)
    {
        GamePeace Goin = gamePiece[initi.indiceX, initi.indiceY];
        GamePeace GoEnd = gamePiece[and.indiceX, and.indiceY];

        Goin.MoverPieza(and.indiceX, and.indiceY, 1f);
        GoEnd.MoverPieza(initi.indiceX, initi.indiceY, 1f);
    }
}



