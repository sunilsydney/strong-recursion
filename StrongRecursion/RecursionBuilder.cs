using System;

namespace StrongRecursion
{
    public class RecursionBuilder
    {
        public RecursionBuilder()
        { }

        Func<Params, bool> _limitingCondition;
        Func<Params, Result, Result> _limitingLogic;
        Func<Params, Result, Result> _logic;

        public RecursionBuilder WithLimitingCondition(Func<Params, bool> func)
        {
            _limitingCondition = func;
            return this;
        }

        public RecursionBuilder WithLimitingLogic(Func<Params, Result, Result> func)
        {
            _limitingLogic = func;
            return this;
        }

        public RecursionBuilder WithLogic(Func<Params, Result, Result> func)
        {
            _logic = func;
            return this;
        }

        public Result Run(Params prms)
        {
            return null;
        }
    }
}
