namespace ExpertSystem
{
    public class MatchResult
    {
        public static MatchResult Match { get; } = new MatchResult(true);

        public static MatchResult NotMatch { get; } = new MatchResult(false);

        public static MatchResult Undefined(string unknownProperty) => new MatchResult(null, unknownProperty);

        public MatchResult(bool? matching, string unknownProperty = null)
        {
            Matching = matching;
            UnknownProperty = unknownProperty;
        }

        public bool? Matching { get; }

        public string UnknownProperty { get; }

        public bool IsMatch => Matching == true;

        public bool IsNotMatch => Matching == false;

        public bool IsUndefined => Matching == null;
    }
}