using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace DupliFilesComparer
{
    public partial class Form1 : Form
    {
        public static Hashtable config = new Hashtable();
        public Form1()
        {
            InitializeComponent();
            Form1.CheckForIllegalCrossThreadCalls = false;

            if (DLL.main.FileSearch("./database") == false)
            {
                DLL.main.ForderCreate("./database");
            }

            //nao0x0.flag.debug = true;

            //def["replace_after_filename"] = textBox4_replace_after_filename.Text;
            //def["date_format"] = textBox1_date_format.Text;

            DLL.main.appcheck(DLL.main.myappname());
            DLL.main.FormStart("DupliFilesComparer", DLL.main.myappname());

            string json = nao0x0.JSON.Load();
            if (json != string.Empty)
            {
                config = nao0x0.JSON.ToHashtable(json);

                if (config["folder_path"] != null)
                    textBox3_folderpath.Text = config["folder_path"].ToString();

            }


            string[] cmds = System.Environment.GetCommandLineArgs();
            try
            {
                if (cmds[1] == "/autoplay")
                {
                    //autoplay = true;
                }

            }
            catch (Exception e) { }

        }

        

        private void Button1_run_Click(object sender, EventArgs e)
        {

            if(radioButton1.Checked != true && radioButton2.Checked != true)
            {
                MessageBox.Show("比較対象のいずれかにチェックを付けてください");
                return;
            }

            button1_run.Enabled = false;
            ArrayList file_paths = new ArrayList();

            for (int i = 0; i < listView1_files.Items.Count; i++)
            {
                file_paths.Add(listView1_files.Items[i].Text);
            }

            string to_directory = textBox3_folderpath.Text;

            int copy_mode = 0;
            if(radioButton1.Checked == true)
            {
                copy_mode = 1;
            }else if(radioButton2.Checked == true)
            {
                copy_mode = 2;
            }

            foreach (var value in file_paths)
            {
                string file_path = value.ToString();
                string dir = Path.GetDirectoryName(file_path);
                string name = Path.GetFileName(file_path);
                string to_path = to_directory + @"\" + name;
                bool flag_move = false;
                nao0x0.PC.DebugLog(to_path);

                if (File.Exists(file_path) == false)
                    continue;

                if (File.Exists(to_path) == true)
                {
                    nao0x0.PC.DebugLog("存在します");
                    FileInfo fi_cur = new FileInfo(to_path);
                    FileInfo fi_to = new FileInfo(file_path);
                    if (copy_mode == 1 && fi_cur.Length <= fi_to.Length)
                    {
                        if(checkBox1.Checked == true)
                            File.Move(to_path, to_path + ".bak");
                        else
                            File.Delete(to_path);

                        flag_move = true;
                    }
                    else if (copy_mode == 2 && fi_cur.Length > fi_to.Length)
                    {
                        if (checkBox1.Checked == true)
                            File.Move(to_path, to_path + ".bak");
                        else
                            File.Delete(to_path);

                        flag_move = true;
                    }
                }
                else
                {
                    flag_move = true;
                    nao0x0.PC.DebugLog("存在しません");
                }

                if (flag_move == true)
                {
                    if (radioButton3.Checked == true)
                        File.Move(file_path, to_path);
                    else if (radioButton4.Checked == true)
                        File.Copy(file_path, to_path);
                }
            }
            button1_run.Enabled = true;
        }

        private void ListView1_files_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileName =
        (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (string name in fileName)
            {
                listView1_files.Items.Add(name);
            }
        }

        private void ListView1_files_DragEnter(object sender, DragEventArgs e)
        {
            //コントロール内にドラッグされたとき実行される
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                //ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
                e.Effect = DragDropEffects.Copy;
            else
                //ファイル以外は受け付けない
                e.Effect = DragDropEffects.None;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hashtable setconfig = new Hashtable();

            string folder_path = textBox3_folderpath.Text;

            setconfig["folder_path"] = folder_path;

            nao0x0.JSON.Save(nao0x0.JSON.ToJSON(setconfig));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            listView1_files.Items.Clear();
        }
    }
}
