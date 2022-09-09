using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
                                                    
public class Cuboo : MonoBehaviour
{
    // Start is called before the first frame update

    public int ancho;
    public int alto;
    public float borde;

    public GameObject prefTile;
    public Camera cameraPlayer;
    public GameObject[] prefpuntos;
    public GamePeace[,] gamepiece;
    public Tile[,] board;

    public Tile final;
    public Tile inicial;



    void Start()
    {
        llenarMatriz();
        CrearBoard();
        organizar();

    }
    void CrearBoard()
    {
        board = new Tile[ancho, alto];

        for (int i = 0; i < ancho; i++)
        {

            for (int j = 0; j < alto; j++)
            {
                GameObject go = Instantiate(prefTile);
                go.name = "Tile(" + i + "," + j + ")";
                go.transform.position = new Vector3(i, j, 0);
                go.transform.parent = transform;

                Tile tile = go.GetComponent<Tile>();
                tile.board = this;
                tile.Inicializar(i, j);
                board[i, j] = tile;

                cameraPlayer.transform.localPosition = new Vector3(i / 2f, j / 2f, -5   );

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
    public GameObject PiezAleatoria()
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
        gamepiece[x, y] = gp;
    }
    void llenarMatriz()
    {
        gamepiece = new GamePeace[ancho, alto];



        for (int i = 0; i < ancho; i++)
        {
            for (int g = 0; g < alto; g++)
            {
                GameObject go = PiezAleatoria();
                UbicarPieza(go.GetComponent<GamePeace>(), i, g);
                go.transform.parent = transform;
                go.name = "Gamep(" + i + "," + g + ")";

                GamePeace ga = go.GetComponent<GamePeace>();
                ga.cordenadas(i, g);
            }
        }
    }
    public void SetInicialTile(Tile ini)
    {
        if (inicial == null)
        {
            this.inicial = ini;
        }
    }
    public void SetFinalTile(Tile fin)
    {
        if (inicial != null && vecino(inicial, fin) == true)
        {
            final = fin;
        }
    }
    public void ReleaseTile()
    {
        if (inicial != null && final != null)
        {
            cambioPieza(inicial, final);
            inicial = null;
            final = null;
        }

    }

    bool vecino(Tile _Inicial, Tile _Final)
    {
        if (Mathf.Abs(_Inicial.indiceX - _Final.indiceY) == 1 && _Inicial.indiceY == _Final.indiceY)
        {
            return true;
        }
        if (Mathf.Abs(_Inicial.indiceY - _Final.indiceY) == 1 && _Inicial.indiceX == _Final.indiceX)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool EstaEnRango(int _X, int _Y)
    {
        return (_X < ancho && _X >= 0 && _Y >= 0 && _Y < alto);
    }

    List<GamePeace> EncontrarCoincidencias(int stratX, int stratY, Vector2 direccionDeBusqueda, int catidaminima = 3)
    {
        //crear una lista de coincedencias encontradas 

        List<GamePeace> Coincidencias = new List<GamePeace>();
        // crear una referencia añ gamepiece inicial
        GamePeace piezaInicial = null;
        if (EstaEnRango(stratX, stratY))
        {
            piezaInicial = gamepiece[stratX, stratY];
        }
        if (piezaInicial != null)
        {
            Coincidencias.Add(piezaInicial);
        }
        else
        {
            return null;
        }
        int siguenteX;
        int SiguenteY;

        int valorMaixmo = ancho > alto ? ancho : alto;

        for (int i = 1; i < valorMaixmo - 1; i++)
        {
            siguenteX = stratX + (int)Mathf.Clamp(direccionDeBusqueda.x, -1, 1) * i;
            SiguenteY = stratY + (int)Mathf.Clamp(direccionDeBusqueda.y, -1, 1) * i;

            if (!EstaEnRango(siguenteX, SiguenteY))
            {
                break;
            }
            GamePeace siguientePieza = gamepiece[siguenteX, SiguenteY];

            //comprobar si las piezas inical y final son del mismo típo 

            if (piezaInicial.TipoFicha_ == siguientePieza.TipoFicha_ && !Coincidencias.Contains(siguientePieza))
            {
                Coincidencias.Add(siguientePieza);
            }
            else
            {
                break;
            }
        }
        if (Coincidencias.Count >= catidaminima)
        {
            return Coincidencias;
        }
        return null;
    }
    List<GamePeace> BusquedaVertical(int stratX, int stratY, int catidadMinima = 1)
    {
        List<GamePeace> arriba = EncontrarCoincidencias(stratX, stratY, Vector2.up, 2);
        List<GamePeace> abajo = EncontrarCoincidencias(stratX, stratY, Vector2.down, 2);

        if (arriba == null)
        {
            arriba = new List<GamePeace>();
        }
        if (abajo == null)
        {
            abajo = new List<GamePeace>();
        }
        var listasCombinaadas = arriba.Union(abajo).ToList();
        return listasCombinaadas.Count >= catidadMinima ? listasCombinaadas : null;
    }
    List<GamePeace> BusquedaHorizontal(int stratX, int stratY, int catidadMinima = 1)
    {
        List<GamePeace> arriba = EncontrarCoincidencias(stratX, stratY, Vector2.down, 2);
        List<GamePeace> abajo = EncontrarCoincidencias(stratX, stratY, Vector2.up, 2);

        if (arriba == null)
        {
            arriba = new List<GamePeace>();
        }
        if (abajo == null)
        {
            abajo = new List<GamePeace>();
        }
        var listasCombinaadas = arriba.Union(abajo).ToList();
        return listasCombinaadas.Count >= catidadMinima ? listasCombinaadas : null;
    }
    void cambioPieza(Tile inicial2, Tile final2)
    {
        GamePeace inicialgp = gamepiece[inicial2.indiceX, inicial2.indiceY];
        GamePeace finalgp = gamepiece[final2.indiceX, final2.indiceY];

        inicialgp.MoverPieza(final2.indiceX, final2.indiceY, 1f);
        finalgp.MoverPieza(inicial2.indiceX, inicial2.indiceY, 1f);
    }

}





