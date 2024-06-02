using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnuncios : MonoBehaviour
{
    public void AnuncioPuntosDobles()
    {
        ControladorAnuncios.instance.MostrarAnuncio(TipoRecompensaEnum.DuplicarScore);
    }
}
