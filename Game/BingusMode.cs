using Godot;
using System;

public class BingusMode : Game
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

    enum PeerType {server, client};

    public void _OnBingusPressed()
    {
        _SetupConnection(PeerType.server);
    }

    public void _OnSpoingusPressed()
    {
        _SetupConnection(PeerType.client);
    }

    private void _SetupConnection(PeerType peerType)
    {
        Button bingusButton = GetNode<Button>("HUD/HBoxContainer/Bingus");
        bingusButton.Disabled = true;
        Button spoingusButton = GetNode<Button>("HUD/HBoxContainer/Spoingus");
        spoingusButton.Disabled = true;
        SceneTree tree = GetTree();
        NetworkedMultiplayerENet peer = new NetworkedMultiplayerENet();
        if (peerType == PeerType.server)
            peer.CreateServer(34125);
        else if (peerType == PeerType.client)
            peer.CreateClient("100.84.7.16", 36125);
        tree.NetworkPeer = peer;
        tree.Connect("network_peer_connected", this, nameof(_PeerConnected));
        tree.Connect("network_peer_disconnected", this, nameof(_PeerDisconnected));
        tree.Connect("connected_to_server", this, nameof(_ConnectedToServer));

        if (peerType == PeerType.server)
        {
            PlayerBingus myBingus = GD.Load<PackedScene>("res://Player/PlayerBingus.tscn").Instance() as PlayerBingus;
            myBingus.Name = GetTree().GetNetworkUniqueId().ToString();
            myBingus.SetNetworkMaster(GetTree().GetNetworkUniqueId());
            GetNode("/root/Game").AddChild(myBingus);
            myBingus.GetNode<Label>("Label").Text = GetTree().GetNetworkUniqueId().ToString();

            Camera2D newCamera = new Camera2D();
            newCamera.Current = true;
            myBingus.AddChild(newCamera);
        }
    }

    public void _PeerConnected(int id)
    {
        GD.Print("PeerConnected");
        PlayerBingus newBingus = GD.Load<PackedScene>("res://Player/PlayerBingus.tscn").Instance() as PlayerBingus;
        newBingus.Name = id.ToString();
        newBingus.SetNetworkMaster(id);
        GetNode("/root/Game").AddChild(newBingus);
        newBingus.GetNode<Label>("Label").Text = id.ToString();
    }

    public void _PeerDisconnected(int id)
    {
        GD.Print("PeerDisconnected");
    }

    public void _ConnectedToServer()
    {
        PlayerBingus myBingus = GD.Load<PackedScene>("res://Player/PlayerBingus.tscn").Instance() as PlayerBingus;
        myBingus.Name = GetTree().GetNetworkUniqueId().ToString();
        myBingus.SetNetworkMaster(GetTree().GetNetworkUniqueId());
        GetNode("/root/Game").AddChild(myBingus);
        myBingus.GetNode<Label>("Label").Text = GetTree().GetNetworkUniqueId().ToString();

        Camera2D newCamera = new Camera2D();
        newCamera.Current = true;
        myBingus.AddChild(newCamera);
    }
}
