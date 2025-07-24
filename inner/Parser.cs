using System;
using System.Collections.Generic;
using System.Text;
using UanMathEvaluator.inner.nodes;

namespace UanMathEvaluator.inner
{
    internal class Parser
    {
        private readonly string _text;
        private int _pos;
        private char Current => _pos < _text.Length ? _text[_pos] : '\0';

        public Parser(string text)
        {
            StringBuilder sb = new StringBuilder(text.Replace("\r", ""));
            for (int i = 0; i < sb.Length; i++)
                if (char.IsWhiteSpace(sb[i]))
                    sb[i] = ' ';
            _text = sb.ToString().Replace(" ", "").Trim();
            _pos = 0;
        }

        public List<ExprNode> ParseStatements()
        {
            var list = new List<ExprNode>();
            while (_pos < _text.Length)
            {
                if (_pos >= _text.Length) break;
                list.Add(ParseStatement());
                if (Current == ';') _pos++;
            }
            return list;
        }

        private ExprNode ParseStatement()
        {
            int backup = _pos;
            if (char.IsLetter(Current))
            {
                var name = ParseIdentifier();
                if (Current == '=')
                {
                    _pos++;
                    var expr = ParseExpression();
                    return new AssignNode(name, expr);
                }
                _pos = backup;
            }
            return ParseExpression();
        }

        public ExprNode ParseExpression() => ParseComparison();

        private ExprNode ParseComparison()
        {
            var node = ParseAddSubtract();
            while (Match("==") || Match("!=") || Match("<=") || Match(">=") || Current == '<' || Current == '>')
            {
                string op = ParseOperator();
                var right = ParseAddSubtract();
                node = new BinaryOpNode(op, node, right);
            }
            return node;
        }

        private ExprNode ParseAddSubtract()
        {
            var node = ParseMultiplyDivide();
            while (Current == '+' || Current == '-')
            {
                char op = Current;
                _pos++;
                var right = ParseMultiplyDivide();
                node = new BinaryOpNode(op.ToString(), node, right);
            }
            return node;
        }

        private ExprNode ParseMultiplyDivide()
        {
            var node = ParsePower();
            while (Current == '*' || Current == '/' || Current == '%')
            {
                char op = Current;
                _pos++;
                var right = ParsePower();
                node = new BinaryOpNode(op.ToString(), node, right);
            }
            return node;
        }

        private ExprNode ParsePower()
        {
            var node = ParsePrimary();
            while (Current == '^')
            {
                _pos++;
                var right = ParsePrimary();
                node = new BinaryOpNode("^", node, right);
            }
            return node;
        }

        private ExprNode ParsePrimary()
        {
            if (char.IsDigit(Current) || Current == '.')
                return ParseNumber();

            if (char.IsLetter(Current))
                return ParseIdentifierOrFunction();

            if (Current == '(')
            {
                _pos++;
                var node = ParseExpression();
                if (Current != ')')
                    throw new Exception("Missing closing parenthesis");
                _pos++;
                return node;
            }

            throw new Exception($"Unexpected character: {Current}");
        }

        private ExprNode ParseNumber()
        {
            int start = _pos;
            while (char.IsDigit(Current) || Current == '.') _pos++;
            var numStr = _text.Substring(start, _pos - start);
            return new NumberNode(double.Parse(numStr));
        }

        private ExprNode ParseIdentifierOrFunction()
        {
            int start = _pos;
            while (char.IsLetterOrDigit(Current)) _pos++;
            string name = _text.Substring(start, _pos - start);

            if (Current == '(')
            {
                _pos++; // skip '('
                var args = new List<ExprNode>();
                if (Current != ')')
                {
                    while (true)
                    {
                        args.Add(ParseExpression());
                        if (Current == ')') break;
                        if (Current != ',') throw new Exception("Expected ',' in function arguments");
                        _pos++;
                    }
                }
                _pos++; // skip ')'
                return new FunctionNode(name, args);
            }

            return new VariableNode(name);
        }

        private string ParseIdentifier()
        {
            int start = _pos;
            while (char.IsLetterOrDigit(Current)) _pos++;
            return _text.Substring(start, _pos - start);
        }

        private string ParseOperator()
        {
            if (Match("==") || Match("!=") || Match("<=") || Match(">="))
            {
                var op = _text.Substring(_pos, 2);
                _pos += 2;
                return op;
            }

            char single = Current;
            _pos++;
            return single.ToString();
        }

        private bool Match(string op)
        {
            return _text.Substring(_pos).StartsWith(op);
        }
    }
}
