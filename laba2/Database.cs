// библиотека dapper для связи таблиц бд с объектами c#
using Dapper;
// библиотека npgsql для работы с субд postgresql
using Npgsql;
using System.Collections.Generic;

using Laba2App.Models;

namespace Laba2App.Data;

public class Database
{
    // строка подключения к postgresql

    private readonly string _connectionString =
        "Host=localhost;Username=tonya;Password=1234;Database=coffee_shop";

    // получение всех напитков из бдшки

    public List<Drink> GetAllDrinks()
    {
        using var connection =
            new NpgsqlConnection(_connectionString);

        var drinks = new List<Drink>();

        // выполнение запроса на выборку данных из трех таблиц
        var result = connection.Query(@"
            SELECT
                d.id,
                d.name,
                d.volume,
                d.price,
                d.drink_type,

                c.strength,
                c.has_milk,

                t.tea_type,
                t.has_lemon

            FROM drinks d

            LEFT JOIN coffee c
                ON d.id = c.id

            LEFT JOIN tea t
                ON d.id = t.id
        ");

        foreach (var item in result)
        {
            // определение типа напитка из строки бд
            string type =
                item.drink_type
                .ToString()
                .ToLower();

            // создание объекта coffee

            if (type == "coffee")
            {
                var coffee = new Coffee(
                    item.name,
                    item.volume,
                    item.price,
                    item.strength ?? 3,
                    item.has_milk ?? false
                );

                // присвоение идентификатора объекту
                coffee.Id = item.id;

                drinks.Add(coffee);
            }

            // создание объекта tea

            else if (type == "tea")
            {
                var tea = new Tea(
                    item.name,
                    item.volume,
                    item.price,
                    item.tea_type ?? "black",
                    item.has_lemon ?? false
                );

                // присвоение идентификатора объекту
                tea.Id = item.id;

                drinks.Add(tea);
            }
        }

        return drinks;
    }

    // добавление кофе

    public void AddCoffee(Coffee coffee)
    {
        using var connection =
            new NpgsqlConnection(_connectionString);

        // вставка в главную таблицу и получение нового id
        int id = connection.ExecuteScalar<int>(@"
            INSERT INTO drinks
            (
                name,
                volume,
                price,
                drink_type
            )

            VALUES
            (
                @Name,
                @Volume,
                @Price,
                'coffee'
            )

            RETURNING id
        ",
        new
        {
            coffee.Name,
            coffee.Volume,
            coffee.Price
        });

        // вставка в таблицу кофе с привязкой к id
        connection.Execute(@"
            INSERT INTO coffee
            (
                id,
                strength,
                has_milk
            )

            VALUES
            (
                @Id,
                @Strength,
                @HasMilk
            )
        ",
        new
        {
            Id = id,

            coffee.Strength,
            coffee.HasMilk
        });
    }

    // добавление чая

    public void AddTea(Tea tea)
    {
        using var connection =
            new NpgsqlConnection(_connectionString);

        // вставка в главную таблицу и получение нового id
        int id = connection.ExecuteScalar<int>(@"
            INSERT INTO drinks
            (
                name,
                volume,
                price,
                drink_type
            )

            VALUES
            (
                @Name,
                @Volume,
                @Price,
                'tea'
            )

            RETURNING id
        ",
        new
        {
            tea.Name,
            tea.Volume,
            tea.Price
        });

        // вставка в таблицу чая с привязкой к id
        connection.Execute(@"
            INSERT INTO tea
            (
                id,
                tea_type,
                has_lemon
            )

            VALUES
            (
                @Id,
                @TeaType,
                @HasLemon
            )
        ",
        new
        {
            Id = id,

            tea.TeaType,
            tea.HasLemon
        });
    }

    // обновление кофе
    public void UpdateCoffee(Coffee coffee)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        
        // обновляем общие поля в таблице drinks
        connection.Execute(@"
            UPDATE drinks
            SET name = @Name,
                volume = @Volume,
                price = @Price
            WHERE id = @Id",
            new { coffee.Id, coffee.Name, coffee.Volume, coffee.Price });
        
        // обновляем специфические поля в таблице coffee
        connection.Execute(@"
            UPDATE coffee
            SET strength = @Strength,
                has_milk = @HasMilk
            WHERE id = @Id",
            new { coffee.Id, coffee.Strength, coffee.HasMilk });
    }

    // обновление чая
    public void UpdateTea(Tea tea)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        
        connection.Execute(@"
            UPDATE drinks
            SET name = @Name,
                volume = @Volume,
                price = @Price
            WHERE id = @Id",
            new { tea.Id, tea.Name, tea.Volume, tea.Price });
        
        connection.Execute(@"
            UPDATE tea
            SET tea_type = @TeaType,
                has_lemon = @HasLemon
            WHERE id = @Id",
            new { tea.Id, tea.TeaType, tea.HasLemon });
    }

    // обновление состояния молока

    public void UpdateCoffeeMilk(int id, bool hasMilk)
    {
        using var connection =
            new NpgsqlConnection(_connectionString);

        // изменение значения флага молока по id
        connection.Execute(@"
            UPDATE coffee

            SET has_milk = @HasMilk

            WHERE id = @Id
        ",
        new
        {
            Id = id,
            HasMilk = hasMilk
        });
    }

    // обновление состояния лимона

    public void UpdateTeaLemon(int id, bool hasLemon)
    {
        using var connection =
            new NpgsqlConnection(_connectionString);

        // изменение значения флага лимона по id
        connection.Execute(@"
            UPDATE tea

            SET has_lemon = @HasLemon

            WHERE id = @Id
        ",
        new
        {
            Id = id,
            HasLemon = hasLemon
        });
    }

    // deletion напитка

    public void DeleteDrink(int id)
    {
        using var connection =
            new NpgsqlConnection(_connectionString);

        // удаление записи из главной таблицы по id
        connection.Execute(@"
            DELETE FROM drinks

            WHERE id = @Id
        ",
        new
        {
            Id = id
        });
    }
}
