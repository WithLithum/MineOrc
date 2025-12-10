// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text;

namespace MineOrc.Foundation.Runtime.Arguments;

public readonly ref struct ArgumentValueResolver
{
    private readonly IReadOnlyDictionary<string, string> _values;
    private readonly StringBuilder _variableBuilder = new();
    private readonly StringBuilder _parseBuilder = new();

    public ArgumentValueResolver(IReadOnlyDictionary<string, string> values)
    {
        _values = values;
    }

    private static KeyNotFoundException VariableNotFound(string variableName)
    {
        return new KeyNotFoundException($"Argument variable '{variableName}' not found.");
    }
    
    public string Resolve(string value)
    {
        // Check if the value starts with '$' and ends with right bracket - this indicates the
        // entire string is a variable pattern
        //
        // Although there exists possibility that something like '${varA}=${varB}' exists Minecraft
        // didn't have that yet ;)
        if (value.StartsWith('$') && value.EndsWith('}'))
        {
            // Resolves using the fast method which does not involve a full string parse, and
            // only the variable name is allocated.
            return ResolveFast(value);
        }

        // Check if the value contains dollar sign which indicates existence of value template
        return value.Contains('$')
            ? ResolveParse(value)
            : value;
    }

    private string ResolveFast(string value)
    {
        var span = value.AsSpan();

        // Check if the string is at least 4 characters long, and has the value name surrounded in
        // braces.
        if (span.Length < 4
            || span[1] != '{'
            || span[^1] != '}')
        {
            return value;
        }

        var variableName = new string(span[2..^1]);
        return !_values.TryGetValue(variableName, out var result)
            ? throw VariableNotFound(variableName)
            : result;
    }

    private void InsertVariable(string variableName)
    {
        if (!_values.TryGetValue(variableName, out var result))
        {
            throw VariableNotFound(variableName);
        }
        
        _parseBuilder.Append(result);
    }

    private string ResolveParse(string value)
    {
        _variableBuilder.Clear();
        _parseBuilder.Clear();
        _parseBuilder.EnsureCapacity(value.Length);

        var span = value.AsSpan();

        var preVariable = false;
        var inVariable = false;
        
        foreach (var ch in span)
        {
            if (ch == '$')
            {
                preVariable = true;
                continue;
            }

            if (preVariable)
            {
                preVariable = false;
                
                if (ch == '{')
                {
                    inVariable = true;
                    continue;
                }
                
                inVariable = false;
                _parseBuilder.Append('$');
                _variableBuilder.Clear();
            }

            if (inVariable)
            {
                // Right brace, end of variable template
                if (ch == '}')
                {
                    // Insert variable
                    inVariable = false;
                    var variableName = _variableBuilder.ToString();
                    InsertVariable(variableName);
                    _variableBuilder.Clear();
                    continue;
                }
                
                _variableBuilder.Append(ch);
            }
            else
            {
                _parseBuilder.Append(ch);
            }
        }

        return _parseBuilder.ToString();
    }
}