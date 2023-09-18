using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class Grafo
{
    private Nodo nodoRaiz;

    public Nodo NodoRaiz { get => nodoRaiz; set => nodoRaiz = value; }

    public Grafo(Nodo newNodoRaiz)
    {
        NodoRaiz = newNodoRaiz;
    }
    public Grafo()
    {
        NodoRaiz = null;
    }
    private ref Nodo searchNodo(Nodo findNodo, out bool found)
    {
        if (NodoRaiz == findNodo)
        {
            found = true;
            return ref nodoRaiz;
        }
        foreach (Nodo sucesor in NodoRaiz.Successors)
        {

            Grafo newGrafo = new Grafo(sucesor);
            ref Nodo fNodo = ref newGrafo.searchNodo(findNodo, out found);
            if(found)
                return ref fNodo;

        }
        found = false;
        return ref nodoRaiz;
    }
    public bool activateNode(Nodo findNodo, ref List<Nodo> listaActivos)
    {
        bool found;
        Nodo foundNodo = searchNodo(findNodo, out found);

        if (!found || !isActivable(foundNodo))
            return false;


        foundNodo.IsActive = true;
        
        listaActivos.Add(foundNodo);
        UnityEngine.Debug.Log("Se activó: " + foundNodo.variableId);
        return true;
    }
    public bool activateNode(Nodo findNodo)
    {

        if (!isActivable(findNodo))
            return false;

        findNodo.IsActive = true;

        return true;
    }
    public bool deactivateNode(Nodo findNodo, ref List<Nodo> listaActivos)
    {
        bool found;
        Nodo foundNodo = searchNodo(findNodo, out found);

        if (!found)
            return false;
        if (foundNodo.VariableStatus)
            return false;
        foundNodo.IsActive = false;
        foundNodo.VariableStatus = true;
        bool nodoRemoved = listaActivos.Remove(foundNodo);

        if (nodoRemoved)
        {
            foreach (Nodo sucesor in foundNodo.Successors)
            {
                if (activateNode(sucesor))
                    listaActivos.Add(sucesor);
            }
            //if(foundNodo.IsOptional)
            //{
            foreach (Nodo predecesor in foundNodo.Predecessors)
            {
                if (predecesor.IsActive && predecesor.IsOptional)
                    deactivateNode(predecesor, ref listaActivos);
            }
            //}
            return true;
        }

        return false;
    }
    public bool isActivable(Nodo nodo)
    {
        if (nodo.IsActive)
            return false;
        foreach (Nodo predecesor in nodo.Predecessors)
        {
            //if ((predecesor.IsActive || predecesor.VariableStatus) && (!predecesor.IsOptional))
            if ((predecesor.IsActive || !predecesor.VariableStatus) && !predecesor.IsOptional)
                return false;
        }

        return true;
    }
}
