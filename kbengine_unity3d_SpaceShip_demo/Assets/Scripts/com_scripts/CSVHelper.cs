using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Excel;
using System.Data;
using System.CodeDom;
using CONF;

public class CSVHelper : Singleton<CSVHelper>
{
    bool fileNameLoad = false;

    Dictionary<string, conf_base> file_conf = new Dictionary<string, conf_base>();

    Dictionary<string, Dictionary<UInt32, object>> conf_dic = new Dictionary<string, Dictionary<uint, object>>();

//    string filePath = Application.dataPath +"/../Pubcsv/";

    // Use this for initialization

    public static string GetFilePath()
    {
        string path = Application.dataPath;
        int num = path.LastIndexOf("/");
        path = path.Substring(0, num);
        return path;
    }

    void InitFileNames()
    {
        fileNameLoad = true;
        file_conf["conf_bullet"] = new conf_bullet();
    }

    int LoadOneExcel(string table)
    {
        if(fileNameLoad == false)
        {
            InitFileNames();
        }

        if (!file_conf.ContainsKey(table))
        {
            Unity.Logout.Log("LoadOneExcel:table:"+table);
            return -1;
        }
        
        if(file_conf[table].init)
        {
            Unity.Logout.Log("LoadOneExcel:file_conf[table].init:" + table + ",init:"+ file_conf[table].init);
            return 0;
        }

        string fileName = GetFilePath() + "/Pubcsv/"  + table + ".xls";

        if (File.Exists(fileName))
        {
            conf_dic[table] = file_conf[table].ReadOneExcel(fileName);
            return 1;
        }
        else
        {
            Debug.LogError(fileName + " can not found!!!");
            Unity.Logout.Log("LoadOneExcel:fileName:" + fileName + " can not found");
            return -1;
        }

    }

    public IEnumerator ReadExcelAllFile()
    {
        if (fileNameLoad == false)
        {
            InitFileNames();
        }

        foreach (KeyValuePair<string, conf_base> item in file_conf)
        {
            yield return new WaitForFixedUpdate();
            LoadOneExcel(item.Key);
        }
    }

    public object GetItem(string table , UInt32 nID)
    {
        if (fileNameLoad == false)
        {
            InitFileNames();
        }

        if (file_conf.ContainsKey(table) ==false)
        {
            Debug.LogError("file_conf not found " + table);
            return null;
        }

        if(file_conf[table].init)
        {
            if(conf_dic.ContainsKey(table)&& conf_dic[table].ContainsKey(nID))
            {
                return conf_dic[table][nID];
            }
            else
            {
                Debug.LogError("conf_dic not found " + table);
                return null;
            }
        }
        else
        {
            if(LoadOneExcel(table) > 0)
            {
                if (conf_dic.ContainsKey(table) && conf_dic[table].ContainsKey(nID))
                {
                    return conf_dic[table][nID];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public Dictionary<UInt32, object> GetTable(string table)
    {
        if (fileNameLoad == false)
        {
            InitFileNames();
        }

        if (file_conf.ContainsKey(table) == false)
        {
            Debug.LogError("file_conf not found " + table);
            
            return null;
        }

        if (file_conf[table].init)
        {
            if (conf_dic.ContainsKey(table))
            {
                return conf_dic[table];
            }
            else
            {
                Debug.LogError("conf_dic not found " + table);
                return null;
            }
        }
        else
        {
            if (LoadOneExcel(table) > 0)
            {
                if (conf_dic.ContainsKey(table) )
                {
                    return conf_dic[table];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

}
