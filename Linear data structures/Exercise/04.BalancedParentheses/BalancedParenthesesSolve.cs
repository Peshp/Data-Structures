namespace Problem04.BalancedParentheses
{
    using System;
    using System.Collections.Generic;

    public class BalancedParenthesesSolve : ISolvable
    {
        public bool AreBalanced(string parentheses)
        {
            //First method

            //int count = 0;
            //for (int i = 0; i < parentheses.Length; i++)
            //{
            //    if (parentheses[i] == '{')
            //        count++;
            //    else if (parentheses[i] == '}')
            //        count--;
            //
            //    if (parentheses[i] == '[')
            //        count++;
            //    else if (parentheses[i] == ']')
            //        count--;
            //
            //    if (parentheses[i] == '(')
            //        count++;
            //    else if (parentheses[i] == ')')
            //        count--;
            //}
            //
            //if (count == 0)
            //    return true;
            //return false;

            //Second Method

            var opened = new List<char> {'(', '[', '{'};
            var closed = new List<char> {')', ']', '}'};

            var matching = new Dictionary<char, char>
            {
                ['{'] = '}',
                ['['] = ']',
                ['('] = ')',
            };
            var result = new Stack<char>();

            foreach (char c in parentheses)
            {
                if(opened.Contains(c))
                    result.Push(c);
                else
                {
                    if(result.Count == 0)
                        return false;
                    if(closed.Contains(c))
                    {
                        if (matching[result.Peek()] == c)
                            result.Pop();
                        else
                            return false;
                    }
                }
            }
            return true;
        }
    }
}
