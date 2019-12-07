using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Item/Generic Item")]
public class TItem : ScriptableObject
{
    public bool DepleteOnUse { get { return m_DepleteOnUse; } }

    [Space]
    public new string ItemName = "";

    [Space]
    public Sprite ItemSprite;

    [SerializeField] private bool m_DepleteOnUse;



    #region Read Description

    [Space, SerializeField] private TextAsset m_TextToRead;
    private string path;

    public List<string> values { get; private set; } = new List<string>();

    public void Awake()
    {
        path = AssetDatabase.GetAssetPath(m_TextToRead);

        if (File.Exists(path))
        {
            ReadTextFile(path);
        }

    }

    void ReadTextFile(string file_path)
    {
        StreamReader inp_stm = new StreamReader(file_path);

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            values.Add(inp_ln);
        }
        inp_stm.Close();
    }

    #endregion
}


