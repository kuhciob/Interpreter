using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01MAPZ
{
    public enum TokenTags
    {
        ID,
        WORD,
        NUMBER,
        TRUE,
        FALSE,
        IF,
        ELSE,
        WHILE,
        RETURN,
        FUNCTION,
        CALL,
        LET,

        ASSIGN,
        BOPERATOR,
        STROPERATOR,
        OPENBRACKET,
        CLOSEBRACKET,
        OPENSQRBRACKET,
        CLOSESQRBRACKET,
        OPENCIRKBRACKET,
        CLOSECIRKBRACKET,

        COMMA,
        DOTCOMMA,
        DOUBLEDOT,

        LITERAL,
        COMENT,
        MULTYLINECOMENT,
        
        NEWLINE
    }
    class Token
    {
        public readonly int tag;
        public Token(int t) { tag = t; }

    }

    class Numb : Token
    {
        public readonly double value;
        public Numb(double v) : base((int)TokenTags.NUMBER) { value = v; }
    }

    class Word : Token
    {
        public readonly string lexeme;
        public Word(int t, string str) : base(t) {lexeme = str;}
    }
}
