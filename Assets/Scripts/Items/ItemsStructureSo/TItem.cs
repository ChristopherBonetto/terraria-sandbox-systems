using System;
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

    [TextArea]
    [SerializeField] protected string m_descriptionField;

    [Space, SerializeField] private TextAsset m_TextToRead;
    public TextAsset TextToRead { get { return m_TextToRead; } private set { } }

    private string m_filePath;

    public List<string> m_textValues { get; private set; } = new List<string>();
    protected List<string> m_infos;




    public void Awake()
    {
        m_filePath = AssetDatabase.GetAssetPath(m_TextToRead);

        WriteAndReadDoc(TakeAllInfos());
    }

    public virtual List<string> TakeAllInfos()
    {
        m_infos = new List<string>();
        m_infos.Add("Item name : ");
        m_infos.Add(ItemName);
        m_infos.Add("");

        m_infos.Add("Description :");
        m_infos.Add(m_descriptionField);

        return m_infos;
    }


    public void WriteAndReadDoc(List<string> fullDescription)
    {
        try
        {
            if (!File.Exists(m_filePath))
            {
                return;
            }

            using (StreamWriter sw = new StreamWriter(m_filePath))
            {
                for(int i = 0; i < fullDescription.Count; i++)
                {
                    sw.WriteLine(fullDescription[i]);
                }
                sw.Close();
            }

            using (StreamReader sr = new StreamReader(m_filePath))
            {
                while (sr.Peek() >= 0)
                {
                    string tempLine = sr.ReadLine();
                    m_textValues.Add(tempLine);
                }
                sr.Close();
            }
        }
        catch (Exception exc)
        {
            Console.WriteLine("The process failed", exc.ToString());
        }
    }
    

    #endregion
}


