
using System;

public class MyEvent
{
    public event EventHandler<string> PrintEvent;
    public void Print(string message)
    {
        PrintEvent?.Invoke(this, message);
    }
}
// 使用示例
public class Program
{
    public static void Main()
    {
        MyEvent myEvent = new MyEvent();
        myEvent.PrintEvent += (sender, e) => Console.WriteLine(e);
        myEvent.Print("Hello World!");
    }
}