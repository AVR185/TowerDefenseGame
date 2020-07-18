using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorJson {

    private static ContenedorTextos contenedorTextos;
    private const SystemLanguage idiomaPorDefecto = SystemLanguage.English;

    public static string ObtenerTexto(TextoId textoId, string textoActual)
    {
        string textoRetornar = string.Empty;

        //Si el idioma por defecto es el actual no cambia ningún texto
        if (idiomaPorDefecto == Application.systemLanguage)
        {
            return textoActual;
        }
        //Si el contendor de textos no está inicializado lo crea con el idioma del sistema actual
        else if (contenedorTextos == null)
        {
            ObtenerIdioma();
        }

        //Se comprueba por si no ha podido crear el objeto por no existir el fichero del idioma
        if(contenedorTextos != null)
        {
            textoRetornar = contenedorTextos.ObtenerTexto(textoId.ToString());
        }

        //Si está vacío le asigna el actual
        if (string.IsNullOrEmpty(textoRetornar))
        {
            textoRetornar = textoActual;
        }

        return textoRetornar;
                
    }
    
    private static void ObtenerIdioma()
    {
        Debug.Log("Idioma detectado: " + Application.systemLanguage.ToString());
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Spanish:
                LoadJsonFile(SystemLanguage.Spanish.ToString());
                break;
            default:
                LoadJsonFile(SystemLanguage.English.ToString());
                break;
        }
    }

    private static void LoadJsonFile(string name)
    {
        TextAsset asset = Resources.Load<TextAsset>(SystemLanguage.Spanish.ToString());
        if (asset != null)
        {
            contenedorTextos = JsonUtility.FromJson<ContenedorTextos>(asset.text);
        }
        else
        {
            Debug.LogWarning("No se ha encontrado el fichero " + name);
        }
        
    }
}
