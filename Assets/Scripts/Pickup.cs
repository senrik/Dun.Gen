
[System.Serializable]
public class Pickup {

	public enum PickupType
    {
        None = 0,
        Weapon = 1,
        Ammo = 2,
        Health = 3
    }

    private PickupType type;
    private string name;

    public Pickup()
    {
        name = "";
        type = PickupType.None;
    }

    public Pickup(string n)
    {
        name = n;
        type = PickupType.None;
    }

    public PickupType Type
    {
        get { return type; }
        set { type = value; }
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
}
