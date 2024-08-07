using System;
using System.Collections.Generic;
using UnityEngine;

public class Good
{
    public string Name { get; private set; }

    public Good(string name)
    {
        Name = name;
    }
}

public class Warehouse
{
    private Dictionary<Good, int> _goods = new Dictionary<Good, int>();

    public void Delive(Good good, int count)
    {
        if (_goods.ContainsKey(good))
        {
            _goods[good] += count;
        }
        else
        {
            _goods.Add(good, count);
        }
    }

    public bool HasEnough(Good good, int count)
    {
        return _goods.ContainsKey(good) && _goods[good] >= count;
    }

    public void Take(Good good, int count)
    {
        if (HasEnough(good, count))
        {
            _goods[good] -= count;
        }
        else
        {
            throw new Exception($"Not enough {good.Name} in warehouse.");
        }
    }
}

public class Cart
{
    private Dictionary<Good, int> _items = new Dictionary<Good, int>();
    private Warehouse _warehouse;

    public Cart(Warehouse warehouse)
    {
        _warehouse = warehouse;
    }

    public void Add(Good good, int count)
    {
        if (_warehouse.HasEnough(good, count))
        {
            if (_items.ContainsKey(good))
            {
                _items[good] += count;
            }
            else
            {
                _items.Add(good, count);
            }

            _warehouse.Take(good, count);
        }
        else
        {
            throw new Exception($"Not enough {good.Name} in warehouse.");
        }
    }

    public Order Order()
    {
        return new Order(_items);
    }
}

public class Order
{
    public string Paylink { get; private set; }
    public Dictionary<Good, int> Items { get; private set; }

    public Order(Dictionary<Good, int> items)
    {
        Items = items;
        Paylink = $"https://example.com/order/{Guid.NewGuid()}";
    }
}

public class Shop
{
    private Warehouse _warehouse;

    public Shop(Warehouse warehouse)
    {
        _warehouse = warehouse;
    }

    public Cart Cart()
    {
        return new Cart(_warehouse);
    }
}

public class Example
{
    public static void Main(string[] args)
    {
        Good iPhone12 = new Good("IPhone 12");
        Good iPhone11 = new Good("IPhone 11");

        Warehouse warehouse = new Warehouse();

        Shop shop = new Shop(warehouse);

        warehouse.Delive(iPhone12, 10);
        warehouse.Delive(iPhone11, 1);

        Cart cart = shop.Cart();

        try
        {
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine(cart.Order().Paylink);

        try
        {
            cart.Add(iPhone12, 9);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
