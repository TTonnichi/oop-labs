namespace Laba2App.Models;

public class Coffee : Drink
{
    // поля класса
    private int _strength;
    private bool _hasMilk;

    // свойства
    public int Strength
    {
        get => _strength;
        set
        {
            if (value >= 1 && value <= 5)
                _strength = value;
        }
    }

    public bool HasMilk
    {
        get => _hasMilk;
        set => _hasMilk = value;
    }

    // полиморфные свойства
    public override string Type => "coffee";

    public override string ExtraInfo =>
        HasMilk ? "milk added" : "without milk";

    // конструктор по умолчанию
    public Coffee() : base("coffee", 200, 150)
    {
        _strength = 3;
        _hasMilk = false;
    }

    // основной конструктор
    public Coffee(string name, int volume, int price, int strength, bool hasMilk)
        : base(name, volume, price)
    {
        Strength = strength;
        HasMilk = hasMilk;
    }

    // конструктор копирования
    public Coffee(Coffee coffee)
        : base(coffee.Name, coffee.Volume, coffee.Price)
    {
        Strength = coffee.Strength;
        HasMilk = coffee.HasMilk;
    }

    // дополнительные методы
    public void AddMilk()
    {
        HasMilk = true;
    }

    public void RemoveMilk()
    {
        HasMilk = false;
    }

    // переопределённый метод
    public override string GetWishes()
    {
        return $"coffee with strength {Strength}/5";
    }

    public override string Info()
    {
        return $"coffee: {Name}, volume: {Volume} ml, price: {Price}, strength: {Strength}, milk: {(HasMilk ? "yes" : "no")}";
    }
}