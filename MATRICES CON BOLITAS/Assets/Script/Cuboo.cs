using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cuboo : MonoBehaviour
{
    // Start is called before the first frame update

    public int alto;
    public int ancho;
    public Tile[,] board;
    public GameObject perfTile;
    public Camera cam;
    public int bordeCam;
    public PiezaDeJuego[,] piezas;
    public GameObject[] prefPiezass;


    [Range(0, 1)] public float swapTime = .3f;
    public bool puedeMover = true;

    public Tile inicioGp;
    public Tile finalGp;

    private void Start()
    {
        piezas = new PiezaDeJuego[ancho, alto];
        CrearBoard();
        OrganizaCamara();
        LlenarMatriz();
        //ResaltarConcidencias();

    }

    private void Update()
    {

    }
    void CrearBoard()
    {
        board = new Tile[ancho, alto];

        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                GameObject go = Instantiate(perfTile);
                go.name = "Tile" + x + "," + y;
                go.transform.position = new Vector3(x, y, 0);
                go.transform.parent = transform;
                Tile tile = go.GetComponent<Tile>();
                //cam.transform.position = new Vector3(j, i, -10)/2;
                tile.board = this;
                //go.GetComponent<Tile>().board = this;
                board[x, y] = tile;
                tile.Inicio(x, y);

                //cam.orthographicSize;


            }
        }
    }

    void OrganizaCamara()
    {
        cam.transform.position = new Vector3((float)ancho / 2 - .5f, (float)alto / 2 - .5f, -10);

        float a = ((float)alto / 2) + bordeCam;
        float an = ((float)ancho / 2 + bordeCam) / ((float)Screen.width / (float)Screen.height);


        /*if (an > a)
        {
            cam.orthographicSize = an;
        }
        else
        {
            cam.orthographicSize = a;

        }*/

        //Exprecion lambda
        cam.orthographicSize = a > an ? a : an;
    }


    GameObject PieazaAleatoria()
    {
        int numeroR = Random.Range(0, prefPiezass.Length);
        GameObject go = Instantiate(prefPiezass[numeroR]);
        go.GetComponent<PiezaDeJuego>().board = this;
        return go;
    }

    public void PiezaPosition(PiezaDeJuego gp, int x, int y)
    {
        gp.transform.position = new Vector3(x, y, 0);
        gp.Coordenada(x, y);
        piezas[x, y] = gp;
    }

    void LlenarMatriz()
    {
        List<PiezaDeJuego> addedPieces = new List<PiezaDeJuego>();
        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                if (piezas[x, y] == null)
                {
                    PiezaDeJuego gamePiece = LlenarMatrizAleatorioEn(x, y);
                    addedPieces.Add(gamePiece);
                }

            }
        }
        bool estaLlena = false;
        int interrraccion = 0;
        int interraccionesMaximas = 100;

        while (!estaLlena)
        {
            List<PiezaDeJuego> concidencias = EncontrarTodasLasConcidencias();

            if (concidencias.Count == 0)
            {
                estaLlena = true;
                break;
            }
            else
            {
                concidencias = concidencias.Intersect(addedPieces).ToList();
                ReemplazarConPiezaAleatoria(concidencias);
            }

            if (interrraccion > interraccionesMaximas)
            {
                estaLlena = true;
                Debug.LogWarning("Se alcanzo el numero maximo de interacciones");
                break;

            }
            interrraccion++;


        }
    }

    private void ReemplazarConPiezaAleatoria(List<PiezaDeJuego> concidencias)
    {
        foreach (PiezaDeJuego gamePieces in concidencias)
        {
            ClearPieceAt(gamePieces.cordenadaX, gamePieces.cordinadaY);
            LlenarMatrizAleatorioEn(gamePieces.cordenadaX, gamePieces.cordinadaY);
        }

    }

    PiezaDeJuego LlenarMatrizAleatorioEn(int x, int y)
    {
        GameObject go = PieazaAleatoria();
        PiezaPosition(go.GetComponent<PiezaDeJuego>(), x, y);
        return go.GetComponent<PiezaDeJuego>();
    }

    public void SetInicialMouse(Tile ini)
    {
        if (inicioGp == null)
        {
            inicioGp = ini;
        }


    }

    public void SetEndMouse(Tile fin)
    {
        if (inicioGp != null && EsVecino(inicioGp, fin) == true)
        {
            finalGp = fin;
        }

    }

    public void ReleaseTile()
    {
        if (inicioGp != null && finalGp != null)
        {
            SwitchPiece(inicioGp, finalGp);
        }
        inicioGp = null;
        finalGp = null;
    }

    public void SwitchPiece(Tile inicio, Tile final)
    {
        StartCoroutine(SwitchTilesCoroutine(inicio, final));
    }

    IEnumerator SwitchTilesCoroutine(Tile inicio, Tile final)
    {
        if (puedeMover)
        {

            puedeMover = false;
            PiezaDeJuego gpIni = piezas[inicio.indiceX, inicio.indiceY];
            PiezaDeJuego gpFin = piezas[final.indiceX, final.indiceY];

            if (gpIni != null && gpFin != null)
            {
                gpIni.MoverPieza(final.indiceX, final.indiceY, swapTime);
                gpFin.MoverPieza(inicio.indiceX, inicio.indiceY, swapTime);

                yield return new WaitForSeconds(swapTime);

                List<PiezaDeJuego> listaInicio = EncontarConcidenciasEn(inicio.indiceX, inicio.indiceY);
                List<PiezaDeJuego> listaFinal = EncontarConcidenciasEn(final.indiceX, final.indiceY);

                //var listaCombinada = listaInicio.Union(listaFinal).ToList();

                if (listaInicio.Count == 0 && listaFinal.Count == 0)
                {
                    gpIni.MoverPieza(inicio.indiceX, inicio.indiceY, swapTime);
                    gpFin.MoverPieza(final.indiceX, final.indiceY, swapTime);
                    yield return new WaitForSeconds(swapTime);
                    puedeMover = true;
                }
                else
                {
                    listaInicio = listaInicio.Union(listaFinal).ToList();
                    ClearAndRefillBoard(listaInicio);
                }
            }
        }


    }

    bool EsVecino(Tile inicial, Tile final)
    {

        if (Mathf.Abs(inicial.indiceX - final.indiceX) == 1 && inicial.indiceY == final.indiceY)
        {
            return true;
        }
        else
        {
            if (Mathf.Abs(inicial.indiceY - final.indiceY) == 1 && inicial.indiceX == final.indiceX)
            {
                return true;
            }

            else
            {
                return false;
            }
        }


    }

    bool estaEnRango(int _x, int _y)
    {
        return (_x < ancho && _x >= 0 && _y < alto && _y >= 0);
    }

    List<PiezaDeJuego> EncontrarCoincidencias(int startX, int startY, Vector2 direccionDelBuscada, int cantidadMinima = 3)
    {
        //Crear una lista de concidencias encontradas
        List<PiezaDeJuego> concidencias = new List<PiezaDeJuego>();

        //Crear una referencia al gamepiece inicial
        PiezaDeJuego piezaInicial = null;

        if (estaEnRango(startX, startY))
        {
            piezaInicial = piezas[startX, startY];
        }
        if (piezaInicial != null)
        {
            concidencias.Add(piezaInicial);
        }
        else
        {
            return null;
        }

        int siguienteX;
        int siguienteY;

        int valorMaximo = ancho > alto ? ancho : alto;

        for (int i = 1; i < valorMaximo - 1; i++)
        {
            siguienteX = startX + (int)Mathf.Clamp(direccionDelBuscada.x, -1, 1) * i;
            siguienteY = startY + (int)Mathf.Clamp(direccionDelBuscada.y, -1, 1) * i;

            if (!estaEnRango(siguienteX, siguienteY))
            {
                break;
            }

            PiezaDeJuego siguientePieza = piezas[siguienteX, siguienteY];

            if (siguientePieza == null)
            {
                break;
            }
            else
            {


                //Compara si las piezas inicial y final son del mismo tipo
                if (piezaInicial.tipoFicha == siguientePieza.tipoFicha && !concidencias.Contains(siguientePieza))
                {
                    concidencias.Add(siguientePieza);
                }
                else
                {
                    break;
                }
            }

        }
        if (concidencias.Count >= cantidadMinima)
        {
            return concidencias;
        }
        return null;
    }

    List<PiezaDeJuego> BusquedaVertical(int startX, int startY, int cantidadMinima = 3)
    {
        List<PiezaDeJuego> arriba = EncontrarCoincidencias(startX, startY, Vector2.up, 2);
        List<PiezaDeJuego> abajo = EncontrarCoincidencias(startX, startY, Vector2.down, 2);

        if (arriba == null)
        {
            arriba = new List<PiezaDeJuego>();
        }

        if (abajo == null)
        {
            abajo = new List<PiezaDeJuego>();
        }

        var listasConvinadas = arriba.Union(abajo).ToList();

        return listasConvinadas.Count >= cantidadMinima ? listasConvinadas : null;
    }

    List<PiezaDeJuego> BusquedaHorizontal(int startX, int startY, int cantidadMinima = 3)
    {
        List<PiezaDeJuego> derecha = EncontrarCoincidencias(startX, startY, Vector2.right, 2);
        List<PiezaDeJuego> izquierda = EncontrarCoincidencias(startX, startY, Vector2.left, 2);

        if (derecha == null)
        {
            derecha = new List<PiezaDeJuego>();
        }

        if (izquierda == null)
        {
            izquierda = new List<PiezaDeJuego>();
        }

        var listasConvinadas = derecha.Union(izquierda).ToList();

        return listasConvinadas.Count >= cantidadMinima ? listasConvinadas : null;
    }

    public void ResaltarConcidencias()
    {
        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                ResaltarConcidenciaEn(x, y);
            }
        }
    }

    private void ResaltarConcidenciaEn(int _x, int _y)
    {
        var listasCombinadas = EncontarConcidenciasEn(_x, _y);


        if (listasCombinadas.Count > 0)
        {
            foreach (PiezaDeJuego piezas in listasCombinadas)
            {

                ResaltarTile(piezas.cordenadaX, piezas.cordinadaY, piezas.GetComponent<SpriteRenderer>().color);
            }
        }
    }

    private void ResaltarTile(int _x, int _y, Color _col)
    {
        SpriteRenderer sr = board[_x, _y].GetComponent<SpriteRenderer>();
        sr.color = _col;
    }

    private List<PiezaDeJuego> EncontarConcidenciasEn(int x, int y)
    {
        List<PiezaDeJuego> horizontal = BusquedaHorizontal(x, y);
        List<PiezaDeJuego> vertical = BusquedaVertical(x, y);

        if (horizontal == null)
        {
            horizontal = new List<PiezaDeJuego>();
        }

        if (vertical == null)
        {
            vertical = new List<PiezaDeJuego>();
        }

        var listasCombinadas = horizontal.Union(vertical).ToList();

        return listasCombinadas;


    }

    private List<PiezaDeJuego> EncontarConcidenciasEn(List<PiezaDeJuego> gamePiece, int minLength = 3)
    {
        List<PiezaDeJuego> matches = new List<PiezaDeJuego>();

        foreach (PiezaDeJuego gp in gamePiece)
        {
            matches = matches.Union(EncontarConcidenciasEn(gp.cordenadaX, gp.cordinadaY)).ToList();
        }

        return matches;
    }

    private List<PiezaDeJuego> EncontrarTodasLasConcidencias()
    {
        List<PiezaDeJuego> todasLasConcidencias = new List<PiezaDeJuego>();

        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                var concidencias = EncontarConcidenciasEn(x, y);
                todasLasConcidencias = todasLasConcidencias.Union(concidencias).ToList();
            }
        }
        return todasLasConcidencias;
    }

    void ClearBoard(List<PiezaDeJuego> gamePieces)
    {
        for (int x = 0; x < ancho; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                ClearPieceAt(x, y);
            }
        }
    }

    private void ClearPieceAt(int x, int y)
    {
        PiezaDeJuego pieceToClear = piezas[x, y];
        if (pieceToClear != null)
        {
            piezas[x, y] = null;
            Destroy(pieceToClear.gameObject);
        }
    }

    private void ClearPieceAt(List<PiezaDeJuego> gamePiece)
    {
        foreach (PiezaDeJuego gp in gamePiece)
        {
            if (gp != null)
            {
                ClearPieceAt(gp.cordenadaX, gp.cordinadaY);
            }
        }
    }

    List<PiezaDeJuego> CollapseCollum(int column, float colpseTime = 0.1f)
    {
        List<PiezaDeJuego> movingPieces = new List<PiezaDeJuego>();
        for (int i = 0; i < alto - 1; i++)
        {
            if (piezas[column, i] == null)
            {
                for (int j = i + 1; j < alto; j++)
                {
                    if (piezas[column, j] != null)
                    {
                        piezas[column, j].MoverPieza(column, i, colpseTime * (j - i));
                        piezas[column, i] = piezas[column, j];
                        piezas[column, j].Coordenada(column, i);

                        if (!movingPieces.Contains(piezas[column, i]))
                        {
                            movingPieces.Add(piezas[column, i]);
                        }
                        piezas[column, j] = null;
                        break;
                    }
                }
            }
        }
        return movingPieces;
    }

    List<PiezaDeJuego> CollapseCollum(List<PiezaDeJuego> gamePieces)
    {
        List<PiezaDeJuego> movengPiece = new List<PiezaDeJuego>();
        List<int> columnsToCollapse = GetColumns(gamePieces);

        foreach (int column in columnsToCollapse)
        {
            movengPiece = movengPiece.Union(CollapseCollum(column)).ToList();
        }

        return movengPiece;
    }

    List<int> GetColumns(List<PiezaDeJuego> gamePieces)
    {
        List<int> collumnsIndex = new List<int>();

        foreach (PiezaDeJuego gamePiece in gamePieces)
        {
            if (!collumnsIndex.Contains(gamePiece.cordenadaX))
            {
                collumnsIndex.Add(gamePiece.cordenadaX);
            }
        }

        return collumnsIndex;
    }

    void ClearAndRefillBoard(List<PiezaDeJuego> gamePieces)
    {
        StartCoroutine(ClearAndRefillBoardRoutine(gamePieces));
    }

    IEnumerator ClearAndRefillBoardRoutine(List<PiezaDeJuego> gamePieces)
    {
        yield return StartCoroutine(CleanAndCollapseColumn(gamePieces));
        yield return null;
        yield return StartCoroutine(RefillRoutine());
        puedeMover = true;
    }
    IEnumerator CleanAndCollapseColumn(List<PiezaDeJuego> gamePiece)
    {
        List<PiezaDeJuego> movingPiece = new List<PiezaDeJuego>();
        List<PiezaDeJuego> matches = new List<PiezaDeJuego>();

        bool isFinished = false;

        while (!isFinished)
        {
            ClearPieceAt(gamePiece);
            yield return new WaitForSeconds(.5f);
            movingPiece = CollapseCollum(gamePiece);

            while (!isColapse(gamePiece))
            {
                yield return new WaitForEndOfFrame();
            }

            matches = EncontarConcidenciasEn(movingPiece);

            if (matches.Count == 0)
            {
                isFinished = true;
                break;
            }
            else
            {
                yield return StartCoroutine(CleanAndCollapseColumn(matches));
            }
        }
    }

    IEnumerator RefillRoutine()
    {
        LlenarMatriz();
        yield return null;
    }

    bool isColapse(List<PiezaDeJuego> gamePiece)
    {
        foreach (PiezaDeJuego gp in gamePiece)
        {
            if (gp != null)
            {
                if (gp.transform.position.y - (float)gp.cordinadaY > 0.001f)
                {
                    return false;
                }
            }
        }
        return true;

    }

}






