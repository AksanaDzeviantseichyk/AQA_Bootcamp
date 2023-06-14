using System.Text.RegularExpressions;

namespace RegularExpression
{
    public static class RegularExpressionStore
    {
        private static readonly Regex _emailRegex = new Regex(@"\s*(?i)[a-z]+.(?i)[a-z]+@(?i)[a-z]+.co(m|m\s)$");
        private static readonly Regex _jsonNameRegex = new Regex(@"(?<="")\w+(?="":)");
        private static readonly Regex _jsonValueRegex = new Regex(@"(?<=:""?)\w+(?=""?)");
        private static readonly Regex _xmlNameRegex = new Regex(@"(?<=<)\w+(?=( \w+:\w+=""\w+"" /)?>)");
        private static readonly Regex _xmlValueRegex = new Regex(@"(?<=>)\w+(?=<)");
        private static readonly Regex _phoneRegex = new Regex(@"(?<!\d)((?:\+?38)[\s\(]*(068|067|095)[\s\)]*|\(?(068|067|095)\)?)[\s.-]*((\d{3}[\s.-]*\d{2}[\s.-]*\d{2})|\d{4}[\s.-]*\d{3})(?=[,|;\/]?)(?!\d)");


        // should return a bool indicating whether the input string is
        // a valid team international email address: firstName.lastName@domain (serhii.mykhailov@teaminternational.com etc.)
        // address cannot contain numbers
        // address cannot contain spaces inside, but can contain spaces at the beginning and end of the string
        public static bool Method1(string input)
        {
            return _emailRegex.IsMatch(input);
        }

        // the method should return a collection of field names from the json input
        public static IEnumerable<string> Method2(string inputJson)
        {
            return _jsonNameRegex.Matches(inputJson).Select(match => match.Value).ToArray();
        }

        // the method should return a collection of field values from the json input
        public static IEnumerable<string> Method3(string inputJson)
        {
            return _jsonValueRegex.Matches(inputJson).Select(match => match.Value).ToArray();
        }

        // the method should return a collection of field names from the xml input
        public static IEnumerable<string> Method4(string inputXml)
        {
            return _xmlNameRegex.Matches(inputXml).Select(match => match.Value).ToArray();
        }

        // the method should return a collection of field values from the input xml
        // omit null values
        public static IEnumerable<string> Method5(string inputXml)
        {
            return _xmlValueRegex.Matches(inputXml).Select(match => match.Value).ToArray();
        }

        // read from the input string and return Ukrainian phone numbers written in the formats of 0671234567 | +380671234567 | (067)1234567 | (067) - 123 - 45 - 67
        // +38 - optional Ukrainian country code
        // (067)-123-45-67 | 067-123-45-67 | 38 067 123 45 67 | 067.123.45.67 etc.
        // make a decision for operators 067, 068, 095 and any subscriber part.
        // numbers can be separated by symbols , | ; /
        public static IEnumerable<string> Method6(string input)
        {
            return _phoneRegex.Matches(input)
                .Select(match => match.Value
                    .StartsWith("38") ? "+38" + match.Value.Substring(2) : match.Value)
                .ToArray();
        }
    }
}