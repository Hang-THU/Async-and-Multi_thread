using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
public class Book
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public int Inventory { get; set; }
}
public class Database
{
    private readonly List<Book> _books = new List<Book>();
    private readonly object _lock = new object();

    public Database()
    {
        _books.Add(new Book { Title = "C#入门", Price = 39.9m, Inventory = 10 });
        _books.Add(new Book { Title = "异步编程", Price = 59.9m, Inventory = 5 });
    }

    public Task<List<Book>> GetBooksAsync()
    {
        return Task.FromResult(_books);
    }

    public async Task UpdateInventoryAsync(string title, int quantity)//更新书籍和库存信息，购买quantity本书
    {
        await Task.Delay(100); // 模拟网络延迟
        lock (_lock)
        {
            var book = _books.Find(b => b.Title == title);
            if (book != null)
            {
                if (book.Inventory >= quantity)
                {
                    book.Inventory -= quantity;
                    Console.WriteLine($"成功购买 {quantity} 本《{book.Title}》，剩余库存：{book.Inventory}");
                }
                else
                {
                    Console.WriteLine($"库存不足，无法购买《{book.Title}》");
                }
            }
            else
            {
                Console.WriteLine($"未找到书籍《{title}》");
            }
        }
    }
}
public class BookStore
{
    private readonly Database _db = new Database();
    public async Task CheckoutAsync(string bookTitle, int quantity)
    {
        Console.WriteLine($"正在购买《{bookTitle}》 {quantity} 本...");
        await _db.UpdateInventoryAsync(bookTitle, quantity);
    }
    public async Task SimulateMultipleUsers()
    {
        var books = await _db.GetBooksAsync();
        Console.WriteLine("当前书店库存：");
        foreach (var book in books)
        {
            Console.WriteLine($"- {book.Title}：{book.Inventory} 本");
        }
        Console.WriteLine("\n 开始模拟多用户购书...\n");
        Task t1 = CheckoutAsync("C#入门", 2);
        Task t2 = CheckoutAsync("C#入门", 3);
        Task t3 = CheckoutAsync("异步编程", 1);
        Task t4 = CheckoutAsync("异步编程", 2);
        Task t5 = CheckoutAsync("异步编程", 3);
        //创建多个 Task 调用 CheckoutAsync，并传入不同书名和数量
        var tasks = new List<Task> { t1, t2, t3, t4, t5 };
        await Task.WhenAll(tasks);
        Console.WriteLine("\n购买后库存：");
        books = await _db.GetBooksAsync();
        foreach (var book in books)
        {
            Console.WriteLine($"- {book.Title}：{book.Inventory} 本");
        }
    }
}public class Program
{
    public static async Task Main()
    {
        var store = new BookStore();
        await store.SimulateMultipleUsers();
    }
}
