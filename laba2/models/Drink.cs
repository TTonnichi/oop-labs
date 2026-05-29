namespace Laba2App.Models;

public abstract class Drink
{
    // поля класса

    private int _id;
    private string _name;
    private int _volume;
    private int _price;

    // свойства

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _name = value;
        }
    }

    public int Volume
    {
        get => _volume;
        set
        {
            if (value > 0)
                _volume = value;
        }
    }

    public int Price
    {
        get => _price;
        set
        {
            if (value > 0)
                _price = value;
        }
    }

    // полиморфные свойства

    public abstract string Type { get; }

    public abstract string ExtraInfo { get; }

    public string InfoText => Info();

    // конструктор

    protected Drink(string name, int volume, int price)
    {
        Name = name;
        Volume = volume;
        Price = price;
    }

    // виртуальный метод информации

    public virtual string Info()
    {
        return $"name: {Name}, volume: {Volume} ml, price: {Price}";
    }

    // абстрактный метод

    public abstract string GetWishes();

    // деструктор

    ~Drink()
    {
        System.Diagnostics.Debug.WriteLine($"deleting drink {Name}");
    }
}