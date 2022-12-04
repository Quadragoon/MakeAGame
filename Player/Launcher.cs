using Godot;
using System;

public class Launcher : Node2D
{
    private float _timeBetweenLaunches;
    private float _timeUntilNextLaunch = 0;
    private int _numberToLaunch;
    private int _launchedCount;
    private PackedScene _launchedObject;
    private Vector2 _targetLocation;
    private bool _isLaunchingTargetedMissiles;
    private float _targetAreaRadius;
    private string _gameNode;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (GetNode("/root").HasNode("EndlessMode"))
            _gameNode = "/root/EndlessMode";
    }

    public void Init(PackedScene sceneToLaunch, float timeBetweenLaunches, Vector2 targetLocation, int numberToLaunch, bool launchImmediately = false, float targetAreaRadius = 50.0f, float launchAngleOffset = 0.0f)
    {
        _launchedObject = sceneToLaunch;
        _timeBetweenLaunches = timeBetweenLaunches;
        _targetLocation = targetLocation;
        _numberToLaunch = numberToLaunch;
        _targetAreaRadius = targetAreaRadius;
        Rotate(launchAngleOffset);

        _timeUntilNextLaunch = (launchImmediately ? 0 : _timeBetweenLaunches);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //GD.Print(Rotation);
        _timeUntilNextLaunch -= delta;
        while (_timeUntilNextLaunch <= 0 && _launchedCount < _numberToLaunch)
        {
            if (_launchedCount == 0)
            {
                Node newProjectile = _launchedObject.Instance();
                if (newProjectile is MissileTargeted)
                {
                    _isLaunchingTargetedMissiles = true;
                    MissileTargeted newMissile = newProjectile as MissileTargeted;

                    newMissile.GlobalPosition = GlobalPosition;
                    newMissile.Rotation = GlobalRotation;
                    newMissile.Rotate(GD.Randf() - 0.5f);
                    newMissile.TargetLocation = _targetLocation + Mathf.Polar2Cartesian((float)GD.RandRange(5, _targetAreaRadius), (float)GD.RandRange(0, Mathf.Tau));
                    newMissile.firedFrom = GetParent() as Node2D;
                    GetNode(_gameNode).AddChild(newMissile);
                }
            }
            else if (_isLaunchingTargetedMissiles)
            {
                MissileTargeted newMissile = _launchedObject.Instance<MissileTargeted>();
                newMissile.GlobalPosition = GlobalPosition;
                newMissile.Rotation = GlobalRotation;
                newMissile.Rotate(GD.Randf() - 0.5f);
                newMissile.TargetLocation = _targetLocation + Mathf.Polar2Cartesian((float)GD.RandRange(5, _targetAreaRadius), (float)GD.RandRange(0, Mathf.Tau));
                newMissile.firedFrom = GetParent() as Node2D;
                GetNode(_gameNode).AddChild(newMissile);
            }
            _launchedCount++;
            if (_launchedCount >= _numberToLaunch)
                QueueFree();
            _timeUntilNextLaunch += _timeBetweenLaunches;
        }
    }
}
