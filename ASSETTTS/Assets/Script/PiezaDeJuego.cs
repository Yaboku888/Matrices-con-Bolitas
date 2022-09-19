using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiezaDeJuego : MonoBehaviour
{
    // Start is called before the first frame update
    public int cordenadaX;
    public int cordinadaY;
    public float tiempoDeMovimiento;

    public bool enMovimiento;
    public TipoInterpolacion tipoDeInterpolo;
    public TipoFicha tipoFicha;
    public AnimationCurve curve;

    public Cuboo board;
    private void Start()
    {

    }
    public void Coordenada(int x, int y)
    {
        cordenadaX = x;
        cordinadaY = y;
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoverPieza(new Vector3 ((int)transform.position.x, (int)transform.position.y +1,0), tiempoDeMovimiento);
            //Debug.Log("arriba");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x, (int)transform.position.y - 1,0), tiempoDeMovimiento);
            //Debug.Log("abajo");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x+1, (int)transform.position.y ,0), tiempoDeMovimiento);
            //Debug.Log("derecha");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x-1, (int)transform.position.y ,0), tiempoDeMovimiento);
            //Debug.Log("izquierda");
        }
    }*/

    public void MoverPieza(int x, int y, float tiempoTrancurrido)
    {
        if (!enMovimiento)
        {
            StartCoroutine(MovePiece(new Vector3(x, y, 0), tiempoTrancurrido));
        }
    }
    IEnumerator MovePiece(Vector3 posicionFinal, float tiempoMovimiento)
    {
        enMovimiento = true;
        bool llegoAlpunto = false;
        Vector3 posicionInicial = transform.position;
        float tiempoTrancurrido = 0;


        while (!llegoAlpunto)
        {
            if (Vector3.Distance(transform.position, posicionFinal) < 0.01f)
            {
                llegoAlpunto = true;
                enMovimiento = false;
                transform.position = new Vector3((int)posicionFinal.x, (int)posicionFinal.y, 0);
                board.PiezaPosition(this, (int)posicionFinal.x, (int)posicionFinal.y);
                break;
            }
            float t = tiempoTrancurrido / tiempoMovimiento;

            switch (tipoDeInterpolo)
            {
                case TipoInterpolacion.Lineal:
                    t = curve.Evaluate(t);
                    break;

                case TipoInterpolacion.Salida:
                    t = Mathf.Sin(t * Mathf.PI * .5f);
                    break;

                case TipoInterpolacion.Entrada:
                    t = 1 - Mathf.Cos(t * Mathf.PI * .5f);
                    break;

                case TipoInterpolacion.Suavisado:
                    //movimiento suavisado
                    t = t * t * (3 - 2 * t);
                    break;

                case TipoInterpolacion.MasSuavisado:
                    //Mas suavisado
                    t = t * t * t * (t * (t * 6 - 15) + 10);
                    break;
            }

            tiempoTrancurrido += Time.deltaTime;
            transform.position = Vector3.Lerp(posicionInicial, posicionFinal, t);
            yield return new WaitForEndOfFrame();
        }
    }

    public enum TipoInterpolacion
    {
        Lineal,
        Entrada,
        Salida,
        Suavisado,
        MasSuavisado
    }

    public enum TipoFicha
    {
        Eye,
        Fire,
        Heart,
        Moon,
        Skull,
        Sun,
        Tree,
        Water

    }

}


