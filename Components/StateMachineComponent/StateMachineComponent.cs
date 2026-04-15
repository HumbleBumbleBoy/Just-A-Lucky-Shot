using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachineComponent : Node
{
    [Export] public NodePath initialState;

    private Dictionary<string, State> _states;
    private State _currentState;

    public override void _Ready()
    {
        _states = new();
        foreach (Node node in GetChildren())
        {
            if (node is State states)
            {
                _states[node.Name] = states;
                states.stateMachineComponent = this;
                states.Ready();
                states.Exit(); // Reset states
            }
        }

        _currentState = GetNode<State>(initialState);
        _currentState.Enter();
    }

    public override void _Process(double delta)
    {
        _currentState.Update((float) delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        _currentState.PhysicsUpdate((float) delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        _currentState.HandleInput(@event);
    }

    public void TransitionTo(string state)
    {
        if (!_states.ContainsKey(state) || _currentState == _states[state]) { return; }
        _currentState.Exit();
        _currentState = _states[state];
        _currentState.Enter();
    }
}
