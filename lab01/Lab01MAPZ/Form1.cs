using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Lab01MAPZ
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        public Form1()
        {
            InitializeComponent();
            AllocConsole();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void Runbutton_Click(object sender, EventArgs e)
        {
            
            string strProgram = this.ProgramtextBox.Text;
            Interpreter interpreter = new Interpreter(strProgram,webBrowser1, HTMLtextBox);
            interpreter.Run();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string Program = "";
            Program = "function A(param){call ShowMessage(param);}\r\n call A(\"HELLO\");\r\n";
            Program += "function UserClick(id){ call Click(id);}\r\n";
            Program += "let myid=\"click\"$+\"it\";\r\n";
            Program += "call UserClick(myid);\r\n";
            Program += "let b=3;\r\n";
            Program += "while(b>0){\r\ncall ShowMessage(b);\r\nb=b-1;\r\n}\r\n";
            Program += "if(b==0){call ShowMessage(\"It work\");}\r\n";
            Program += "call PutTextById(\"username\",\"Ivan\");\r\n";
            Program += "call PutTextById(\"password\",\"1234\");\r\n";
            this.ProgramtextBox.Text = Program;
            webBrowser1.Navigate(new System.Uri(@"file://Z:\!Ivasuk\!SE\PZ24\MAPZ\Lab01MAPZ\HTMLsite\lab01.html"));
        }
        

        private void Backbutton_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void Homebutton_Click(object sender, EventArgs e)
        {
            webBrowser1.GoHome();
        }

        private void webBrowser1_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HTMLtextBox.Text = webBrowser1.DocumentText;
        }
    }
}
