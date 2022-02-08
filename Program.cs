using System.Text.RegularExpressions;

var answersDict = new Dictionary<string, string>()
{
    { "Token_2", "99" }
};

string testinput = "(Token_2 == \"2\") ? OK : (Token_2 == \"99\") ? ERROR : (Token_2 == \"100\") ? WRONG : WARNING";
Console.WriteLine(Parse(testinput, answersDict));

static string Parse(string input, Dictionary<string, string> answers)
{
    string[] tokenExpressions = TrimCollection(input.Split(new char[] { '(', ')', '?' }));

    for (int i = 0; i < tokenExpressions.Length - 1; i += 2)
    {
        string mainBooleanExpression = tokenExpressions[i];
        string messageExpression = tokenExpressions[i + 1];

        string[] expressions = TrimCollection(Regex.Split(mainBooleanExpression, @"\s+"));
        string[] messages = TrimCollection(messageExpression.Split(':'));

        string left = expressions[0];
        string csharpOperator = expressions[1];
        string right = expressions[2];

        // removing double quotes from answer in input expression
        string answer = string.Concat(TrimCollection(right.Split('"')));
        
        if (csharpOperator == "==" && answers[left] == answer)
        {
            return messages[0];
        }
        // TODO: add other operators

        // usually the last else
        if (messages.Length == 2)
        {
            return messages[1];
        }
    }

    throw new Exception("Couldn't parse the ternary expression");
}

static string[] TrimCollection(IEnumerable<string> collection)
    => collection.Where(x => !string.IsNullOrWhiteSpace(x))
                 .Select(x => x.Trim())
                 .ToArray();
