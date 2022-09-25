using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class columnas : MonoBehaviour
{
    public bool SeMueve, subiendo;

    public float velocity, TopPosition, MinPosition, MaxPosition;

    public Vector3 PosicionInicial, NuevaPosición;
    void Start()
    {
        PosicionInicial = transform.position;
        TopPosition = Random.Range(MinPosition, MaxPosition);
        if (SeMueve == true) 
        { Mover(); }
    }
    
    public void Mover()
    {
        NuevaPosición = PosicionInicial + new Vector3(0, TopPosition, 0);
        subiendo = true;
    }

    void Update()
    {
         if (transform.position == NuevaPosición)
        {
            subiendo = false;
          
        }
         if(transform.position == PosicionInicial)
        {
            subiendo = true;
        }
        if ( subiendo == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, NuevaPosición, velocity * Time.deltaTime);
        }
        if (subiendo == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, PosicionInicial, velocity * Time.deltaTime);
        }
       
       
    }
    
}
