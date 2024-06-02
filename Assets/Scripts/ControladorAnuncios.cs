using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class ControladorAnuncios : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static ControladorAnuncios instance;

    public string androidGameId;
    public string iOSGameId;

    public string idAnunciosAndroid;
    public string idAnunciosIOS;

    private string idSeleccionado;
    private string idAnuncioSeleccionado;

    public bool modoPruebas = true;

    public TipoRecompensaEnum tipoRecompensaSeleccioanda;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            IniciarAnuncios();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void IniciarAnuncios()
    {
        #if UNITY_ANDROID
            idSeleccionado = androidGameId;
            idAnuncioSeleccionado = idAnunciosAndroid;
        #elif UNITY_IOS
            idSeleccionado = iOSGameId;
            idAnuncioSeleccionado = idAnunciosIOS;
        #elif UNITY_EDITOR
            idSeleccionado = androidGameId;
            idAnuncioSeleccionado = idAnunciosAndroid;
        #endif

        if(!Advertisement.isInitialized)
        {
            Advertisement.Initialize(idSeleccionado, modoPruebas, this);
        }

    }

    public void OnInitializationComplete()
    {
        Debug.Log("Anuncios inicializados correctamente");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Error al inicializar anuncios");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(idAnuncioSeleccionado) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            //RECOMPENSA
            SeleccionarRecompensaAnuncio(tipoRecompensaSeleccioanda);
        }
    }

    public void MostrarAnuncio(TipoRecompensaEnum tipoRecompensa)
    {
        tipoRecompensaSeleccioanda = tipoRecompensa;
        
        Advertisement.Load(idAnuncioSeleccionado, this);
    }

    private void SeleccionarRecompensaAnuncio(TipoRecompensaEnum tipoRecompensa)
    {
        switch (tipoRecompensa)
        {
            case TipoRecompensaEnum.DuplicarScore:
                FindFirstObjectByType<GameManager>().DuplicarScoreDespuesDeAnuncio();
                break;
            case TipoRecompensaEnum.OtraOportunidad:
                FindFirstObjectByType<GameManager>().ResetGame();
                break;
        }
    }
}
