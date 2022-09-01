using System.Text.RegularExpressions;

var answersDict = new Dictionary<string, string>()
{
    { "Token_2", "2" },
    { "Token_3", "55" }
};

// Currently works only for separated if/elseif/elses with only one level. 
// TODO: create new version so that it can be parsed to when there are nested conditions
// example: (ex1) ? (ex2) : MSG2 : MSG1

string testinput = "(Token_2 != \"2\") ? OK : (Token_3 == \"55.0\") ? OK : (Token_2 == \"99\") ? ERROR : (Token_2 == \"100\") ? WRONG : WARNING";
Console.WriteLine(Parse(testinput, answersDict));
ParseMultipleResult(testinput, answersDict).ForEach(x=>Console.WriteLine(x));

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

        decimal providedAnswer = ToNumber(answers[left]);

        // check the possible answer on expression if it matches the answer provided by user
        switch (csharpOperator)
        {
            case "==" when providedAnswer == answer:
                return messages[0];
            case "!=" when providedAnswer != answer:
                return messages[0];
            case ">" when providedAnswer > answer:
                return messages[0];
            case "<" when providedAnswer < answer:
                return messages[0];
            case "<=" when providedAnswer <= answer:
                return messages[0];
            case ">=" when providedAnswer >= answer:
                return messages[0];
        }

        // usually the last else
        if (messages.Length == 2)
        {
            return messages[1];
        }
    }

    throw new Exception("Couldn't parse the ternary expression");
}

static List<(string token, string message)> ParseMultipleResult(string input, Dictionary<string, string> answers)
{
    var result = new List<(string token, string message)>();

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
        decimal answer = ParsePossibleAnswer(right);

        decimal providedAnswer = ToNumber(answers[left]);

        switch (csharpOperator)
        {
            case "==" when providedAnswer == answer:
                result.Add((token: left, message: messages[0]));
                continue;
            case "!=" when providedAnswer != answer:
                result.Add((token: left, message: messages[0]));
                continue;
            case ">" when providedAnswer > answer:
                result.Add((token: left, message: messages[0]));
                continue;
            case "<" when providedAnswer < answer:
                result.Add((token: left, message: messages[0]));
                continue;
            case "<=" when providedAnswer <= answer:
                result.Add((token: left, message: messages[0]));
                continue;
            case ">=" when providedAnswer >= answer:
                result.Add((token: left, message: messages[0]));
                continue;
        }

        // the last else
        if (!result.Any() && messages.Length == 2)
        {
            result.Add((token: left, message: messages[1]));
        }
    }

    return result;
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
