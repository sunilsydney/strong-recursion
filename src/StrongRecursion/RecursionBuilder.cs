using System;
using StrongRecursion.StateMachine;
using System.Linq;

namespace StrongRecursion
{
    public class RecursionBuilder<TParams, TResult> : IRecursionBuilder<TParams, TResult>
        where TParams : Params
        where TResult : Result, new()
    {
        private FiniteStateMachine _stateMachine = new FiniteStateMachine();
        private RecursionEngine<TParams, TResult> _engine;
        public RecursionBuilder()
        {
            _engine = new RecursionEngine<TParams, TResult>();
        }


        /// <summary>
        /// The condition of base case aka limiting logic
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder<TParams, TResult> If(Func<TParams, bool> func)
        {
            _stateMachine.On(Transitions.If);
            _engine.If = func;
            return this;
        }

        /// <summary>
        /// The logic of base case aka limiting logic
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder<TParams, TResult> Then(Func<TParams, TResult, TResult> func)
        {
            _stateMachine.On(Transitions.Then0);
            _engine.Then = func;
            return this;
        }
        
        /// <summary>
        /// When evaluated to true, leads to a recursive action
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder<TParams, TResult> ElseIf(Func<TParams, bool> func)
        {
            _stateMachine.On(Transitions.ElseIf);
            _engine.ElseIfList.Add(func); // Will match with elemet of _thenList at same index
            return this;
        }

        /// <summary>
        /// The logic to perform when the preceding condition is true.
        /// When used immediately following "ElseIf" or "Else", 
        /// this can be chained to support multiple actions or logical branching
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder<TParams, TResult> Then(Func<TParams, TResult, StackFrame<TParams, TResult>> func)
        {            
            if (_stateMachine.State == States.ElseIf)
            {
                _stateMachine.On(Transitions.Then1);
                // Will match with elemet of _elseIfList at same index                   
                _engine.ThenListList.Add(new ThenList<TParams, TResult>() { func});
            }
            else if (_stateMachine.State == States.Then1)
            {
                _stateMachine.On(Transitions.Then1);
                // Will match with elemet of _elseIfList at same index
                _engine.ThenListList.Last().Add(func);
            }
            else if (_stateMachine.State == States.Else)
            {
                _stateMachine.On(Transitions.Then2);
                _engine.ElseList.Add(func);
            }
            else if (_stateMachine.State == States.Then2)
            {
                _stateMachine.On(Transitions.Then2);
                _engine.ElseList.Add(func);
            }
            
            return this;
        }

        /// <summary>
        /// Optional action to perform if no condition evaluated to true
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder<TParams, TResult> Else(Func<TParams, TResult, StackFrame<TParams, TResult>> func)
        {
            _stateMachine.On(Transitions.Else);
            _engine.ElseList.Add(func);
            return this;
        }

        private void ValidateState()
        {
            if (_stateMachine.State == States.Empty)
            {
                throw new InvalidOperationException("Recursion builder not initialized, please check your fluent expression");
            }
            if (_stateMachine.State != States.Ready)
            {
                throw new InvalidOperationException("Recursion builder is in error state, please check your fluent expression");
            }
        }

        /// <summary>
        /// Returns an IRecursionEngine
        /// </summary>
        /// <returns></returns>
        public IRecursionEngine<TParams, TResult> Build()
        {
            _stateMachine.On(Transitions.Finish);
            ValidateState();
            return _engine;         
        }
    }
}
