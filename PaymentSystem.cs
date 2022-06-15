using System;

public static class PaymentSystem
{
    public static void Main()
    {
        //Выведите платёжные ссылки для трёх разных систем платежа: 
        //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
        //order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
        //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа
        //+ секретный ключ от системы}

        string payingLink_1 = "pay.system1.ru/order?amount=12000RUB&hash=";
        string payingLink_2 = "order.system2.ru/pay?hash=";
        string payingLink_3 = "system3.com/pay?amount=12000&curency=RUB&hash=";
        var order = new Order(12, 30);
        string seretKey = "123";

        var PaymentSystem_1 = new PaymentSystem_1(payingLink_1, new MD5Hashing());
        var PaymentSystem_2 = new PaymentSystem_2(payingLink_2, new MD5Hashing());
        var PaymentSystem_3 = new PaymentSystem_3(payingLink_3, new SHA1Hashing(), seretKey);
        Console.WriteLine(PaymentSystem_1.GetPayingLink(order));
        Console.WriteLine(PaymentSystem_2.GetPayingLink(order));
        Console.WriteLine(PaymentSystem_3.GetPayingLink(order));
        Console.ReadKey();
    }
}

public class Order
{
    public readonly int Id;
    public readonly int Amount;

    public Order(int id, int amount) => (Id, Amount) = (id, amount);
}

public interface IPaymentSystem
{
    string GetPayingLink(Order order);
}

public abstract class PaymentSystem : IPaymentSystem
{
    private readonly string _payingLink;
    private readonly IHashing _hashing;

    public PaymentSystem(string payingLink, IHashing hashing)
    {
        if (string.IsNullOrEmpty(payingLink))
            throw new ArgumentNullException();

        if (hashing == null)
            throw new ArgumentNullException();

        _payingLink = payingLink;
        _hashing = hashing;
    }

    public string GetPayingLink(Order order)
    {
        if (order == null)
            throw new ArgumentNullException();

        return _payingLink + GetAdditionToLink(order);
    }

    protected string Hash(string message)
        => _hashing.Hash(message);

    protected virtual string GetAdditionToLink(Order order)
        => Hash(order.Id.ToString() + order.Amount);
}

public class PaymentSystem_1 : PaymentSystem
{
    public PaymentSystem_1(string payingLink, IHashing hashing)
        : base(payingLink, hashing) { }

    protected override string GetAdditionToLink(Order order)
        => Hash(order.Id.ToString());
}

public class PaymentSystem_2 : PaymentSystem
{
    public PaymentSystem_2(string payingLink, IHashing hashing)
        : base(payingLink, hashing) { }

    protected override string GetAdditionToLink(Order order)
        => Hash(order.Id.ToString()) + order.Amount;
}

public class PaymentSystem_3 : PaymentSystem
{
    private readonly string _secretKey;

    public PaymentSystem_3(string payingLink, IHashing hashing, string secretKey)
        : base(payingLink, hashing)
    {
        if (string.IsNullOrEmpty(secretKey))
            throw new ArgumentNullException();

        _secretKey = secretKey;
    }

    protected override string GetAdditionToLink(Order order)
        => Hash(order.Amount.ToString()) + order.Id + _secretKey;
}

public interface IHashing
{
    string Hash(string message);
}

public abstract class Hashing : IHashing
{
    private readonly HashAlgorithm _hashAlgorithm;

    public Hashing(HashAlgorithm hashAlgorithm)
    {
        if (hashAlgorithm == null)
            throw new ArgumentNullException();

        _hashAlgorithm = hashAlgorithm;
    }

    public string Hash(string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException();


        _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(message));
        return Convert.ToBase64String(_hashAlgorithm.Hash);
    }
}

public class MD5Hashing : Hashing
{
    public MD5Hashing() : base(MD5.Create()) { }
}

public class SHA1Hashing : Hashing
{
    public SHA1Hashing() : base(SHA1.Create()) { }
}