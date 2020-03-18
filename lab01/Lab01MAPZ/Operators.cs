using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01MAPZ
{
    abstract class Operator : Expression
    {
        protected Expression param1;
        protected Expression param2;

        public Expression Param1 { get { return param1; }  }
        public Expression Param2 { get { return param2; } }
        public Operator(ExpressionTypes type, Expression p1, Expression p2) : base(type)
        {
            this.param1 = p1;
            this.param2 = p2;
        }
        public abstract override object Value();
    }

    class Pluss : Operator
    {
        public Pluss(ExpressionTypes type, Expression p1, Expression p2) : base(type, p1, p2) { }
        public override object Value()
        {
            if (Type == ExpressionTypes.Number)
                return Convert.ToDouble(param1.Value()) + Convert.ToDouble(param2.Value());
            else
            if (Type == ExpressionTypes.String)
                return Convert.ToString(param1.Value()) + Convert.ToString(param2.Value());
            else
                return null;
        }
    }

    class Minus : Operator
    {
        public Minus(Expression p1, Expression p2) : base(ExpressionTypes.Number, p1, p2) { }
        public override object Value()
        {
            //if (param1.Type == ExpressionTypes.Number && param2.Type == ExpressionTypes.Number )
                return Convert.ToDouble(param1.Value()) - Convert.ToDouble(param2.Value());

           // else
                //return null;
        }
    }

    class Mult : Operator
    {
        public Mult(Expression p1, Expression p2) : base(ExpressionTypes.Number, p1, p2) { }
        public override object Value()
        {
           // if (param1.Type == ExpressionTypes.Number && param2.Type == ExpressionTypes.Number)
                return Convert.ToDouble(param1.Value()) * Convert.ToDouble(param2.Value());
            //else
               // return null;
        }
    }

    class Div : Operator
    {
        public Div(Expression p1, Expression p2) : base(ExpressionTypes.Number, p1, p2) { }
        public override object Value()
        {
            //if (param1.Type == ExpressionTypes.Number && param2.Type == ExpressionTypes.Number)
                return Convert.ToDouble(param1.Value()) / Convert.ToDouble(param2.Value());
            //else
                //return null;
        }
    }

    class Bigger : Operator
    {
        public Bigger(Expression p1, Expression p2) : base(ExpressionTypes.Number, p1, p2) { }
        public override object Value()
        {
            //if (param1.Type == ExpressionTypes.Number && param2.Type == ExpressionTypes.Number)
                return (Convert.ToDouble(param1.Value()) > Convert.ToDouble(param2.Value())) ? 1 : 0;
           // else
               // return null;
        }
    }

    class Less : Operator
    {
        public Less(Expression p1, Expression p2) : base(ExpressionTypes.Number, p1, p2) { }
        public override object Value()
        {
            //if (param1.Type == ExpressionTypes.Number && param2.Type == ExpressionTypes.Number)
                return (Convert.ToDouble(param1.Value()) < Convert.ToDouble(param2.Value())) ? 1 : 0;
            //else
               // return null;
        }
    }

    class Equal : Operator
    {
        public Equal(ExpressionTypes type, Expression p1, Expression p2) : base(type, p1, p2) { }
        public override object Value()
        {
            if (Type == ExpressionTypes.Number)
                return (Convert.ToDouble(param1.Value()) == Convert.ToDouble(param2.Value())) ? 1 : 0;
            else
            if (Type == ExpressionTypes.String)
                return (Convert.ToString(param1.Value()) == Convert.ToString(param2.Value())) ? 1 : 0;
            else
                return null;
        }
    }

    class NotEqual : Operator
    {
        public NotEqual(ExpressionTypes type, Expression p1, Expression p2) : base(type, p1, p2) { }
        public override object Value()
        {
            if (Type == ExpressionTypes.Number)
                return (Convert.ToDouble(param1.Value()) != Convert.ToDouble(param2.Value())) ? 1 : 0;
            else
            if (Type == ExpressionTypes.String)
                return (Convert.ToString(param1.Value()) != Convert.ToString(param2.Value())) ? 1 : 0;
            else
                return null;
        }
    }
    class BiggerEqual : Operator
    {
        public BiggerEqual(Expression p1, Expression p2) : base(ExpressionTypes.Number, p1, p2) { }
        public override object Value()
        {
            //if (param1.Type == ExpressionTypes.Number && param2.Type == ExpressionTypes.Number)
                return (Convert.ToDouble(param1.Value()) <= Convert.ToDouble(param2.Value())) ? 1 : 0;
            //else
                //return null;
        }
    }

    class LessEqual : Operator
    {
        public LessEqual(Expression p1, Expression p2) : base(ExpressionTypes.Number, p1, p2) { }
        public override object Value()
        {
            //if (param1.Type == ExpressionTypes.Number && param2.Type == ExpressionTypes.Number)
                return (Convert.ToDouble(param1.Value()) >= Convert.ToDouble(param2.Value())) ? 1 : 0;
           // else
                //return null;
        }
    }
}
