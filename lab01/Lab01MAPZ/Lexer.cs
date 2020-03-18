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
    class Lexer
    {
        private string InputText = "";
        private char peek = ' ';                              //змінна для пересування по вхідному тексту для лексера
        private List<Token> TokenList = new List<Token>();
        public Hashtable words = new Hashtable();           //хештаблиця для зарезервованих слів(токенів)

        public Lexer(string input)
        {
            this.InputText = input;
        }
        private void reserve(Word t) { words.Add(t.lexeme, t); }

        private void reserveWords()
        {
            reserve(new Word((int)TokenTags.TRUE, "true"));
            reserve(new Word((int)TokenTags.FALSE, "false"));
            reserve(new Word((int)TokenTags.IF, "if"));
            reserve(new Word((int)TokenTags.ELSE, "else"));
            reserve(new Word((int)TokenTags.WHILE, "while"));
            reserve(new Word((int)TokenTags.RETURN, "return"));
            reserve(new Word((int)TokenTags.FUNCTION, "function"));
            reserve(new Word((int)TokenTags.LET, "let"));
            reserve(new Word((int)TokenTags.OPENBRACKET, "{"));
            reserve(new Word((int)TokenTags.CLOSEBRACKET, "}"));
            reserve(new Word((int)TokenTags.OPENSQRBRACKET, "["));
            reserve(new Word((int)TokenTags.CLOSESQRBRACKET, "]"));
            reserve(new Word((int)TokenTags.OPENCIRKBRACKET, "("));
            reserve(new Word((int)TokenTags.CLOSECIRKBRACKET, ")"));
            reserve(new Word((int)TokenTags.COMMA, ","));
            reserve(new Word((int)TokenTags.DOTCOMMA, ";"));
            reserve(new Word((int)TokenTags.DOUBLEDOT, ":"));
            reserve(new Word((int)TokenTags.CALL, "call"));
            reserve(new Word((int)TokenTags.CALL, "~"));

            reserve(new Word((int)TokenTags.BOPERATOR, ">"));
            reserve(new Word((int)TokenTags.BOPERATOR, "<"));
            reserve(new Word((int)TokenTags.BOPERATOR, "=="));
            reserve(new Word((int)TokenTags.BOPERATOR, ">="));
            reserve(new Word((int)TokenTags.BOPERATOR, "<="));
            reserve(new Word((int)TokenTags.BOPERATOR, "!="));
            reserve(new Word((int)TokenTags.ASSIGN, "="));
            reserve(new Word((int)TokenTags.BOPERATOR, "-"));
            reserve(new Word((int)TokenTags.BOPERATOR, "+"));
            reserve(new Word((int)TokenTags.BOPERATOR, "*"));
            reserve(new Word((int)TokenTags.BOPERATOR, "/"));
            reserve(new Word((int)TokenTags.STROPERATOR, "$+"));
            reserve(new Word((int)TokenTags.STROPERATOR, "$=="));
            reserve(new Word((int)TokenTags.STROPERATOR, "$!="));

            reserve(new Word((int)TokenTags.COMENT, "//"));
            reserve(new Word((int)TokenTags.MULTYLINECOMENT, "@"));
            reserve(new Word((int)TokenTags.NEWLINE, "\r\n"));
            //reserve(new Word((int)TokenTags.NEWLINE, "\r"));
            //reserve(new Word((int)TokenTags.NEWLINE, "\n"));
        }

        public List<Token> Lex()
        {
            try
            {
                reserveWords();
                for (int i = 0; i < InputText.Length; ++i)
                {
                    peek = (char)InputText[i];

                    //~~~~~MISSING SPACES and others
                    if (peek == ' ' || peek == '\t' || peek == '\n') continue;

                    //~~~~~WINDOWS NEWLINE
                    if (peek == '\r')
                        if (i + 1 < InputText.Length)
                            if ((char)InputText[i + 1] == '\n')
                            {
                                string str = "\r\n";
                                Word coment = (Word)words[str];
                                if (coment != null)
                                {
                                    TokenList.Add(coment);
                                    continue;
                                }
                                coment = new Word((int)TokenTags.NEWLINE, str);
                                TokenList.Add(coment);
                                ++i;
                            }
                            else
                                continue;



                    //~~~~~LEX LITTERAL
                    if (peek == '"')
                    {
                        string str = "";
                        //str += '"';
                        for (++i; i < InputText.Length; ++i)
                        {
                            peek = InputText[i];
                            if ((char)peek != '"')
                            {
                                str += peek;
                                if (i + 1 == InputText.Length)
                                    throw new Exception("Error: wrong format of litersl; \" expectet");
                            }
                            else
                            {
                                break;
                            }
                        }

                        Word w = new Word((int)TokenTags.LITERAL, str);

                        TokenList.Add(w);
                        continue;
                    }

                    //~~~~~LEX NUMBERS
                    if (Char.IsDigit(peek))
                    {
                        string str = "";
                        int dotcount = 0;
                        while (i < InputText.Length)
                        {
                            peek = InputText[i];
                            if (peek == '.')
                            {
                                ++dotcount;
                                if (dotcount > 1)
                                {
                                    throw new Exception("Error: ; expected\n");
                                }
                                else
                                {
                                    peek = ',';
                                    str += peek;
                                    ++i;
                                    continue;
                                }
                            }

                            if (Char.IsDigit(peek))
                            {
                                str += peek;
                            }
                            else break;
                            ++i;

                        }
                        --i;
                        Token c = new Numb(Convert.ToDouble(str));
                        TokenList.Add(c);
                        continue;
                    }
                    //~~~~~LEX WORDS
                    if (Char.IsLetter(peek))
                    {
                        string str = "";

                        while (i < InputText.Length)
                        {
                            peek = InputText[i];

                            if (Char.IsLetter(peek) || Char.IsDigit(peek) || peek == '_')
                            {
                                str += peek;
                            }
                            else break;
                            ++i;

                        }
                        --i;

                        Word w = (Word)words[str];
                        if (w != null)
                        {
                            TokenList.Add(w);
                            //DO ERROR
                            continue;
                        }

                        w = new Word((int)TokenTags.ID, str);
                        TokenList.Add(w);
                        continue;
                    }

                    if (peek == '.')
                        throw new Exception("Error :  Only assignment, call, increment, decrement, await, and new object expressions can be used as a statement \n");

                    //~~~~~LEX OTHERS
                    if (!Char.IsLetter(peek) && !Char.IsDigit(peek))
                    {
                        string str = "";

                        peek = InputText[i];
                        //~~
                        //~~   HARDCODE for  coment
                        //~~
                        if (peek == '@')
                        {
                            str = "@";
                            Word div = (Word)words[str];
                            if (div != null)
                            {
                                TokenList.Add(div);
                                continue;
                            }
                            div = new Word((int)TokenTags.MULTYLINECOMENT, str);
                            TokenList.Add(div);
                            continue;
                        }

                        if (peek == '/')
                            if (i + 1 < InputText.Length)
                                if (InputText[i + 1] == '/')
                                {
                                    str = "//";
                                    Word div = (Word)words[str];
                                    if (div != null)
                                    {
                                        TokenList.Add(div);
                                        continue;
                                    }
                                    div = new Word((int)TokenTags.COMENT, str);
                                    TokenList.Add(div);
                                    continue;
                                }
                                else
                                {
                                    str = "" + peek;
                                    Word div = (Word)words[str];
                                    if (div != null)
                                    {
                                        TokenList.Add(div);
                                        continue;
                                    }
                                    div = new Word((int)TokenTags.ID, str);
                                    TokenList.Add(div);
                                    continue;
                                }


                        //~~
                        //~~   HARDCODE for string operators
                        //~~
                        if (peek == '$')
                        {
                            if (i + 1 < InputText.Length)
                            {
                                str += peek;
                                ++i;
                                peek = InputText[i];
                                if (peek == '=')
                                {
                                    if (i + 1 < InputText.Length)
                                    {
                                        str += peek;
                                        ++i;
                                        peek = InputText[i];
                                        if (peek == '=')
                                        {
                                            str += peek;
                                            Word wrd = (Word)words[str];
                                            if (wrd != null)
                                            {
                                                TokenList.Add(wrd);
                                                continue;
                                            }
                                            --i;
                                            wrd = new Word((int)TokenTags.BOPERATOR, str);
                                            words.Add(str, wrd);
                                        }
                                    }
                                }
                                else
                                if (peek == '!')
                                {
                                    if (i + 1 < InputText.Length)
                                    {
                                        str += peek;
                                        ++i;
                                        peek = InputText[i];
                                        if (peek == '=')
                                        {
                                            str += peek;
                                            Word wrd = (Word)words[str];
                                            if (wrd != null)
                                            {
                                                TokenList.Add(wrd);
                                                continue;
                                            }
                                            --i;
                                            wrd = new Word((int)TokenTags.BOPERATOR, str);
                                            words.Add(str, wrd);
                                        }
                                    }
                                }
                                else
                                if (peek == '+')
                                {
                                    str += peek;
                                    Word wrd = (Word)words[str];
                                    if (wrd != null)
                                    {
                                        TokenList.Add(wrd);
                                        continue;
                                    }
                                    --i;
                                    wrd = new Word((int)TokenTags.BOPERATOR, str);
                                    words.Add(str, wrd);
                                }
                            }
                        }

                        //~~
                        //~~   HARDCODE for binar operaors whith 2 syms
                        //~~
                        if (peek == '<' || peek == '>' || peek == '=' || peek == '!')
                        {
                            if (i + 1 < InputText.Length)
                            {
                                str += peek;
                                ++i;
                                peek = InputText[i];
                                if (peek == '=')
                                {
                                    str += peek;
                                    Word wrd = (Word)words[str];
                                    if (wrd != null)
                                    {
                                        TokenList.Add(wrd);
                                        continue;
                                    }
                                    wrd = new Word((int)TokenTags.BOPERATOR, str);
                                    words.Add(str, wrd);
                                }
                                else
                                {
                                    --i;
                                    Word wrd = (Word)words[str];
                                    if (wrd != null)
                                    {
                                        TokenList.Add(wrd);
                                        continue;
                                    }
                                    wrd = new Word((int)TokenTags.BOPERATOR, str);
                                    words.Add(str, wrd);
                                    continue;
                                }
                            }
                        }

                        //~~
                        //~~   HARDCODE for others single symbol tokens
                        //~~   if it is not reserved, it whould be ID
                        //~~
                        if (!Char.IsLetter(peek) && !Char.IsDigit(peek) && peek != ' ' && peek != '\n' && peek != '\t' && peek != '\r')
                        {
                            str += peek;
                        }
                        else
                        {
                            --i;
                            continue;
                        }

                        Word w = (Word)words[str];
                        if (w != null)
                        {
                            TokenList.Add(w);
                            continue;
                        }
                        w = new Word((int)TokenTags.ID, str);
                        TokenList.Add(w);
                        continue;
                    }
                }
                return TokenList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void PrintLexems()
        {
            for (int i = 0; i < TokenList.Count(); ++i)
            {
                string strout = "";
                strout = "<" + Convert.ToString((TokenTags)TokenList[i].tag) + ",";

                if ((TokenTags)TokenList[i].tag == TokenTags.NUMBER)
                {
                    strout += Convert.ToString(((Numb)TokenList[i]).value);
                }
                else
                {
                    if ((TokenTags)TokenList[i].tag == TokenTags.WORD)
                    {
                        strout += Convert.ToString(((Word)TokenList[i]).lexeme);
                    }
                    else
                    {
                        strout += Convert.ToString(((Word)TokenList[i]).lexeme);
                    }
                }
                //else

                //else
                strout += "> ";

                Console.Write(strout);
                if ((TokenTags)TokenList[i].tag == TokenTags.DOTCOMMA
                    || (TokenTags)TokenList[i].tag == TokenTags.CLOSEBRACKET
                    || (TokenTags)TokenList[i].tag == TokenTags.OPENBRACKET)
                    Console.WriteLine("");


            }
            Console.WriteLine("");
        }
    }
}
