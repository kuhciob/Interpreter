using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01MAPZ
{
    class Node
    {
        public readonly Expression Father;
        public Node LeftSon;
        public Node RightSon;
        public Node(Expression f, Node ls, Node rs)
        {
            Father = f;
            LeftSon = ls;
            RightSon = rs;
        }
        public void SetRightSon(Node rs)
        {
            RightSon = rs;
        }
        public void SetLeftSon(Node ls)
        {
            LeftSon= ls;
        }
    }

    static class ExTree
    {
        static public Node BuildTree(Expression ex)
        {
            if (ex == null)
                return new Node(null, null, null); ;
            try
            {
                ex = (Operator)ex;
                return new Node(ex, BuildTree(((Operator)ex).Param1), BuildTree(((Operator)ex).Param2));
            }
            catch (InvalidCastException )
            {
                
            }
            return new Node(ex, null, null);
        }

        static public void PrintTree(Node tree)
        {
            Node Head = tree;
            Node foo = Head;
            if (Head.Father == null || Head==null)
            {
                Console.WriteLine("[ null ]");
            }
            while (foo != null)
            {
                
                try
                {
                    Operator tmp=(Operator)foo.Father;
                    System.Type type = (foo.Father.GetType());
                    if(type == typeof(Pluss))
                    {
                        Console.Write("[ + ]");
                    }
                    if (type == typeof(Minus))
                    {
                        Console.Write("[ - ]");
                    }
                    if (type == typeof(Mult))
                    {
                        Console.Write("[ * ]");
                    }
                    if (type == typeof(Div))
                    {
                        Console.Write("[ / ]");
                    } 
                    if (type == typeof(Less))
                    {
                        Console.Write("[ < ]");
                    }
                    if (type == typeof(Bigger))
                    {
                        Console.Write("[ > ]");
                    }
                    if (type == typeof(Equal))
                    {
                        Console.Write("[ == ]");
                    }
                    if (type == typeof(NotEqual))
                    {
                        Console.Write("[ != ]");
                    }

                    if (((Operator)foo.Father).Param1.Type == ExpressionTypes.Var)
                        Console.WriteLine("--left son: [ " + Convert.ToString(((IDExpr)((Operator)foo.Father).Param1).Name) + " ]");
                    else
                        Console.WriteLine("--left son: [ " + Convert.ToString(((Operator)foo.Father).Param1.Value()) + " ]");

                    Console.WriteLine(" |");
                    foo = foo.RightSon;
                    continue;
                }
                catch (InvalidCastException )
                {

                }
                if (foo.Father.Type == ExpressionTypes.Function || foo.Father.Type == ExpressionTypes.VoidFunction)
                {
                    Console.Write("[ call " + Convert.ToString(((Function)foo.Father).Name) + " ]");
                    string strparams = "";
                    for (int i = 0; i < ((Function)foo.Father).parameters.Length; ++i)
                    {
                        if (i + 1 == ((Function)foo.Father).parameters.Length)
                            strparams += "[ " + Convert.ToString(((Function)foo.Father).parameters[i].Value()) + " ] ";
                        else
                            strparams += "[ " + Convert.ToString(((Function)foo.Father).parameters[i].Value()) + " ], ";

                    }
                    Console.WriteLine("--left son: Parameters{ " + strparams + " }");
                }
                else
                    if (foo.Father.Type == ExpressionTypes.Var)
                    Console.WriteLine("right son: [ " + Convert.ToString(((IDExpr)foo.Father).Name + " ]"));
                else
                    Console.WriteLine("right son: [ " + Convert.ToString(foo.Father.Value()) + " ]");

                break;
            }
        }
    }
}
