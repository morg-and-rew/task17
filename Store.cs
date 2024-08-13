using System;
using System.Collections.Generic;

public class Good
{
    public Good(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }
}

public interface IInventory
{
    bool HasEnough(Good good, int count);
    void Take(Good good, int count);
    void Deliver(Good good, int count);
}

public class Warehouse : IInventory
{
    private readonly Dictionary<Good, int> _goods = new Dictionary<Good, int>();

    public bool HasEnough(Good good, int count)
    {
        return _goods.ContainsKey(good) && _goods[good] >= count;
    }

    public void Take(Good good, int count)
    {
        if (good == null)
            throw new ArgumentNullException(nameof(good), "Good cannot be null.");

        if (count < 0)
            throw new ArgumentException("Count must be non-negative.");

        if (!_goods.ContainsKey(good) || _goods[good] < count)
            throw new Exception($"Not enough {good.Name} in warehouse.");

        _goods[good] -= count;
    }

    public void Deliver(Good good, int count)
    {
        if (good == null)
            throw new ArgumentNullException(nameof(good), "Good cannot be null.");

        if (count < 0)
            throw new ArgumentException("Count must be non-negative.");

        if (_goods.ContainsKey(good))
            _goods[good] += count;
        else
            _goods.Add(good, count);
    }
}

public class Cart
{
    private readonly IInventory _inventory;
    private readonly Dictionary<Good, int> _currentOrder = new Dictionary<Good, int>();

    public Cart(IInventory inventory)
    {
        if (inventory == null)
            throw new NullReferenceException("Inventory cannot be null.");

        _inventory = inventory;
    }

    public void Add(Good good, int count)
    {
        if (good == null)
            throw new ArgumentNullException(nameof(good), "Good cannot be null.");

        if (count < 0)
            throw new ArgumentException("Count must be non-negative.");

        if (!_inventory.HasEnough(good, count))
            throw new Exception($"Not enough {good.Name} in warehouse.");

        if (_currentOrder.ContainsKey(good))
            _currentOrder[good] += count;
        else
            _currentOrder.Add(good, count);
    }

    public Order Order()
    {
        foreach (var item in _currentOrder)
        {
            _inventory.Take(item.Key, item.Value);
        }

        _currentOrder.Clear();

        return new Order(_currentOrder);
    }
}

public class Order
{
    public Order(Dictionary<Good, int> items)
    {
        Items = items;
        Paylink = $"https://example.com/order/{Guid.NewGuid()}";
    }

    public string Paylink { get; private set; }
    public Dictionary<Good, int> Items { get; private set; }
}

public class Shop
{
    private readonly IInventory _inventory;

    public Shop(IInventory inventory)
    {
        if (inventory == null)
            throw new NullReferenceException("Inventory cannot be null.");

        _inventory = inventory;
    }

    public Cart Cart()
    {
        return new Cart(_inventory);
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

        warehouse.Deliver(iPhone12, 10);
        warehouse.Deliver(iPhone11, 1);

        Cart cart1 = shop.Cart();
        cart1.Add(iPhone12, 2);

        Cart cart2 = shop.Cart();
        cart2.Add(iPhone12, 1);

        Order order1 = cart1.Order();
        Order order2 = cart2.Order();
    }
}