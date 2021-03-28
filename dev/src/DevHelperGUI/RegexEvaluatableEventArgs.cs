using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace DevHelperGUI
{
    public abstract class RegexEvaluatableEventArgs : CancelEventArgs
    {
        private readonly Regex _regex;
        public abstract string PrimaryInput { get; }
        public abstract bool IsSingleInput { get; }
        public abstract IEnumerable<string> AllInputs { get; }
        public Exception Exception { get; private set; }
        public object Result { get; set; }
        public string ErrorMessage => (Exception is null) ? (Cancel ? "Operation canceled" : null) :
            (string.IsNullOrWhiteSpace(Exception.Message) ? Exception.ToString() : Exception.Message);
        public EvaluationState State { get; private set; }
        public bool ShouldContinue => Exception is null && !Cancel;
        protected RegexEvaluatableEventArgs(Regex regex)
        {
            State = EvaluationState.NotEvaluated;
            _regex = regex;
        }

        private BinaryAlternate<T, Exception> ApplyResult<T>(string input, Func<string, T> func)
        {
            if (Cancel)
                throw new OperationCanceledException();
            if (!(Exception is null))
                return BinaryAlternate<T, Exception>.AsSecondary(Exception);
            BinaryAlternate<T, Exception> result = BinaryAlternate.TryInvoke(input, func);
            result.Apply(m =>
            {
                switch (State)
                {
                    case EvaluationState.Succeeded:
                    case EvaluationState.NotEvaluated:
                        State = EvaluationState.Succeeded;
                        break;
                    default:
                        State = EvaluationState.PartialSuccess;
                        break;
                }
            }, e =>
            {
                Exception = e;
                State = EvaluationState.EvaluationError;
            });
            return result;
        }

        private BinaryAlternate<MatchCollection, Exception> GetMatchCollection(string input) => ApplyResult(input, _regex.Matches);

        public BinaryAlternate<IList<MatchCollection>, Exception> GetAllMatchCollections()
        {
            List<MatchCollection> list = new List<MatchCollection>();
            foreach (string input in AllInputs)
            {
                if (Cancel)
                    break;
                BinaryAlternate<MatchCollection, Exception> result = GetMatchCollection(input);
                if (result.TryGetPrimary(out MatchCollection mc))
                    list.Add(mc);
                else
                    return BinaryAlternate<IList<MatchCollection>, Exception>.AsSecondary(result.SecondaryValue);
            }
            return BinaryAlternate<IList<MatchCollection>, Exception>.AsPrimary(list);
        }

        public BinaryAlternate<IList<TResult>, Exception> GetAllMatchCollections<TResult>(Func<int, string, MatchCollection, TResult> mapper)
        {
            List<TResult> list = new List<TResult>();
            int itemNumber = 0;
            foreach (string input in AllInputs)
            {
                if (Cancel)
                    break;
                BinaryAlternate<MatchCollection, Exception> result = GetMatchCollection(input);
                if (result.TryGetPrimary(out MatchCollection mc))
                    list.Add(mapper(++itemNumber, input, mc));
                else
                    return BinaryAlternate<IList<TResult>, Exception>.AsSecondary(result.SecondaryValue);
            }
            return BinaryAlternate<IList<TResult>, Exception>.AsPrimary(list);
        }

        public BinaryAlternate<MatchCollection, Exception> GetMatchCollection() => GetMatchCollection(PrimaryInput);

        private BinaryAlternate<Match, Exception> GetMatch(string input) => ApplyResult(input, _regex.Match);

        public BinaryAlternate<IList<TResult>, Exception> GetAllMatches<TResult>(Func<int, string, Match, TResult> mapper)
        {
            List<TResult> list = new List<TResult>();
            int itemNumber = 0;
            foreach (string input in AllInputs)
            {
                if (Cancel)
                    break;
                BinaryAlternate<Match, Exception> result = GetMatch(input);
                if (result.TryGetPrimary(out Match m))
                    list.Add(mapper(++itemNumber, input, m));
                else
                    return BinaryAlternate<IList<TResult>, Exception>.AsSecondary(result.SecondaryValue);
            }
            return BinaryAlternate<IList<TResult>, Exception>.AsPrimary(list);
        }

        public BinaryAlternate<IList<Match>, Exception> GetAllMatches()
        {
            List<Match> list = new List<Match>();
            foreach (string input in AllInputs)
            {
                if (Cancel)
                    break;
                BinaryAlternate<Match, Exception> result = GetMatch(input);
                if (result.TryGetPrimary(out Match mc))
                    list.Add(mc);
                else
                    return BinaryAlternate<IList<Match>, Exception>.AsSecondary(result.SecondaryValue);
            }
            return BinaryAlternate<IList<Match>, Exception>.AsPrimary(list);
        }

        public BinaryAlternate<Match, Exception> GetMatch() => GetMatch(PrimaryInput);

        public BinaryAlternate<IList<TResult>, Exception> GetAllSplitValues<TResult>(Func<int, string, string[], TResult> mapper)
        {
            List<TResult> list = new List<TResult>();
            int itemNumber = 0;
            foreach (string input in AllInputs)
            {
                if (Cancel)
                    break;
                BinaryAlternate<string[], Exception> result = GetSplitValues(input);
                if (result.TryGetPrimary(out string[] values))
                    list.Add(mapper(++itemNumber, input, values));
                else
                    return BinaryAlternate<IList<TResult>, Exception>.AsSecondary(result.SecondaryValue);
            }
            return BinaryAlternate<IList<TResult>, Exception>.AsPrimary(list);
        }

        public BinaryAlternate<IList<string[]>, Exception> GetAllSplitValues()
        {
            List<string[]> list = new List<string[]>();
            foreach (string input in AllInputs)
            {
                if (Cancel)
                    break;
                BinaryAlternate<string[], Exception> result = GetSplitValues(input);
                if (result.TryGetPrimary(out string[] mc))
                    list.Add(mc);
                else
                    return BinaryAlternate<IList<string[]>, Exception>.AsSecondary(result.SecondaryValue);
            }
            return BinaryAlternate<IList<string[]>, Exception>.AsPrimary(list);
        }

        private BinaryAlternate<string[], Exception> GetSplitValues(string input) => ApplyResult(input, _regex.Split);

        public BinaryAlternate<string[], Exception> GetSplitValues() => GetSplitValues(PrimaryInput);
    }
}
