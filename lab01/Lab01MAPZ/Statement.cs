using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Lab01MAPZ
{
    enum StatementTypes
    {
        LET,
        FUNCTION,
        IF,
        WHILE,
        ASSIGN,
        EMPTY,
        EXPRESSION,
        COMPOUND,
        RETURN
    }
    abstract class Statement
    {
        public readonly string Name;
        protected Node tree=null;
        public readonly StatementTypes type;

        public Statement(string name,StatementTypes type) { Name = name; this.type = type; }
        abstract public void Action();

        public void SetTree(Expression tree) { this.tree = ExTree.BuildTree(tree); }
        public abstract void PrintTree();
    }
    class EmptyStatement : Statement
    {
        public EmptyStatement() : base("empty", StatementTypes.EMPTY) { }
        public override void Action(){ }
        public override void PrintTree()
        {
            Console.WriteLine("");
            Console.WriteLine(String.Format("Statement ' {0} '", Name));
            Console.WriteLine("[ null ]");
        }
    }
    class ExpressionStatement : Statement
    {
        readonly Expression expression;
        public ExpressionStatement(Expression ex) : base("expression", StatementTypes.EXPRESSION) { 
            this.expression= ex;
        }
        public override void Action() {
            try
            {
                ((Function)expression).Action();
            }
            catch (InvalidCastException )
            {

            }
        }
        public override void PrintTree() {
            Console.WriteLine("");
            Console.WriteLine(String.Format("Statement ' {0} '", Name));
            ExTree.PrintTree(this.tree);
        }
    }

    class CompoundStatement : Statement {

        public readonly Statement[] statements;
        public CompoundStatement(Statement[] sts) : base("compound", StatementTypes.COMPOUND) { statements = sts; }
        public override void Action()
        {
            for(int i = 0; i < statements.Length; ++i)
            {
                statements[i].Action();
            }
        }
        public override void PrintTree()
        {
            //Console.WriteLine("");
            Console.WriteLine(String.Format("Statement ' {0} '", Name));
            Console.WriteLine(" ||");
            for (int i = 0; i < statements.Length; ++i)
            {
                if(i!=0)
                Console.WriteLine(" |");
                Console.WriteLine(String.Format("right son: [ {0} ]", statements[i].Name));
            }
            //ExTree.PrintTree(this.tree);
        }
    }

    
    class LetStatement : Statement {
        Expression value;
        string IdName;
        Hashtable vars;
        public LetStatement(string id,Expression val,Hashtable globalvar) : base("let", StatementTypes.LET) { 
            this.value = val;
            this.IdName = id; 
            this.vars = globalvar;
        }
        public override void Action()
        {
        Expression w = (Expression)vars[IdName];
            if (w != null)
            {
                string err = "Variable ' " + IdName + " 'is allready define in this scope";
                throw new Exception(err);
            }
            else
            {
                Expression retExpr = null;
                if (value.Type == ExpressionTypes.Number)
                {
                    retExpr = new NumbExpr(Convert.ToDouble(value.Value()));
                }
                else
                if (value.Type == ExpressionTypes.String)
                {
                    retExpr = new StrExpr(Convert.ToString(value.Value()));
                }
                else
                if (value.Type == ExpressionTypes.Var)
                {
                    retExpr = (Expression)vars[((IDExpr)value).Name];
                }
                vars.Add(this.IdName, retExpr);
            }
        }
        public override void PrintTree() {
            Console.WriteLine("");
            Console.WriteLine(String.Format("Statement ' {0} '",Name));
            Console.Write("[ = ]");
            Console.WriteLine("--left son: [ " + IdName+" ]");
            Console.WriteLine(" |");
            ExTree.PrintTree(this.tree);
        }
    }

    class AssignStatement : Statement
    {
        string IdName;
        Expression expr;
        Hashtable vars;
        public AssignStatement(string id,Expression expr, Hashtable globalvar) : base("assign", StatementTypes.ASSIGN)
        {
            this.expr = expr;
            this.IdName = id;
            this.vars = globalvar;
        }
        public override void Action()
        {
            
            Expression w = (Expression)vars[IdName];
            if (w == null)
            {
                string err = "The name '" + IdName + "' does not exist in the current context ";
                //MessageBox.Show(err);
                throw new Exception(err);

            }

            Expression retExpr = null;
            if (expr.Type == ExpressionTypes.Number)
            {
                retExpr = new NumbExpr(Convert.ToDouble(expr.Value()));
            }else
            if (expr.Type == ExpressionTypes.String)
            {
                retExpr = new StrExpr(Convert.ToString(expr.Value()));
            }
            else
            if (expr.Type == ExpressionTypes.Var)
            {
                //ExpressionTypes exType=vars[]
                //retExpr = new StrExpr(Convert.ToString(expr.Value()));
                retExpr = (Expression)vars[((IDExpr)expr).Name];
            }

            vars[IdName] = retExpr;
        }
        public override void PrintTree()
        {
            Console.WriteLine("");
            Console.WriteLine(String.Format("Statement ' {0} '", Name));
            Console.Write("[ = ]");
            Console.WriteLine("--left son: [ " + IdName + " ]");
            Console.WriteLine(" |");
            ExTree.PrintTree(this.tree);
        }
    }

    class UserFunctionStatement: Statement
    {
        private Expression[] parameters;
        private Statement actions;
        private Hashtable variables;
        private Hashtable localvariables;
        private Hashtable functions;
        private string userfuncname;
        private string funcname;
        public UserFunctionStatement(string name, Expression[] parms,Statement sts,Hashtable globalvars, Hashtable funcs) :base("define function", StatementTypes.FUNCTION) {
            this.parameters = parms;     
            this.actions = sts;
            this.localvariables = new Hashtable();
            this.variables = globalvars;
            this.functions = funcs;
            this.userfuncname = name;
            this.funcname = name+"#"+this.parameters.Length;
            Function f = (Function)functions[funcname];
            if (f != null)
            {
                string error = "The function is allready deffined";
                throw new Exception(error);
            }
            else
            {
                Function newfunction = new UserFunction(userfuncname, this.parameters, actions);
                functions.Add(funcname, newfunction);
            }

        }
        public override void Action()
        {
            /*Function f = (Function)functions[funcname];
            if (f != null)
            {
                string error = "The function is allready deffined";
                throw new Exception(error);
            }
            else
            {
                Function newfunction = new UserFunction(userfuncname, this.parameters, actions);
                functions.Add(funcname, newfunction);
            }*/
        }
        public override void PrintTree()
        {
            Console.WriteLine("");
            Console.WriteLine(String.Format("Statement ' {0} '", Name));
            Console.Write("{ function }");
            string strparams = "";
            for(int i = 0; i < parameters.Length; ++i)
            {           
                if(i+1== parameters.Length)
                    strparams += "[ variable ]";
                else
                    strparams += "[ variable ], ";
            }
            Console.WriteLine("--left son: Parameters( " + strparams + " )");
            Console.WriteLine(" |");
            Console.WriteLine("right son: { body }");
            Console.WriteLine(" \\||/");
            actions.PrintTree();
        }
    }
    class IfStatement : Statement
    {
        private Expression condition;
        private Statement body;
        public IfStatement(Expression cond, Statement stt) : base("if", StatementTypes.IF) { this.condition = cond; this.body = stt; }
        public override void Action()
        {
            if (Convert.ToBoolean(condition.Value()))
            {
                body.Action();
            }
            else
            {
                return;
            }
        }
        public override void PrintTree()
        {
            Console.WriteLine("");
            Console.WriteLine(String.Format("Statement ' {0} '", Name));
            Console.WriteLine("left son:{ condition }");
            Console.WriteLine(" |");
            ExTree.PrintTree(this.tree);
            Console.WriteLine(" . . . ");
            Console.WriteLine("right son: { body }");
            Console.WriteLine(" \\||/");
            body.PrintTree();
        }
    }
    class WhileStatement : Statement {
        Expression condition;
        Statement action;
        public WhileStatement(Expression condition,Statement action) : base("while",StatementTypes.WHILE)
        {
            this.condition = condition;
            this.action = action;

        }
        public override void Action()
        {
            while (Convert.ToBoolean(condition.Value()))
            {
                action.Action();
            }
        }
        public override void PrintTree()
        {
            Console.WriteLine("");
            Console.WriteLine(String.Format("Statement [ {0} ]", Name));
            Console.WriteLine("left son: { condition }");
            Console.WriteLine(" |");
            ExTree.PrintTree(this.tree);
            Console.WriteLine(" . . . ");
            Console.WriteLine("right son: { body }");
            Console.WriteLine(" \\||/");
            action.PrintTree();
        }
    }
    /// <summary>
    /// UNREALISED
    /// </summary>
    class ReturnStatement : Statement
    {
        Expression returnedexpression;
        public ReturnStatement(Expression expression) : base("return", StatementTypes.RETURN) { this.returnedexpression = expression; }
        public override void Action()
        {
            try
            {
                returnedexpression = ((Function)returnedexpression);
                ((Function)returnedexpression).Action();
               
            }
            catch(InvalidCastException)
            {

            }
        }
        public override void PrintTree()
        {
            Console.WriteLine("");
            Console.WriteLine(String.Format("Statement [ {0} ]", Name));
            Console.Write("[ return ]");
            Console.WriteLine(" \\||/");
            ExTree.PrintTree(this.tree);

        }
    }

}
