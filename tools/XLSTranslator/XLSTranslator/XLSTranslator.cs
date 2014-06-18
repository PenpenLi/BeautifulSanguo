// Author : dandanshih
// Desc : 轉換程式
// [History]
// 2014/6/14 修改輸出目錄到 Client / Server
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XLSTranslator
{
    public partial class XLSTranslator : Form
    {
        public XLSTranslator()
        {
            InitializeComponent();
        }

        void ResetTextBox()
        {
            this.TextBox_FilenameList.Clear();
            m_listFilename.Clear();
        }

        List<string> m_listFilename = new List<string>();
        private void XLSTranslator_DragDrop(object sender, DragEventArgs e)
        {
            // 清除掉畫面
            ResetTextBox();
            string[] Files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string Filename in Files)
            {
                if (Filename.EndsWith(".xls") == false)
                    continue;
                m_listFilename.Add(Filename);
                string strFilename = Utility.GetFilename(Filename);
                this.TextBox_FilenameList.Text += strFilename;
            }
            // 做畫面上的更新
            this.Refresh();
        }
        private void XLSTranslator_DragEnter(object sender, DragEventArgs e)
        {
            // 確定使用者抓進來的是檔案
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                // 允許拖拉動作繼續 (這時滑鼠游標應該會顯示 +)
                e.Effect = DragDropEffects.All;
            }
        }

        // 做檔案轉換
        private void Button_Translate_Click(object sender, EventArgs e)
        {
            if (m_listFilename.Count == 0)
            {
                MessageBox.Show("請拖檔案進來處理");
                return;
            }
            // 檔案一個一個處理
            foreach (string strFilename in m_listFilename)
            {
                Dictionary<string, SingleExcelEx> dictExcel = ExcelExMgr.instance().GetExcelSheet(strFilename);
                List<string> listSheetName = new List<string>(dictExcel.Keys);
                foreach (string SheetName in listSheetName)
                {
                    SingleExcelEx Single = dictExcel[SheetName];
                    // 做存檔的動作 - Server
                    Single.SaveToJson("../Login/GameData/");
                    // 做存檔的動作 - Client
                    Single.SaveToJson("../../SanguoClient/Assets/JsonTxt/");
                }
            }
            MessageBox.Show("轉換成功");
        }
    }
}
