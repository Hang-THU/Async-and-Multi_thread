using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// Book 类定义了一个书籍对象，包含书名、价格和库存属性
public class Book
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public int Inventory { get; set; }
}

// Database 类模拟了数据库操作，提供获取书籍列表和更新库存的功能
public class Database
{
    // _books 是存储书籍信息的列表，作为共享资源
    private readonly List<Book> _books = new List<Book>();
    // _lock 是一个私有的对象，用作 lock 语句的锁对象，以保护 _books 列表的线程安全
    private readonly object _lock = new object();

    // 构造函数，初始化书籍列表
    public Database()
    {
        _books.Add(new Book { Title = "C#入门", Price = 39.9m, Inventory = 10 });
        _books.Add(new Book { Title = "异步编程", Price = 59.9m, Inventory = 5 });
    }

    // GetBooksAsync 异步获取书籍列表。
    public Task<List<Book>> GetBooksAsync()
    {
        // Task.FromResult 方法立即返回一个已完成的 Task，其中包含指定的值。
        // 这里的任务是同步的，但封装成异步方法，以便与异步调用保持一致。
        return Task.FromResult(_books);
    }

    // UpdateInventoryAsync 异步更新书籍库存。
    public async Task UpdateInventoryAsync(string title, int quantity)
    {
        // 使用 await Task.Delay(100) 模拟网络或IO延迟，这是一个非阻塞的异步操作。
        await Task.Delay(100); // 模拟网络延迟

        // TODO: 使用 lock 语句保证线程安全
        // 提示：在 lock 块中查找书籍并更新库存，若库存不足则输出提示
        // lock 语句是 Monitor.Enter 和 Monitor.Exit 的语法糖
        // 它确保同一时间只有一个线程可以进入此代码块，从而保护共享资源 _books
        lock (_lock)
        {
            var book = _books.Find(b => b.Title == title);
            if (book != null)
            {
                if (book.Inventory >= quantity)
                {
                    book.Inventory -= quantity;
                }
                else
                {
                    Console.WriteLine($"书籍《{title}》库存不足");
                }
            }
        }
    }
}

// BookStore 类模拟了书店的业务逻辑，包括结账和模拟多用户操作
public class BookStore
{
    private readonly Database _db = new Database();

    // TODO: 实现异步购书方法CheckoutAsync，调用 UpdateInventoryAsync
    // 这是个异步方法，使用 async 关键字标记
    public async Task CheckoutAsync(string bookTitle, int quantity)
    {
        // 调用 UpdateInventoryAsync 方法并使用 await 等待其完成，await 不会阻塞线程
        await _db.UpdateInventoryAsync(bookTitle, quantity);
    }

    // SimulateMultipleUsers 模拟多个用户并发购书
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
            CheckoutAsync("C#入门", 2),    // 用户1：C#入门 x2
            CheckoutAsync("C#入门", 3),    // 用户2：C#入门 x3
            CheckoutAsync("异步编程", 1),  // 用户3：异步编程 x1
            CheckoutAsync("异步编程", 2),  // 用户4：异步编程 x2
            CheckoutAsync("异步编程", 3)   // 用户5：异步编程 x3
        };

        // Task.WhenAll 等待所有给定的任务完成，模拟所有用户并发进行购买操作
        await Task.WhenAll(tasks);

        Console.WriteLine("\n购买后库存：");
        books = await _db.GetBooksAsync();
        foreach (var book in books)
        {
            Console.WriteLine($"- {book.Title}：{book.Inventory} 本");
        }
    }
}

// Program 类是程序的入口点
public class Program
{
    // Main 方法被标记为 async Task，使其能够使用 await 关键字
    public static async Task Main()
    {
        var store = new BookStore();
        await store.SimulateMultipleUsers();
    }
}