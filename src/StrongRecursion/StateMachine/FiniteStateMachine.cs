using System;

namespace StrongRecursion.StateMachine
{
    public class FiniteStateMachine
    {
        public States State { get; private set; }
        public States PrevState { get; private set; }

        private int[,] _table;
        public FiniteStateMachine()
        {
            State = States.Empty;
            PrevState = States.Empty;
            _table = new int[9, 7]
            {
{(int)States.If, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error},
{(int)States.Error, (int)States.Then0, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error},
{(int)States.Error, (int)States.Error, (int)States.ElseIf, (int)States.Error, (int)States.Else, (int)States.Error, (int)States.Error},
{(int)States.Error, (int)States.Error, (int)States.Error, (int)States.Then1, (int)States.Error, (int)States.Error, (int)States.Error},
{(int)States.Error, (int)States.Error, (int)States.ElseIf, (int)States.Then1, (int)States.Else, (int)States.Error, (int)States.Ready},
{(int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Then2, (int)States.Ready},
{(int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Then2, (int)States.Ready},
{(int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error},
{(int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Error, (int)States.Ready}
            }; 
        }

        public States On(Transitions transition)
        {
            int row = (int)State;
            int column = (int)transition;
            PrevState = State;
            State = (States)_table[row, column];
            ValidateState();
            return State;
        }

        private void ValidateState()
        {
            if (State == States.Error)
                throw new InvalidOperationException("Recursion builder is in error state, please check your fluent expression");
        }
    }
}
