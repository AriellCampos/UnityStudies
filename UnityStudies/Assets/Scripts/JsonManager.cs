using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class JsonManager : MonoBehaviour
{
    //Declara��o das vari�veis
    [SerializeField] private List<CardData> CardDataList; // Lista de cartas
    string DataPath, NameFile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        NameFile = "cardData"; //O nome do arquivo JSON ser� "cardData.json".


        // Application.persistentDataPath � um caminho espec�fico onde os dados do jogo s�o armazenados.
        // No Windows, pode ser algo como: C:/Usu�rios/NomeDoUsuario/AppData/LocalLow/NomeDoJogo/cardData.json
        // No Android, pode ser algo como: /storage/emulated/0/Android/data/com.NomeDoJogo/files/cardData.
        // Isso garante que os dados sejam salvos e carregados corretamente em diferentes dispositivos

        DataPath = $"{Application.persistentDataPath}/{NameFile}.json";

        // Carregar os dados do JSON ou inicializar com dados padr�o se n�o existir
        CardDataList = LoadJsonFromDisk(DataPath) ?? new List<CardData>();
    }

    // Salva os dados das cartas no arquivo JSON
    private void SaveJsonToDisk(string path, List<CardData> cardDataList)
    {
        string json = JsonUtility.ToJson(new CardDataWrapper { cards = cardDataList }, true); // Formata��o "pretty" para facilitar leitura
        File.WriteAllText(path, json);
        Debug.Log("Dados das cartas salvos com sucesso!");
    }

    // Carrega os dados das cartas a partir de um arquivo JSON
    private List<CardData> LoadJsonFromDisk(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("Arquivo de dados das cartas n�o encontrado, criando um novo arquivo.");
            return null;  // Retorna nulo se o arquivo n�o existir
        }

        string json = File.ReadAllText(path);
        CardDataWrapper wrapper = JsonUtility.FromJson<CardDataWrapper>(json);
        return wrapper.cards;
    }

    // Adiciona uma nova carta � lista de cartas
    public void AddCard(string character, int cost, string attribute, int power, int counter, string color, string[] type, string effect, string cardSet)
    {
        if (CardDataList == null)
        {
            CardDataList = new List<CardData>();
        }

        CardDataList.Add(new CardData
        {
            Character = character,
            Cost = cost,
            Attribute = attribute,
            Power = power,
            Counter = counter,
            Color = color,
            Type = type,
            Effect = effect,
            CardSet = cardSet
        });

        // Atualiza a exibi��o dos dados e salva no disco
        SaveJsonToDisk(DataPath, CardDataList);
    }

    // Salva os dados quando o objeto for destru�do
    private void OnDestroy()
    {
        SaveJsonToDisk(DataPath, CardDataList);
    }

}

// Classe para armazenar a lista de caDrtas (para facilitar a serializa��o)
[Serializable]
public class CardDataWrapper
{
    public List<CardData> cards;
}

// Classe representando os dados de uma carta
[Serializable] 
public class CardData
{
    public string Character;
    public int Cost;
    public string Attribute;
    public int Power;
    public int Counter;
    public string Color;
    public string[] Type;
    public string Effect;
    public string CardSet;
}