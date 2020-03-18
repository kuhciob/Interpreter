using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;

namespace Lab01MAPZ
{
    abstract class Function : Expression
    {
        public readonly string Name;
        public readonly string ForUserName;
        public readonly int ParametersCount;
        public Expression[] parameters;
        public Function(ExpressionTypes type, string name, int n) : base(type)
        {
            this.Name = name + "#" + n;
            this.ForUserName = name;
            this.ParametersCount = n;
            parameters = new Expression[n];
        }
        public abstract override object Value();
        public abstract void Action();

        public void SetParameters(Expression[] arr)
        {
            for (int i = 0; i < ParametersCount && i < arr.Length; ++i)
            {
                try
                {
                    if (parameters[i] != null)
                    {
                        ((IDExpr)parameters[i]).SetValue(arr[i]);
                    }
                    else
                        parameters[i] = arr[i];
                }
                catch
                {

                }
            }
        }

        public Function Copy()
        {
            Function f = (Function)this.MemberwiseClone();
            f.parameters = (Expression[])this.parameters.Clone();

            return f;
        }
    }

    class UserFunction : Function
    {
        private Statement action;
        public UserFunction(string name, Expression[] paramss, Statement act) : base(ExpressionTypes.Function, name, paramss.Length)
        {
            this.action = act;
            parameters = paramss;
        }
        public override object Value()
        {
            return null;
        }
        public override void Action()
        {
            this.action.Action();


            //~~~
            //~~~   це МЕГАХАРДКОД, МЕГАКОСТИЛЬ , але воно робе
            //~~~   воно лізе дуже глибоко у тіло функції і заміняє параметри викликів функції на потрібні
            //~~~   
            //~~~   цей МЕГАХАРДКОД виявився повним неробочим лайном, і дивно що я взагалі таке написав, залишу його тут на згадку
            /*
            if (action.type == StatementTypes.COMPOUND)
            {
                foreach(Statement sts in ((CompoundStatement)action).statements)
                {
                    if (sts.type == StatementTypes.EXPRESSION)
                    {
                        if(((ExpressionStatement)sts).expression.Type==ExpressionTypes.Function|| ((ExpressionStatement)sts).expression.Type == ExpressionTypes.VoidFunction)
                        {
                            /*
                            if (((Function)((ExpressionStatement)sts).expression).parameters.Length > parameters.Length)
                            {
                                for (int i = 0; i < ((Function)((ExpressionStatement)sts).expression).parameters.Length; ++i)
                                {
                                    for(int j = 0; j < parameters.Length; ++i)
                                    {

                                    }
                                       
                                }
                            }

                            for(int i=0;i< ((Function)((ExpressionStatement)sts).expression).parameters.Length; ++i)
                            {
                                if (((Function)((ExpressionStatement)sts).expression).parameters[i].Type == ExpressionTypes.FuncParam)
                                    ((Function)((ExpressionStatement)sts).expression).parameters[i]=parameters[i];
                            }
                            //int i=0;
                           
                            



                        }
                    }
                }
            }

            if (action.type == StatementTypes.EXPRESSION)
            {
                if (((ExpressionStatement)action).expression.Type == ExpressionTypes.Function || ((ExpressionStatement)action).expression.Type == ExpressionTypes.VoidFunction)
                {
                    ((Function)((ExpressionStatement)action).expression).SetParameters(parameters);
                }
            }


            if(action.type == StatementTypes.FUNCTION)
            {
                ((Function)((ExpressionStatement)action).expression).SetParameters(parameters);
            }
            */

        }
    }


    class ClickButton : Function
    {
        private WebBrowser WB;
        public ClickButton(WebBrowser browser) : base(ExpressionTypes.VoidFunction, Convert.ToString(nameof(ClickButton)), 1) { WB = browser; }
        public override object Value() { return null; }
        public override void Action()
        {
            if (parameters[0] != null)
            {
                string bttnID = Convert.ToString(parameters[0].Value());
                char[] trimSyms = { '\"' };
                bttnID = bttnID.Trim(trimSyms);
                HtmlElement button = WB.Document.GetElementById(bttnID);
                if (button == null)
                {
                    string err1 = "Error: element doesn`t exist";
                    MessageBox.Show(err1);
                }
                else
                {
                    button.InvokeMember("Click");
                }
            }


        }

    }

    class Click : Function
    {
        private WebBrowser WB;
        public Click(WebBrowser browser) : base(ExpressionTypes.VoidFunction, Convert.ToString(nameof(Click)), 1)
        {
            WB = browser;
        }
        public override object Value() { return null; }
        public override void Action()
        {
            if (parameters[0] != null)
            {
                string ID = Convert.ToString(parameters[0].Value());
                HtmlElement element = WB.Document.GetElementById(ID);
                if (element == null)
                {
                    string err1 = "Error: element doesn`t exist";
                    MessageBox.Show(err1);
                }
                else
                {
                    element.InvokeMember("Click");
                }
            }


        }

    }
    class ShowMessage : Function
    {
        public ShowMessage() : base(ExpressionTypes.VoidFunction, Convert.ToString(nameof(ShowMessage)), 1) { }
        public override object Value() { return null; }
        public override void Action()
        {
            if (parameters[0] != null)
            {
                MessageBox.Show(Convert.ToString(parameters[0].Value()));
            }


        }
    }

    class PutTextById : Function
    {
        private WebBrowser WB;
        public PutTextById(WebBrowser browser) : base(ExpressionTypes.VoidFunction, Convert.ToString(nameof(PutTextById)), 2)
        {
            WB = browser;
        }
        public override object Value() { return null; }
        public override void Action()
        {
            if (parameters[0] != null)
            {
                string ID = Convert.ToString(parameters[0].Value());
                HtmlElement element = WB.Document.GetElementById(ID);
                if (element == null)
                {
                    string err1 = "Error: element doesn`t exist";
                    MessageBox.Show(err1);
                }
                else
                {
                    element.SetAttribute("value",Convert.ToString(parameters[1].Value())); 
                }
            }


        }
    }

    class FindHTMLItem : Function
    {
        private RichTextBox textBox;
        public FindHTMLItem(RichTextBox sender) : base(ExpressionTypes.VoidFunction, Convert.ToString(nameof(FindHTMLItem)), 1)
        {
            textBox = sender;      
        }
        public override object Value() { return null; }
        public override void Action()
        {
            if (parameters[0] != null)
            {
                string word = Convert.ToString(parameters[0].Value());
                Color color = Color.White;

                if (word == string.Empty)
                    return;

                int s_start = textBox.SelectionStart, startIndex = 0, index;

                while ((index = textBox.Text.IndexOf(word, startIndex)) != -1)
                {
                    textBox.Select(index, word.Length);
                    textBox.SelectionColor = color;
                    textBox.SelectionBackColor = Color.DarkBlue;

                    startIndex = index + word.Length;
                }

                textBox.SelectionStart = s_start;
                textBox.SelectionLength = 0;
                textBox.SelectionColor = Color.Black;
                textBox.SelectionBackColor = Color.White;
            }


        }

    }
    class Help : Function
    {
        Hashtable functions;
        public Help(Hashtable funcs) : base(ExpressionTypes.VoidFunction, Convert.ToString(nameof(Help)), 0) { this.functions = funcs; }
        public override object Value() { return null; }
        public override void Action()
        {
            Console.WriteLine("Functions:");
            foreach (string key in functions.Keys)
            {
                string str = String.Format("{0} (", ((Function)functions[key]).ForUserName);
                //foreach(Expression param in ((Function)functions[key]).parameters)
                for (int i = 0; i < ((Function)functions[key]).parameters.Length; ++i)
                {

                    str += Convert.ToString(ExpressionTypes.Var) + ", ";
                }
                str += ")";
                Console.WriteLine(str);
            }
        }
    }
    class ClickAllReff : Function
    {
        private WebBrowser WB;
        public ClickAllReff(WebBrowser browser) : base(ExpressionTypes.VoidFunction, Convert.ToString(nameof(ClickAllReff)), 0) { WB = browser; }
        public override object Value() { return null; }
        public override void Action()
        {
            string tagname = "a";
            HtmlElementCollection elements = WB.Document.GetElementsByTagName(tagname);
            foreach (HtmlElement elem in elements)
            {
                elem.InvokeMember("Click");

            }
        }

    }
}
