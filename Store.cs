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
    Console.WriteLine($"Ошибка: {ex.Message}");
}

Console.WriteLine(cart.Order().Paylink);

try
{
    cart.Add(iPhone12, 9);
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}