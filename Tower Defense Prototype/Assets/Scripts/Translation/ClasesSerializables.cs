using System.Collections.Generic;

[System.Serializable]
public class ContenedorTextos
{
    public List<Texto> textos;

    public string ObtenerTexto(string textoId)
    {
        for (int i = 0; i < textos.Count; i++)
        {
            if (textos[i].id == textoId)
                return textos[i].contenido;
        }

        return string.Empty;
    }
}

[System.Serializable]
public class Texto
{
    public string id;
    public string contenido;
}
