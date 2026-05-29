// основное окно приложения

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

using Laba2App.Data;
using Laba2App.Models;

using System;
using System.Collections.ObjectModel;
using Avalonia.Input;
namespace Laba2App.Views;

public partial class MainWindow : Window
{
    // подключение к базе данных

    private readonly Database _db = new();

    // коллекция напитков для таблицы

    private readonly ObservableCollection<Drink> _drinks = new();

    // редактируемый напиток

    private Drink? _editingDrink = null;

    // кнопка добавления

    private Button? _addButton = null;
    
    // кнопки экстра
    private Button? _addExtraButton = null;
    private Button? _removeExtraButton = null;
    
    // кнопка отмены
    private Button? _cancelButton = null;

    // конструктор окна

    public MainWindow()
    {
        InitializeComponent();

        SetupUi();
        LoadData();
        
        // подписываемся на изменение выбранного элемента в таблице
        MyGrid.SelectionChanged += OnSelectionChanged;
    }

    // начальная настройка интерфейса

    private void SetupUi()
    {
        StrengthTextBox.IsVisible = true;
        TeaTypeComboBox.IsVisible = false;

        _addButton = this.FindControl<Button>("AddButton");
        _addExtraButton = this.FindControl<Button>("AddExtraButton");
        _removeExtraButton = this.FindControl<Button>("RemoveExtraButton");
        _cancelButton = this.FindControl<Button>("CancelButton");
        
        // кнопка отмены изначально скрыта
        if (_cancelButton != null)
            _cancelButton.IsVisible = false;
            
        // кнопки экстра изначально неактивны
        if (_addExtraButton != null)
            _addExtraButton.IsEnabled = false;
        if (_removeExtraButton != null)
            _removeExtraButton.IsEnabled = false;
    }
    
    // изменения выбранного элемента в таблице
    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (MyGrid.SelectedItem is Coffee coffee)
        {
            // кофе выбран
            if (_addExtraButton != null)
                _addExtraButton.IsEnabled = !coffee.HasMilk; // если нет молока - можно добавить
            if (_removeExtraButton != null)
                _removeExtraButton.IsEnabled = coffee.HasMilk; // если есть молоко - можно удалить
        }
        else if (MyGrid.SelectedItem is Tea tea)
        {
            // чай выбран
            if (_addExtraButton != null)
                _addExtraButton.IsEnabled = !tea.HasLemon; // если нет лимона - можно добавить
            if (_removeExtraButton != null)
                _removeExtraButton.IsEnabled = tea.HasLemon; // если есть лимон - можно удалить
        }
        else
        {
            // ничего не выбрано
            if (_addExtraButton != null)
                _addExtraButton.IsEnabled = false;
            if (_removeExtraButton != null)
                _removeExtraButton.IsEnabled = false;
        }
    }

    // загрузка данных из базы данных

    private void LoadData()
    {
        try
        {
            _drinks.Clear();

            var drinks = _db.GetAllDrinks();

            foreach (var drink in drinks)
            {
                _drinks.Add(drink);
            }

            MyGrid.ItemsSource = _drinks;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    // изменение интерфейса при смене типа напитка

    private void OnTypeChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (StrengthTextBox == null || TeaTypeComboBox == null || TypeComboBox == null)
            return;
        
        string? type =
            (TypeComboBox.SelectedItem as ComboBoxItem)?
            .Content?
            .ToString();

        bool isCoffee = type == "Coffee";

        StrengthTextBox.IsVisible = isCoffee;
        TeaTypeComboBox.IsVisible = !isCoffee;
    }


    // это вообще для циферок

    private void OnlyNumbersKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Back || e.Key == Key.Delete || 
            e.Key == Key.Left || e.Key == Key.Right)
            return;
        if (e.Key < Key.D0 || e.Key > Key.D9)
        {
            if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
            {
                e.Handled = true;
            }
        }
    }

    // обновление таблицы

    private void OnRefreshClick(object? sender, RoutedEventArgs e)
    {
        LoadData();
    }

    // добавление напитка

    private void OnAddClick(object? sender, RoutedEventArgs e)
    {
        string? type = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
        string name = NameTextBox.Text?.Trim() ?? "";
        int.TryParse(VolumeTextBox.Text, out int volume);
        int.TryParse(PriceTextBox.Text, out int price);
        
        if (_editingDrink != null)
        {
            // РЕДАКТИРОВАНИЕ существующего напитка
            if (_editingDrink is Coffee coffee && type == "Coffee")
            {
                coffee.Name = name;
                coffee.Volume = volume;
                coffee.Price = price;
                int.TryParse(StrengthTextBox.Text, out int strength);
                if (strength > 0) coffee.Strength = strength;
                _db.UpdateCoffee(coffee);
            }
            else if (_editingDrink is Tea tea && type == "Tea")
            {
                tea.Name = name;
                tea.Volume = volume;
                tea.Price = price;
                string teaType = (TeaTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "black";
                tea.TeaType = teaType;
                _db.UpdateTea(tea);
            }
            else
            {
                return;
            }
            ResetEditMode();
        }
        else
        {
            // ДОБАВЛЕНИЕ нового напитка
            if (type == "Coffee")
            {
                int.TryParse(StrengthTextBox.Text, out int strength);
                if (strength <= 0) strength = 3;
                Coffee coffee;
                if (string.IsNullOrWhiteSpace(name) || volume <= 0 || price <= 0)
                    coffee = new Coffee();
                else
                    coffee = new Coffee(name, volume, price, strength, false);
                _db.AddCoffee(coffee);
            }
            else if (type == "Tea")
            {
                string teaType = (TeaTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "black";
                Tea tea;
                if (string.IsNullOrWhiteSpace(name) || volume <= 0 || price <= 0)
                    tea = new Tea();
                else
                    tea = new Tea(name, volume, price, teaType, false);
                _db.AddTea(tea);
            }
        }
        
        LoadData();
    }


    // удаление напитка

    private void OnDeleteClick(object? sender, RoutedEventArgs e)
    {
        if (MyGrid.SelectedItem is not Drink drink)
            return;

        _db.DeleteDrink(drink.Id);

        LoadData();
    }

    // сброс режима редактирования

    private void ResetEditMode()
    {
        _editingDrink = null;
        if (_addButton != null)
            _addButton.Content = "Add";
        
        // скрываем кнопку отмены
        if (_cancelButton != null)
            _cancelButton.IsVisible = false;
        
        // разблокируем комбобокс типа
        TypeComboBox.IsEnabled = true;
        
        // очищаем поля
        NameTextBox.Text = "";
        VolumeTextBox.Text = "";
        PriceTextBox.Text = "";
        StrengthTextBox.Text = "";
        TeaTypeComboBox.SelectedIndex = 0;
        TypeComboBox.SelectedIndex = 0;
    }

    // создание копии объекта

    private void OnDuplicateClick(object? sender, RoutedEventArgs e)
    {
        if (MyGrid.SelectedItem is Coffee coffee)
        {
            Coffee copy = new Coffee(coffee);
            _db.AddCoffee(copy);
        }
        else if (MyGrid.SelectedItem is Tea tea)
        {
            Tea copy = new Tea(tea);
            _db.AddTea(copy);
        }

        LoadData();
    }

    // добавление молока или лимона

    private void OnAddExtraClick(object? sender, RoutedEventArgs e)
    {
        if (MyGrid.SelectedItem is Coffee coffee)
        {
            coffee.AddMilk();
            _db.UpdateCoffeeMilk(coffee.Id, coffee.HasMilk);
        }
        else if (MyGrid.SelectedItem is Tea tea)
        {
            tea.AddLemon();
            _db.UpdateTeaLemon(tea.Id, tea.HasLemon);
        }
        else
        {
            return;
        }

        LoadData();
    }

    // удаление молока или лимона

    private void OnRemoveExtraClick(object? sender, RoutedEventArgs e)
    {
        if (MyGrid.SelectedItem is Coffee coffee)
        {
            coffee.RemoveMilk();
            _db.UpdateCoffeeMilk(coffee.Id, coffee.HasMilk);
        }
        else if (MyGrid.SelectedItem is Tea tea)
        {
            tea.RemoveLemon();
            _db.UpdateTeaLemon(tea.Id, tea.HasLemon);
        }
        else
        {
            return;
        }

        LoadData();
    }

    // отображение пожеланий

    private async void OnWishClick(object? sender, RoutedEventArgs e)
    {
        if (MyGrid.SelectedItem is not Drink drink)
            return;

        string wish = drink.GetWishes();

        await ShowDialog(
            "wish",
            wish,
            "#FFD700"
        );
    }

    // отображение информации о напитке

    private async void OnInfoClick(object? sender, RoutedEventArgs e)
    {
        if (MyGrid.SelectedItem is not Drink drink)
            return;

        await ShowDialog(
            "drink info",
            drink.Info(),
            "#EAEAEA"
        );
    }

    // редактирование напитка

    private void OnEditClick(object? sender, RoutedEventArgs e)
    {
        if (MyGrid.SelectedItem is not Drink drink) return;
        
        // показываем кнопку отмены
        if (_cancelButton != null)
            _cancelButton.IsVisible = true;
        
        // Заполняем поля ввода данными выбранного напитка
        NameTextBox.Text = drink.Name;
        VolumeTextBox.Text = drink.Volume.ToString();
        PriceTextBox.Text = drink.Price.ToString();
        
        if (drink is Coffee coffee)
        {
            TypeComboBox.SelectedIndex = 0;
            StrengthTextBox.Text = coffee.Strength.ToString();
            TypeComboBox.IsEnabled = false;
            StrengthTextBox.IsVisible = true;
            TeaTypeComboBox.IsVisible = false;
        }
        else if (drink is Tea tea)
        {
            TypeComboBox.SelectedIndex = 1;
            foreach (ComboBoxItem item in TeaTypeComboBox.Items)
            {
                if (item.Content?.ToString() == tea.TeaType)
                {
                    TeaTypeComboBox.SelectedItem = item;
                    break;
                }
            }
            TypeComboBox.IsEnabled = false;
            StrengthTextBox.IsVisible = false;
            TeaTypeComboBox.IsVisible = true;
        }
        
        _editingDrink = drink;
        if (_addButton != null)
            _addButton.Content = "Update";
    }

    // отмена редактирования

    private void OnCancelEditClick(object? sender, RoutedEventArgs e)
    {
        ResetEditMode();
    }

    // универсальное диалоговое окно

    private async System.Threading.Tasks.Task ShowDialog(
        string title,
        string message,
        string color)
    {
        var dialog = new Window
        {
            Title = title,

            Width = 400,
            Height = 220,

            Background =
                new SolidColorBrush(
                    Color.Parse("#2A2A33")
                ),

            Content = new StackPanel
            {
                Margin = new Avalonia.Thickness(20),
                Spacing = 12,

                Children =
                {
                    new TextBlock
                    {
                        Text = title,

                        FontSize = 22,

                        Foreground =
                            new SolidColorBrush(
                                Color.Parse(color)
                            )
                    },

                    new TextBlock
                    {
                        Text = message,

                        FontSize = 16,

                        TextWrapping =
                            Avalonia.Media.TextWrapping.Wrap,

                        Foreground =
                            new SolidColorBrush(
                                Color.Parse("#EAEAEA")
                            )
                    }
                }
            }
        };

        await dialog.ShowDialog(this);
    }
}