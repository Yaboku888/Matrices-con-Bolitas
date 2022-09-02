using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePeace : MonoBehaviour
{
    public int indicex;
    public int indicey;
    public float tiempoDMV;


    public bool yaSeEjecuto = true;
    public TipoInterpolacion tipoDeInterpolo;
    public AnimationCurve curve;
    //Vector3 startpos;
    //Vector3 endpos;
    //[Range(0f, 1f)] float t;
   
   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x,(int)transform.position.y +1, 0), tiempoDMV);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x, (int)transform.position.y  -1, 0), tiempoDMV);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x, (int)transform.position.y , 0), tiempoDMV);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoverPieza(new Vector3((int)transform.position.x, (int)transform.position.y , 0), tiempoDMV);
        }

        //transform.position = Vector3.Lerp(startpos, endpos, t);
    }

    public void Inicializar(int x, int y)
    {
        indicex = x;
        indicey = y;
    }

    IEnumerator MovePiece(Vector3 posicionFinal, float tiempoMv)
    {
        yaSeEjecuto = false;
        bool llegoAlpunto = false;
        Vector3 posicionInicial = transform.position;
        float tiempoTranscurrido = 0;

        while (!llegoAlpunto)
        {
            if (Vector3.Distance (posicionInicial, posicionFinal)<0.01f)
            {
                llegoAlpunto = true;
                yaSeEjecuto = true;
                transform.position = new Vector3((int)posicionFinal.x, (int)posicionFinal.y, 0);
                break;
            }
            float t = tiempoTranscurrido / tiempoMv;

            switch (tipoDeInterpolo)
            {
                case TipoInterpolacion.Lineal:
                    t = curve.Evaluate(t);
                    break;

                case TipoInterpolacion.Salida:
                    t = 1- Mathf.Cos(t * Mathf.PI * .5f);
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
            tiempoMv += Time.deltaTime;
            transform.position = Vector3.Lerp(posicionInicial, posicionFinal, t);
            yield return new WaitForEndOfFrame();


         
        }

    }  
            void MoverPieza(Vector3 posicionFinal, float tiempoMv)
            {
                if (yaSeEjecuto ==true)
                {
                    StartCoroutine(MovePiece(posicionFinal, tiempoMv));
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


}

