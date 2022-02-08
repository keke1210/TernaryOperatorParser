using System.Text.RegularExpressions;

var answersDict = new Dictionary<string, string>()
{
    { "Token_2", "3" }
};

string testinput = "(Token_2 == \"2\") ? OK : (Token_2 == \"99\") ? ERROR : (Token_2 == \"100\") ? WRONG : WARNING";
Console.WriteLine(Parse(testinput, answersDict));

static string Parse(string input, Dictionary<string, string> answers)
{
    var tokenExpressions = input
                        .Split(new char[] { '(', ')', '?' })
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => x.Trim())
                        .ToArray();

    for (int i = 0; i < tokenExpressions.Length - 1; i += 2)
    {
        var mainBooleanExpression = tokenExpressions[i];
        var messageExpression = tokenExpressions[i + 1];

        string[] expressions = Regex
                        .Split(mainBooleanExpression, @"\s+")
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => x.Trim())
                        .ToArray();

        var messages = messageExpression
                        .Split(':')
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => x.Trim())
                        .ToArray();

        var left = expressions[0];
        var csharpOperator = expressions[1];
        var right = expressions[2];

        // removing double quotes from answer in input expression
        var answer = string.Concat(right.Split('"').Where(x => !string.IsNullOrWhiteSpace(x)));

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