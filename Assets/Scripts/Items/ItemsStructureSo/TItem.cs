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
    [Space]
    public new string ItemName = "";

    [Space]
    public Sprite ItemSprite;

    //Boolean used to check if it can decreases his amount.
    [SerializeField] private bool m_DepleteOnUse;
    public bool DepleteOnUse { get { return m_DepleteOnUse; } }


    #region Read Description
    
    //Description field.
    [TextArea, SerializeField] protected string m_descriptionField;

    //Document used to write all descriptio of an item.
    [Space, SerializeField] private TextAsset m_textRef;
    public TextAsset TextToRead { get { return m_textRef; } protected set { } }

    //String to contains the path of the TextDoc.
    private string m_filePath;

    //This list will contains all the words for the description.
    public List<string> m_textValues { get; private set; } = new List<string>();

    //Temporary list used to store all the item's infos.
    protected List<string> m_infos;




    public void Awake()
    {
        //#if UNITY_EDITOR
        //m_filePath = AssetDatabase.GetAssetPath(m_TextToRead);

        //WriteAndReadDoc(TakeAllInfos());
        //#endif

        m_textValues = TakeAllInfos();
    }

    /// <summary>
    /// Used to fill <param m_infos> with the item's info.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Detect if the TextDoc file Exist. If return true so start to write on it all the infos.
    /// After that fill <param m_textValues> List with all the words finded.
    /// </summary>
    public void WriteAndReadDoc(List<string> fullDescription)
    {
        //Try to find do these operations
        try
        {
            if (!File.Exists(m_filePath))
            {
                Debug.Log("no");
                return;
            }

            //Using StreamWriter this is used to writes all the info into the TextDoc.
            using (StreamWriter sw = new StreamWriter(m_filePath))
            {
                for(int i = 0; i < fullDescription.Count; i++)
                {
                    sw.WriteLine(fullDescription[i]);
                }
                sw.Close();
            }

            //Using StreamReader this is used to reads all the info into the TextDoc.
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
        //Debug if this method failed.
        catch (Exception exc)
        {
            Console.WriteLine("The process failed", exc.ToString());
        }
    }

    #endregion
}


