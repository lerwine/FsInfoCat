using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FsInfoCat.Generator
{
    [Obsolete("Use System.CodeDom.Compiler.IndentedTextWriter, instead")]
    public class CodeBuilder
    {
        private readonly LinkedList<ITextToken> _tokens = new();

        public CodeBuilder(int indentLevel = 0) => IndentLevel = (indentLevel < 0) ? 0 : indentLevel;

        public int IndentLevel { get; }

        public CodeBuilder Append(char value)
        {
            if (_tokens.Last?.Value is StringToken token)
                token.Builder.Append(value);
            else
                _tokens.AddLast(new StringToken(new StringBuilder().Append(value)));
            return this;
        }

        public CodeBuilder Append(string value)
        {
            if (string.IsNullOrEmpty(value)) return this;
            if (_tokens.Last?.Value is StringToken token)
                token.Builder.Append(value);
            else
                _tokens.AddLast(new StringToken(new StringBuilder().Append(value)));
            return this;
        }

        public CodeBuilder AppendLine()
        {
            if (_tokens.Last?.Value is StringToken token)
                token.Builder.AppendLine();
            else
                _tokens.AddLast(new StringToken(new StringBuilder().AppendLine()));
            return this;
        }

        public CodeBuilder AppendLine(string value)
        {
            if (string.IsNullOrEmpty(value)) return this;
            if (_tokens.Last?.Value is StringToken token)
                token.Builder.AppendLine(value);
            else
                _tokens.AddLast(new StringToken(new StringBuilder().AppendLine(value)));
            return this;
        }

        public bool Contains(CodeBuilder other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            for (LinkedListNode<ITextToken> node = _tokens.First; node is not null; node = node.Next)
                if (node.Value is NestedToken nestedToken && nestedToken.Contains(other)) return true;
            return false;
        }

        public CodeBuilder Append(CodeBuilder other)
        {
            if (other is null) return this;
            if (Contains(other) || other.Contains(this)) throw new InvalidOperationException();
            _tokens.AddLast(new NestedToken(other));
            return this;
        }

        public CodeBuilder AppendLine(CodeBuilder other)
        {
            if (other is null) return this;
            if (Contains(other) || other.Contains(this)) throw new InvalidOperationException();
            _tokens.AddLast(new NestedToken(other));
            _tokens.AddLast(new StringToken(new StringBuilder().AppendLine()));
            return this;
        }

        public override string ToString()
        {
            LinkedList<(int Indent, string Text)> lineList = new();
            (int Indent, string Text) final = (IndentLevel, "");
            for (LinkedListNode<ITextToken> node = _tokens.First; node is not null; node = node.Next)
                final = node.Value.GetLines(final, lineList, IndentLevel);
            StringBuilder stringBuilder = new();
            string indent = (IndentLevel > 0) ? new string(' ', IndentLevel * 4) : "";
            int currentLevel = IndentLevel;
            for (LinkedListNode<(int Indent, string Text)> currentLine = lineList.First; currentLine is not null; currentLine = currentLine.Next)
            {
                (int level, string text) = currentLine.Value;
                if ((text = text.TrimEnd()).Length == 0)
                    stringBuilder.AppendLine();
                else
                {
                    if (currentLevel != level)
                    {
                        if (level > 0)
                        {
                            int l = level * 4;
                            if (l != indent.Length) indent = new string(' ', l);
                            stringBuilder.Append(indent).AppendLine(text);
                        }
                        else
                            stringBuilder.AppendLine(text);
                        currentLevel = level;
                    }
                    else if (currentLevel > 0)
                        stringBuilder.Append(indent).AppendLine(text);
                    else
                        stringBuilder.AppendLine(text);
                }
            }
            string f = final.Text.TrimEnd();
            if (f.Length > 0)
            {
                if (currentLevel != final.Indent)
                {
                    if (final.Indent > 0)
                    {
                        int l = final.Indent * 4;
                        if (l != indent.Length) indent = new string(' ', l);
                        stringBuilder.Append(indent).Append(f);
                    }
                    else
                        stringBuilder.Append(f);
                }
                else if (currentLevel > 0)
                    stringBuilder.Append(indent).Append(f);
                else
                    stringBuilder.Append(f);
            }
            return stringBuilder.ToString();
        }

        interface ITextToken
        {
            (int Indent, string Text) GetLines((int Indent, string Text) previous, LinkedList<(int Indent, string Text)> lines, int indent);
        }

        class NestedToken : ITextToken
        {
            internal CodeBuilder Builder { get; }
            internal NestedToken(CodeBuilder builder) => Builder = builder;
            public (int Indent, string Text) GetLines((int Indent, string Text) previous, LinkedList<(int Indent, string Text)> lineList, int indent)
            {
                indent += Builder.IndentLevel;
                for (LinkedListNode<ITextToken> node = Builder._tokens.First; node is not null; node = node.Next)
                    previous = node.Value.GetLines(previous, lineList, indent);
                return previous;
            }
            internal bool Contains(CodeBuilder other) => ReferenceEquals(other, Builder) || Builder._tokens.OfType<NestedToken>().Any(t => t.Contains(other));
        }

        class StringToken : ITextToken
        {
            private static readonly Regex NewLineRegex = new(@"\r\n?|\n", RegexOptions.Compiled);

            internal StringBuilder Builder { get; }
            internal StringToken(StringBuilder builder) => Builder = builder;
            public (int Indent, string Text) GetLines((int Indent, string Text) previous, LinkedList<(int Indent, string Text)> lineList, int indent)
            {
                string[] lines = NewLineRegex.Split(Builder.ToString());
                int i = lines.Length - 1;
                if (i == 0)
                    return (previous.Indent, previous.Text + lines[0]);
                if (!string.IsNullOrEmpty(previous.Text))
                    lines[0] = previous.Text + lines[0];
                foreach (string l in lines.Take(i))
                    lineList.AddLast((indent, l));
                return (indent, lines[i]);
            }
        }
    }
}
