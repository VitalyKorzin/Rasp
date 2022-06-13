using System;
using System.Linq;
using System.Collections.Generic;

public class Store
{
    public static void Main()
    {
        Good iPhone12 = new Good("IPhone 12");
        Good iPhone11 = new Good("IPhone 11");

        Warehouse warehouse = new Warehouse();

        Shop shop = new Shop(warehouse);

        warehouse.Delive(iPhone12, 10);
        warehouse.Delive(iPhone11, 1);

        //Вывод всех товаров на складе с их остатком
        foreach (var cell in warehouse.Cells)
            Console.WriteLine($"{cell.Good} - {cell.Count}");

        Cart cart = shop.Cart();
        cart.Add(iPhone12, 4);
        cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

        //Вывод всех товаров в корзине
        foreach (var cell in cart.Cells)
            Console.WriteLine($"{cell.Good} - {cell.Count}");

        Console.WriteLine(cart.Order().Paylink);

        cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
    }
}

public class Shop
{
    private readonly Warehouse _warehouse;

    public Shop(Warehouse warehouse)
    {
        if (warehouse == null)
            throw new ArgumentNullException(nameof(warehouse));

        _warehouse = warehouse;
    }

    public Cart Cart() => new Cart(_warehouse);
}

public class Warehouse
{
    private readonly List<Cell> _cells;

    public Warehouse()
        => _cells = new List<Cell>();

    public IReadOnlyList<IReadOnlyCell> Cells => _cells;

    public void Delive(Good good, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        var newCell = new Cell(good, count);

        if (TryFindCellIndex(good, out int cellIndex))
            _cells[cellIndex] = _cells[cellIndex].Merge(newCell);
        else
            _cells.Add(newCell);
    }

    public void Order(Good good, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (TryFindCellIndex(good, out int cellIndex))
            _cells[cellIndex] = _cells[cellIndex].RemoveGoods(count);
        else
            throw new InvalidOperationException();
    }

    private bool TryFindCellIndex(Good good, out int cellIndex)
    {
        cellIndex = _cells.FindIndex(cell => cell.Good.Name == good.Name);
        return cellIndex != -1;
    }
}

public interface IReadOnlyCell
{
    Good Good { get; }
    int Count { get; }
}

public class Cell : IReadOnlyCell
{
    public Cell(Good good, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        Good = good;
        Count = count;
    }

    public Good Good { get; }
    public int Count { get; }

    public Cell Merge(IReadOnlyCell newCell)
    {
        if (newCell == null)
            throw new ArgumentNullException(nameof(newCell));

        if (newCell.Good.Name != Good.Name)
            throw new InvalidOperationException();

        return new Cell(Good, Count + newCell.Count);
    }

    public Cell RemoveGoods(int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        if (count > Count)
            throw new InvalidOperationException();

        return new Cell(Good, Count - count);
    }
}

public class Cart
{
    private readonly Warehouse _warehouse;
    private readonly List<Cell> _cells;

    public Cart(Warehouse warehouse)
    {
        if (warehouse == null)
            throw new ArgumentNullException(nameof(warehouse));

        _warehouse = warehouse;
        _cells = new List<Cell>();
    }

    public IReadOnlyList<IReadOnlyCell> Cells => _cells;
    public bool Empty => _cells.Count == 0;

    public void Add(Good good, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        _warehouse.Order(good, count);
        var newCell = new Cell(good, count);

        if (TryFindCellIndex(good, out int cellIndex))
            _cells[cellIndex] = _cells[cellIndex].Merge(newCell);
        else
            _cells.Add(newCell);
    }

    public void Cancel()
    {
        if (Empty) return;

        foreach (var cell in _cells)
            _warehouse.Delive(cell.Good, cell.Count);

        _cells.Clear();
    }

    public Order Order()
    {
        if (Empty)
            throw new InvalidOperationException();

        _cells.Clear();
        return new Order();
    }

    private bool TryFindCellIndex(Good good, out int cellIndex)
    {
        cellIndex = _cells.FindIndex(cell => cell.Good.Name == good.Name);
        return cellIndex != -1;
    }
}

public struct Good
{
    public Good(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException();

        Name = name;
    }

    public string Name { get; }
}

public class Order
{
    public Order() => Paylink = "...";

    public string Paylink { get; }
}