# 《异步与多线程》课程作业
请fork此仓库，根据提示将仓库中homework.cs文件代码补充完整，并提pull request.
作业具体说明：你需要实现一个支持多用户并发购书的异步书店系统，在标有`// TODO:`注释的地方填入适当代码,满足以下要求：
- `UpdateInventoryAsync`方法必须使用 lock 保证库存更新的线程安全.
- `CheckoutAsync`方法要异步调用`UpdateInventoryAsync`.
- `SimulateMultipleUsers`方法使用`Task.WhenAll`模拟多个用户并发购书。

模拟用户购书清单如下：

- 用户1：C#入门 x2
- 用户2：C#入门 x3
- 用户3：异步编程 x1
- 用户4：异步编程 x2
- 用户5：异步编程 x3

## 说明：如何通过 Visual Studio 2022 运行此代码

新建一个C#控制台项目，删除原有的`Program.cs`源文件，并将本目录的`homework.cs`文件添加到项目中，然后编译并运行。
