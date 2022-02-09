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

        // remove double quotes and convert to decimal
        decimal answer = ParsePossibleAnswer(right);

        // check the possible answer on expression if it matches the answer provided by user
        if (csharpOperator == "==" && ToNumber(answers[left]) == answer)
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

// remove empty items and trim them
static string[] TrimCollection(IEnumerable<string> collection)
    => collection.Where(x => !string.IsNullOrWhiteSpace(x))
                 .Select(x => x.Trim())
                 .ToArray();

// converts string to decimal
static decimal ToNumber(string input)
{
    decimal.TryParse(input, out var res);
    return res;
}

// removes double quotes from string and convert to decimal
static decimal ParsePossibleAnswer(string expression)
    => ToNumber(string.Concat(TrimCollection(expression.Split('"'))));
