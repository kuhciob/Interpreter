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
    class Parser
    {
        private readonly WebBrowser webBrowser=null;
        private readonly RichTextBox HTMLtext=null;
        private List<Statement> StatementsList = new List<Statement>();
        private readonly List<Token> TokenList = new List<Token>();

        private int TokenListItem;       //змінна для пересування по списку ТОКЕНІВ
        private int lookahead;          // ТЕГ токена

        public Hashtable IDs = new Hashtable();          //хештаблиця для зарезервованих змінних(ID)
        public Hashtable functions = new Hashtable();   //хештаблиця для зарезервованих функцій

        public Parser(List<Token> list, WebBrowser web, RichTextBox HTMLtxt)
        {
            this.TokenList = new List<Token>(list);
            this.webBrowser = web;
            this.HTMLtext = HTMLtxt;
            reserveFunctions();
        }
        public List<Statement> Parse()
        {
            //try
           // {
                if (TokenList.Count == 0) 
                    return new List<Statement>();

                this.lookahead = TokenList[0].tag;
                TokenListItem = 0;
                for (; TokenListItem < TokenList.Count();)
                {
                    lookahead = (int)TokenList[TokenListItem].tag;
                    Statement state = statement();
                    StatementsList.Add(state);

                }
                return StatementsList;

           /* }
            catch (Exception ex0)
            {
                MessageBox.Show(ex0.Message);
                return null;
            }*/

        }
        private void reserve(Function t) { functions.Add(t.Name, t); }
        //private void reserve(IDExpr t) { IDs.Add(t.Name, t); }
        private void reserveFunctions()
        {
            reserve(new ClickButton(this.webBrowser));
            reserve(new Click(this.webBrowser));
            reserve(new ClickAllReff(this.webBrowser));
            reserve(new ShowMessage());
            reserve(new Help(functions));
            reserve(new PutTextById(this.webBrowser));
            reserve(new FindHTMLItem(this.HTMLtext));
            
        }
        
        private Statement statement()
        {
            if (TokenListItem < TokenList.Count())
            {
                lookahead = (int)TokenList[TokenListItem].tag;
                switch (lookahead)
                {
                    case (int)TokenTags.OPENBRACKET:
                        match((int)TokenTags.OPENBRACKET);
                        List<Statement> stateArr = new List<Statement>();
                        while (lookahead != (int)TokenTags.CLOSEBRACKET)
                        {
                            if ((TokenListItem) >= TokenList.Count())
                                break;
                            Statement foo = statement();
                            stateArr.Add(foo);
                        }
                        match((int)TokenTags.CLOSEBRACKET);
                        return new CompoundStatement(stateArr.ToArray());

                    case (int)TokenTags.FUNCTION:
                        match((int)TokenTags.FUNCTION);
                        int idindx2 = match((int)TokenTags.ID);
                        match((int)TokenTags.OPENCIRKBRACKET);

                        string funcname = ((Word)TokenList[idindx2]).lexeme;

                        List<Expression> func_parameters = new List<Expression>();

                        while (lookahead != (int)TokenTags.CLOSECIRKBRACKET)
                        {
                            int tokindx = match((int)TokenTags.ID);

                            string forfunctionID = ((Word)TokenList[tokindx]).lexeme;
                            if (IDs[forfunctionID] != null)
                                throw new Exception("Existed ID can`t be a parameter for your function");
                            //~~~
                            //~~~  я передаватиму у сворену юзером функію захардкодений параметр, значення якого при виклику заміниться
                            //~~~
                            IDExpr megacrutch = new IDExpr(forfunctionID, IDs);
                            megacrutch.SetValue(new NumbExpr(-666));
                            func_parameters.Add(megacrutch);

                            if (lookahead != (int)TokenTags.COMMA)
                                break;
                            else
                                match((int)TokenTags.COMMA);
                        }
                        match((int)TokenTags.CLOSECIRKBRACKET);
                        Statement body = statement();
                        UserFunctionStatement userFunctionStatement = new UserFunctionStatement(funcname, func_parameters.ToArray(), body, IDs, functions);
                        userFunctionStatement.SetTree(null);
                        return userFunctionStatement;

                    case (int)TokenTags.IF:
                        match((int)TokenTags.IF);
                        match((int)TokenTags.OPENCIRKBRACKET);
                        Expression ifcondition = expression();
                        match((int)TokenTags.CLOSECIRKBRACKET);
                        Statement ifaction = statement();
                        IfStatement ifStatement = new IfStatement(ifcondition, ifaction);
                        ifStatement.SetTree(ifcondition);
                        return ifStatement;

                    case (int)TokenTags.WHILE:
                        match((int)TokenTags.WHILE);
                        match((int)TokenTags.OPENCIRKBRACKET);
                        Expression condition = expression();
                        match((int)TokenTags.CLOSECIRKBRACKET);
                        Statement action = statement();
                        WhileStatement whileStatement = new WhileStatement(condition, action);
                        whileStatement.SetTree(condition);
                        return whileStatement;

                    case (int)TokenTags.RETURN:
                        match((int)TokenTags.RETURN);
                        Expression retexpr = expression();
                        match((int)TokenTags.DOTCOMMA);
                        ReturnStatement RetState = new ReturnStatement(retexpr);
                        RetState.SetTree(retexpr);
                        return RetState;

                    case (int)TokenTags.LET:
                        match((int)TokenTags.LET);
                        int idindx = match((int)TokenTags.ID);
                        match((int)TokenTags.ASSIGN);
                        Expression val = expression();
                        match((int)TokenTags.DOTCOMMA);
                        string id = Convert.ToString(((Word)TokenList[idindx]).lexeme);
                        LetStatement LetState = new LetStatement(id, val, IDs);
                        LetState.SetTree(val);
                        return LetState;

                    case ((int)TokenTags.DOTCOMMA):
                        match((int)TokenTags.DOTCOMMA);
                        EmptyStatement emptyStatement = new EmptyStatement();
                        emptyStatement.SetTree(null);
                        return emptyStatement;

                    case ((int)TokenTags.ID):
                        int idindxx = match((int)TokenTags.ID);
                        string idname = Convert.ToString(((Word)TokenList[idindxx]).lexeme);
                        match((int)TokenTags.ASSIGN);
                        Expression newval = expression();
                        match((int)TokenTags.DOTCOMMA);
                        AssignStatement assignStatement = new AssignStatement(idname, newval, IDs);
                        assignStatement.SetTree(newval);
                        return assignStatement;

                    default:
                        Expression onlyexp = expression();
                        match((int)TokenTags.DOTCOMMA);
                        ExpressionStatement expressionStatement = new ExpressionStatement(onlyexp);
                        expressionStatement.SetTree(onlyexp);
                        return expressionStatement;
                }

            }
            string error = "Fatal error: don`t fitted statement";
            throw new Exception(error);
        }
        private Expression expression()
        {
            Expression expr = null;
            if (TokenListItem < TokenList.Count())
            {
                lookahead = (int)TokenList[TokenListItem].tag;

                switch (lookahead)
                {
                    case (int)TokenTags.BOPERATOR:
                        int operator_indx = match((int)TokenTags.BOPERATOR);

                        Expression eNumb = expression();
                        if (eNumb.Type != ExpressionTypes.Number)
                            throw new Exception("Must be a number");

                        switch (((Word)TokenList[operator_indx]).lexeme)
                        {
                            case "+":
                                return new NumbExpr(Convert.ToDouble(eNumb.Value()));
                            case "-":
                                return new NumbExpr((-1) * Convert.ToDouble(eNumb.Value()));
                            default: throw new Exception("Invalid expression term '"+ ((Word)TokenList[operator_indx]).lexeme+"'");
                        }

                    case (int)TokenTags.NUMBER:
                        match((int)TokenTags.NUMBER);
                        Expression eNumb1 = new NumbExpr(((Numb)TokenList[TokenListItem - 1]).value);

                        if (lookahead == (int)TokenTags.STROPERATOR)
                            throw new Exception("Number operator expected;");

                        if (lookahead == (int)TokenTags.BOPERATOR)
                            return matchBOPERATOR(eNumb1);
                        else
                            return eNumb1;

                    case (int)TokenTags.LITERAL:
                        match((int)TokenTags.LITERAL);
                        Expression litr1 = new StrExpr(((Word)TokenList[TokenListItem - 1]).lexeme);
                        if (lookahead == (int)TokenTags.BOPERATOR)
                            throw new Exception("String operator expected;");

                        if (lookahead == (int)TokenTags.STROPERATOR)
                            return matchSTROPERATOR(litr1);
                        else
                            return litr1;

                    case (int)TokenTags.ID:
                        int IDindx = match((int)TokenTags.ID);

                        string str1 = ((Word)TokenList[IDindx]).lexeme;
                        IDExpr id1 = new IDExpr(str1, IDs);

                        if (lookahead == (int)TokenTags.BOPERATOR)
                            return matchBOPERATOR(id1);

                        if (lookahead == (int)TokenTags.STROPERATOR)
                            return matchSTROPERATOR(id1);

                        return id1;

                    case (int)TokenTags.OPENCIRKBRACKET:
                        match((int)TokenTags.OPENCIRKBRACKET);
                        expr = expression();
                        match((int)TokenTags.CLOSECIRKBRACKET);

                        if (lookahead == (int)TokenTags.BOPERATOR)
                            return matchBOPERATOR(expr);

                        if (lookahead == (int)TokenTags.STROPERATOR)
                            return matchSTROPERATOR(expr);

                        return expr;

                    case (int)TokenTags.CALL:
                        match((int)TokenTags.CALL);
                        int FunkNameTokeIndx = match((int)TokenTags.ID);

                        List<Expression> FunkParams = new List<Expression>();

                        match((int)TokenTags.OPENCIRKBRACKET);
                        while (lookahead != (int)TokenTags.CLOSECIRKBRACKET)
                        {
                            Expression param1 = expression();
                            FunkParams.Add(param1);
                            if (lookahead != (int)TokenTags.CLOSECIRKBRACKET)
                                match((int)TokenTags.COMMA);
                            else
                                break;
                        }
                        match((int)TokenTags.CLOSECIRKBRACKET);
                        string FunkName = ((Word)TokenList[FunkNameTokeIndx]).lexeme + "#" + Convert.ToString(FunkParams.Count());
                        Expression func = (matchFunk(FunkName)).Copy();

                        ((Function)func).SetParameters(FunkParams.ToArray());
                        return func;
                    default:
                        string err = "Syntax Error: not fited expression";
                        //MessageBox.Show(err);
                        throw new Exception(err);

                }
            }
            else
            {
                string err = "Out of range";
                //MessageBox.Show(err);
                throw new Exception(err);
            }

        }
        private int match(int t)
        {
            if (lookahead == t)
            {
                ++TokenListItem;
                if (TokenListItem < TokenList.Count)
                    lookahead = (int)TokenList[TokenListItem].tag;

            }
            else
            {
                string err = "Syntax Error:" + Convert.ToString((TokenTags)t) + " expected";
                //MessageBox.Show(err);
                throw new Exception(err);

            }
            return TokenListItem - 1;
        }

        private Expression matchBOPERATOR(Expression eNumb1)
        {
            int operator_indx = match((int)TokenTags.BOPERATOR);

            Expression eNumb2 = expression();

            if (eNumb2.Type == ExpressionTypes.String)
            {
                string err1 = "Syntax Error : must be Number";
                throw new Exception(err1);
            }
            Expression oprtr;
            switch (((Word)TokenList[operator_indx]).lexeme)
            {
                case "+":
                    oprtr = new Pluss(ExpressionTypes.Number, eNumb1, eNumb2);
                    break;
                case "-":
                    oprtr = new Minus(eNumb1, eNumb2);
                    break;
                case "*":
                    oprtr = new Mult(eNumb1, eNumb2);
                    break;
                case "/":
                    oprtr = new Div(eNumb1, eNumb2);
                    break;
                case ">":
                    oprtr = new Bigger(eNumb1, eNumb2);
                    break;
                case "<":
                    oprtr = new Less(eNumb1, eNumb2);
                    break;
                case ">=":
                    oprtr = new BiggerEqual(eNumb1, eNumb2);
                    break;
                case "<=":
                    oprtr = new LessEqual(eNumb1, eNumb2);
                    break;
                case "==":
                    oprtr = new Equal(ExpressionTypes.Number, eNumb1, eNumb2);
                    break;
                case "!=":
                    oprtr = new NotEqual(ExpressionTypes.Number, eNumb1, eNumb2);
                    break;
                default: throw new Exception("Fatal error: wrong operator");
            }
            return oprtr;
        }
        private Expression matchSTROPERATOR(Expression litr1)
        {
            int operator_indx = match((int)TokenTags.STROPERATOR);

            Expression litr2 = expression();
            if (litr2.Type == ExpressionTypes.Number)
            {
                litr2 = new StrExpr(Convert.ToString(litr2.Value()));
            }
            Expression oprtr;
            switch (((Word)TokenList[operator_indx]).lexeme)
            {
                case "$+":
                    oprtr = new Pluss(ExpressionTypes.String, litr1, litr2);
                    break;
                case "$==":
                    oprtr = new Equal(ExpressionTypes.String, litr1, litr2);
                    break;
                case "$!=":
                    oprtr = new NotEqual(ExpressionTypes.String, litr1, litr2);
                    break;
                default: throw new Exception("Fatal error: wrong operator");
            }
            return oprtr;
        }
        private Function matchFunk(string name)
        {
            Function f = (Function)functions[name];
            if (f == null)
            {
                string err = "The name '" + name + "' does not exist in the current context ";
                //MessageBox.Show(err);
                throw new Exception(err);
            }
            else
            {
                return f;
            }
        }
    }
}
