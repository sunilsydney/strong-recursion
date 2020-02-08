using System;
using System.Collections.Generic;
using StrongRecursion.StateMachine;
using System.Linq;

namespace StrongRecursion
{
    public class RecursionBuilder<TParams, TResult> : IRecursionBuilder<TParams, TResult>
        where TParams : Params
        where TResult : Result, new()
    {
        private FiniteStateMachine _stateMachine = new FiniteStateMachine();
        private RecursionEngine<TParams, TResult> _recursion;
        public RecursionBuilder()
        {
            _recursion = new RecursionEngine<TParams, TResult>();
        }


        /// <summary>
        /// The condition of base case aka limiting logic
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder<TParams, TResult> If(Func<TParams, bool> func)
        {
            _stateMachine.On(Transitions.If);
            _recursion.If = func;
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
            _recursion.Then = func;
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
            _recursion.ElseIfList.Add(func); // Will match with elemet of _thenList at same index
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
            //_stateMachine.On(Transitions.Then1);
            if(_stateMachine.State == States.ElseIf)
            {
                _stateMachine.On(Transitions.Then1);
                _recursion.ThenList.Add(func); // Will match with elemet of _elseIfList at same index                   
            }
            else
            {
                _recursion.ElseIfList.Add(_recursion.ElseIfList.Last()); // Duplicate the condition
                _recursion.ThenList.Add(func); // Will match with elemet of _elseIfList at same index
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
           // TODO  _recursion._else = func;
            return this;
        }

        public IRecursionEngine<TParams, TResult> Build()
        {
            if(_stateMachine.State == States.Empty)
            {
                throw new InvalidOperationException("Recursion builder not initialized, please check your fluent expression");
            }
            if(_stateMachine.State == States.Ready)
            {
                return _recursion;
            }
            throw new InvalidOperationException("Recursion builder is in error state, please check your fluent expression");
        }
        
    }
}
