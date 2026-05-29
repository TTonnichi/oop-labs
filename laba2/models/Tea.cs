namespace Laba2App.Models;

public class Tea : Drink
{
    // поля класса
    private string _teaType;
    private bool _hasLemon;

    // свойства
    public string TeaType
    {
        get => _teaType;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _teaType = value;
        }
    }

    public bool HasLemon
    {
        get => _hasLemon;
        set => _hasLemon = value;
    }

    // полиморфные свойства
    public override string Type => "tea";

    public override string ExtraInfo =>
        HasLemon ? "lemon added" : "without lemon";

    // конструктор по умолчанию
    public Tea() : base("tea", 250, 100)
    {
        _teaType = "black";
        _hasLemon = false;
    }

    // основной конструктор
    public Tea(string name, int volume, int price, string teaType, bool hasLemon)
        : base(name, volume, price)
    {
        TeaType = teaType;
        HasLemon = hasLemon;
    }

    // конструктор копирования
    public Tea(Tea tea)
        : base(tea.Name, tea.Volume, tea.Price)
    {
        TeaType = tea.TeaType;
        HasLemon = tea.HasLemon;
    }

    // дополнительные методы
    public void AddLemon()
    {
        HasLemon = true;
    }

    public void RemoveLemon()
    {
        HasLemon = false;
    }

    // переопределённый метод

    public override string GetWishes()
    {
        return $"tea type: {TeaType}";
    }


    public override string Info()
    {
        return $"tea: {Name}, volume: {Volume} ml, price: {Price}, TeaType: {TeaType}, lemon: {(HasLemon ? "yes" : "no")}";
    }
}