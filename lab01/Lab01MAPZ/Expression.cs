using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Lab01MAPZ
{

    public enum ExpressionTypes
    {
        Number,
        String,
        Function,
        VoidFunction,
        Array,
        Var,
        //FuncParam
    }
    public abstract class Expression
    {
        public ExpressionTypes Type;
        public Expression(ExpressionTypes type) { Type = type; }
        public abstract object Value();

    }
    public class IDExpr : Expression
    {
        public readonly string Name;
        private Hashtable globalvars;

        public IDExpr(string name, Hashtable vars) : base(ExpressionTypes.Var)
        {
            this.Name = name;
            this.globalvars = vars;
        }
        public override object Value() {
            Expression id = ((Expression)globalvars[Name]);
            if (id == null)
              throw new Exception("The name '" + this.Name + "' does not exist in the current context ");
            return ((Expression)globalvars[Name]).Value();
        }
        public void SetValue(Expression expr)
        {
            globalvars[Name] = expr;
            base.Type = expr.Type;
        }
        public void SetType(ExpressionTypes type)
        {
            base.Type = type;
        }
    }
    class ArrayExpr: Expression
    {
        private Expression[] array;
        public  ArrayExpr(Expression[] arr) : base(ExpressionTypes.Array) { array = arr; }
        public override object Value() { return this.array; }
    }
    public class NumbExpr : Expression
    {
        private double value;

        public NumbExpr( double val) : base(ExpressionTypes.Number) { this.value = val; }
        public override object Value() { return this.value; }
        
    }

    class StrExpr : Expression
    {
        private string value;

        public StrExpr(string str) : base(ExpressionTypes.String) { this.value = str; }
        public override object Value() { return this.value; }
    }


}
