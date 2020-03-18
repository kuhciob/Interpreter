using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows.Forms;

namespace Lab01MAPZ
{
    public class Interpreter
    {
        private readonly WebBrowser webBrowser=null;
        private readonly RichTextBox HTMLtext=null;

        Lexer lexer;
        Parser parser;
        private List<Token> TokenList = new List<Token>();
        private List<Statement> StatementsList = new List<Statement>();

        public Interpreter(string str, WebBrowser wb, RichTextBox HTMLtxt)
        {
            this.lexer = new Lexer(str);
            this.webBrowser = wb;
            this.HTMLtext = HTMLtxt;
        }
        public Interpreter(string str)
        {
            this.lexer = new Lexer(str);
            webBrowser = null;         

        }

        public void Run()
        {
            try
            {
                this.TokenList=lexer.Lex();
                Prepretator();
                lexer.PrintLexems();
                parser = new Parser(this.TokenList, this.webBrowser,this.HTMLtext);
                this.StatementsList = parser.Parse();
                Optimize();

                foreach (Statement state in StatementsList)
                {
                    state.PrintTree();
                    state.Action();
                }

            }
            catch (Exception ex0)
            {
                MessageBox.Show(ex0.Message);
                return;
            }
        }
        private void Prepretator()
        {
            for ( int i=0;i< TokenList.Count;++i)
            {
                if (TokenList[i].tag == (int)TokenTags.MULTYLINECOMENT)
                {
                    TokenList.RemoveAt(i);
                    for (; i < TokenList.Count; ++i)
                    {   
                        if (TokenList[i].tag == (int)TokenTags.MULTYLINECOMENT)
                        {
                            TokenList.RemoveAt(i);
                            break;
                        }                           
                        TokenList.RemoveAt(i);
                        --i;
                    }
                }

                if (i >= TokenList.Count || i < 0)
                    return;
                    

                if (TokenList[i].tag == (int)TokenTags.COMENT)
                {
                    for (; i < TokenList.Count; ++i)
                    {
                        TokenList[i] = TokenList[i];                       
                        if (TokenList[i].tag == (int)TokenTags.NEWLINE)
                            break;
                        TokenList.RemoveAt(i);
                        --i;
                    }
                }
                if (TokenList[i].tag==(int)TokenTags.NEWLINE)
                {
                    TokenList.RemoveAt(i);
                    --i;
                }
            }
        }

        private void Optimize()
        {
            try
            {
                for (int i = 0; i < this.StatementsList.Count(); ++i) 
                {
                    if (StatementsList[i].type == StatementTypes.EMPTY)
                        StatementsList.RemoveAt(i);
                }

            }
            catch (Exception ex0)
            {
                MessageBox.Show(ex0.Message);
                return;
            }
        }
    }
 
}
