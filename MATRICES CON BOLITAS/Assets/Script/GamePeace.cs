using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePeace : MonoBehaviour
{
    // Start is called before the first frame update
    public int indiceX;
    public int indiceY;
    public float tiempoDMV;

    public bool enmovimiento = false;
    public TipoInterpolacion tipoDeInterpolo;
    public TipoFicha TipoFicha_;
    public AnimationCurve curve;
    public Cuboo board;
    /*void Update()
    {
       /* if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x,(int)transform.position.y +1,0), tiempoDMV);
            Debug.Log("arriba");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x, (int)transform.position.y -1, 0), tiempoDMV);
            Debug.Log("Abajo");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x+1, (int)transform.position.y , 0), tiempoDMV);
            Debug.Log("derecha");

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x-1, (int)transform.position.y , 0), tiempoDMV);
            Debug.Log("Izquierda");
        }*/



    public void cordenadas(int x, int y)
    {
        indiceX = x;
        indiceY = y;
    }

    IEnumerator MovePiece(Vector3 posicionFinal, float tiempoMv)
    {
        enmovimiento = true;
        bool llegoAlpunto = false;
        Vector3 posicionInicial = new Vector3((int)transform.position.x, (int)transform.position.y, 0);
        float tiempoTrascurrido = 0;

        while (!llegoAlpunto)
        {

            if (Vector3.Distance(transform.position, posicionFinal) < 0.01f)
            {
                enmovimiento = false;
                llegoAlpunto = true;
                board.UbicarPieza(this, (int)posicionFinal.x, (int)posicionFinal.y);
                transform.position = new Vector3((int)posicionFinal.x, (int)posicionFinal.y, 0);
                break;
            }
            float t = tiempoTrascurrido / tiempoMv;

            switch (tipoDeInterpolo)
            {
                case TipoInterpolacion.Lineal:
                    t = curve.Evaluate(t);
                    break;

                case TipoInterpolacion.Salida:
                    t = 1 - Mathf.Cos(t * Mathf.PI * .5f);
                    break;

                case TipoInterpolacion.Entrada:
                    t = Mathf.Cos(t * Mathf.PI * .5f);
                    break;

                case TipoInterpolacion.Suavisado:
                    t = t * t * (3 - 2 * t);
                    break;

                case TipoInterpolacion.MasSuavisado:
                    t = t * t * t * (t * (t * 6 - 15) + 10);
                    break;
            }
            tiempoTrascurrido += Time.deltaTime;
            transform.position = Vector3.Lerp(posicionInicial, posicionFinal, t);
            yield return new WaitForEndOfFrame();
        }
    }

    public void MoverPieza(int x, int y, float tiempoMOV)
    {
        if (!enmovimiento)
        {
            StartCoroutine(MovePiece(new Vector3(x, y), tiempoMOV));
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
        Verde,
        Amarillo,
        Cafe,
        Azul,
        Gris,
        verdeOsucro,
        rosado,
        rojo
    }


}


