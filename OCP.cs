using System;

public class Program
{
    public static void Main(string[] args)
    {
        var orderForm = new OrderForm();
        var paymentHandler = new PaymentHandler();
        IPaymentSystem paymentSystem = PaymentSystemFactory.GetPaymentSystem(orderForm.ShowForm());
        paymentSystem.Connect();
        paymentHandler.ShowPaymentResult(paymentSystem);
    }
}

public class OrderForm
{
    public string ShowForm()
    {
        Console.WriteLine("Мы принимаем: QIWI, WebMoney, Card");
        Console.WriteLine("Какое системой вы хотите совершить оплату?");
        return Console.ReadLine();
    }
}

public class PaymentHandler
{
    public void ShowPaymentResult(IPaymentSystem paymentSystem)
    {
        Console.WriteLine($"Вы оплатили с помощью {paymentSystem.ID}");
        Console.WriteLine($"Проверка платежа через {paymentSystem.ID}...");
        Console.WriteLine("Оплата прошла успешно!");
    }
}

public class PaymentSystemFactory
{
    public static IPaymentSystem GetPaymentSystem(string systemId)
    {
        if (systemId == nameof(QIWI))
            return new QIWI();
        else if (systemId == nameof(WebMoney))
            return new WebMoney();
        else if (systemId == nameof(Card))
            return new Card();

        return new InvalidOperationException();
    }
}

public interface IPaymentSystem
{
    string ID { get; }

    void Connect();
}

public class QIWI : IPaymentSystem
{
    public string ID { get; } = nameof(QIWI);

    public void Connect()
        => Console.WriteLine("Перевод на страницу QIWI...");
}

public class WebMoney : IPaymentSystem
{
    public string ID { get; } = nameof(WebMoney);

    public void Connect()
        => Console.WriteLine("Вызов API WebMoney...");
}

public class Card : IPaymentSystem
{
    public string ID { get; } = nameof(Card);

    public void Connect()
        => Console.WriteLine("Вызов API банка эмитера карты Card...");
}