using Cfg;
using UnityEngine;

namespace Cfg.railway
{
    public static partial class TableManager
    {
        /// <summary>
        /// 按ID获取表格中的数据  TableManager.Tables.TbDistricts.GetById(1000);
        /// 按序号获取表格中的数据  TableManager.Tables.TbDistricts[0];
        /// </summary>
        
        //2023/2/27 从railway拆
        // private static Tables Tables;
        //
        public static void Init()
        {
            // Tables = new Tables(file => 
            //     SimpleJSON.JSON.Parse(Resources.Load<TextAsset>("Data/" + file).text)
            // );
        }

    }
}