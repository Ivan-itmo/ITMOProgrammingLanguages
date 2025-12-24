namespace PL_Lab5;

public class PagePayload
{
    public string UserData { get; set; }
    public string OrderData { get; set; }
    public string AdData { get; set; }

    public override string ToString()
    {
        return
            "--- Агрегированный результат ---\n" +
            $"Пользователь: {UserData}\n" +
            $"Заказы: {OrderData}\n" +
            $"Реклама: {AdData}\n" +
            "-------------------------------";
    }
}