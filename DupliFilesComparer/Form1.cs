using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DupliFilesComparer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            InitializeComponent();
            Form1.CheckForIllegalCrossThreadCalls = false;

            if (DLL.main.FileSearch("./database") == false)
            {
                DLL.main.ForderCreate("./database");
            }

            nao0x0.flag.debug = true;

            def["replace_after_filename"] = textBox4_replace_after_filename.Text;
            def["date_format"] = textBox1_date_format.Text;

            DLL.main.appcheck(DLL.main.myappname());
            DLL.main.FormStart("DupliFilesRenamer", DLL.main.myappname());
            //DLL.form.Icon("Fuji", Properties.Resources.Icon, this);


            string json = nao0x0.JSON.Load();
            if (json != string.Empty)
            {
                config = nao0x0.JSON.ToHashtable(json);

                if (config["folder_path"] != null)
                    textBox3_folderpath.Text = config["folder_path"].ToString();

                if (config["replace_files"] != null)
                    textBox2_replacefiles.Text = config["replace_files"].ToString();

                if (config["replace_after_filename"] != null)
                    textBox4_replace_after_filename.Text = config["replace_after_filename"].ToString();

                if (config["ext"] != null)
                    textBox1_ext.Text = config["ext"].ToString();

                if (config["date_format"] != null)
                    textBox1_date_format.Text = config["date_format"].ToString();

            }


            string[] cmds = System.Environment.GetCommandLineArgs();
            try
            {
                if (cmds[1] == "/autoplay")
                {
                    autoplay = true;
                }

            }
            catch (Exception e) { }

        }
    }
}
