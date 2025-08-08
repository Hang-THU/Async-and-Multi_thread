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

    public async Task UpdateInventoryAsync(string title, int quantity)
    {
        await Task.Delay(100); // 模拟网络延迟

        // TODO: 使用 lock 语句保证线程安全
        // 提示：在 lock 块中查找书籍并更新库存，若库存不足则输出提示
        var book = _books.Find(x => x.Title == title);
        lock (_lock)
        {
            if (book is not null && book.Inventory + quantity >= 0)
            {
                book.Inventory += quantity;
            }
            else
            {
                Console.WriteLine($"《{title}》不存在或库存不足。");
            }

        }
    }
}

public class BookStore
{
    private readonly Database _db = new Database();

    // TODO: 实现异步购书方法CheckoutAsync，调用 UpdateInventoryAsync
    public async Task CheckoutAsync(string bookTitle, int quantity)
    {
        await _db.UpdateInventoryAsync(bookTitle, -quantity);
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

        // TODO: 使用 Task.WhenAll 模拟多个用户并发购书
        // 提示：创建多个 Task 调用 CheckoutAsync，并传入不同书名和数量
        var tasks = new List<Task>
        {
            CheckoutAsync("C#入门", 2),
            CheckoutAsync("C#入门", 3),
            CheckoutAsync("异步编程", 1),
            CheckoutAsync("异步编程", 2),
            CheckoutAsync("异步编程", 3),
        };
        //await Task.WhenAll(tasks.ToArray());
        Task.WaitAll(tasks.ToArray());


        Console.WriteLine("\n购买后库存：");
        books = await _db.GetBooksAsync();
        foreach (var book in books)
        {
            Console.WriteLine($"- {book.Title}：{book.Inventory} 本");
        }
    }
}

public class Program
{
    public static async Task Main()
    {
        var store = new BookStore();
        await store.SimulateMultipleUsers();
    }
}
