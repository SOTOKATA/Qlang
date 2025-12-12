using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Qlang.Core.Lang.Interpreter.Native;
using Qlang.NativeLib;

public class UISupportLib : IQlangLib
{
    public string Name { get; } = "UIAvalonia";
    public string Version { get; } = "0.0.1";
    
    private static bool _isInitialized = false;
    private static Thread _uiThread;

    public void Register(NativeFunctionRegistry registry)
    {
        // Инициализация Avalonia
        registry.RegisterPublic("ui", "init", (Action)InitializeAvalonia);
        
        // Создание окна
        registry.RegisterPublic("ui", "createWindow", (Func<string, int, int, object>)CreateWindow);
        
        // Показать окно
        registry.RegisterPublic("ui", "showWindow", (Action<object>)ShowWindow);
        
        // Добавить кнопку
        registry.RegisterPublic("ui", "addButton", (Action<object, string, int, int>)AddButton);
        
        // Запустить UI цикл
        registry.RegisterPublic("ui", "run", (Action)RunUI);
    }

    private void InitializeAvalonia()
    {
        if (_isInitialized) return;
        
        _uiThread = new Thread(() =>
        {
            AppBuilder.Configure<Application>()
                .UsePlatformDetect()
                .SetupWithoutStarting();
        });
        
        _uiThread.SetApartmentState(ApartmentState.STA);
        _uiThread.Start();
        _uiThread.Join();
        
        _isInitialized = true;
    }

    private object CreateWindow(string title, int width, int height)
    {
        Window window = null;
        
        Dispatcher.UIThread.Post(() =>
        {
            window = new Window
            {
                Title = title,
                Width = width,
                Height = height,
                Content = new StackPanel()
            };
        });
        
        Thread.Sleep(100); // Небольшая задержка для создания окна
        return window;
    }

    private void ShowWindow(object windowObj)
    {
        if (windowObj is Window window)
        {
            Dispatcher.UIThread.Post(() => window.Show());
        }
    }

    private void AddButton(object windowObj, string text, int x, int y)
    {
        if (windowObj is Window window)
        {
            Dispatcher.UIThread.Post(() =>
            {
                var button = new Button
                {
                    Content = text,
                    Margin = new Thickness(x, y, 0, 0)
                };
                
                button.Click += (s, e) => Console.WriteLine($"Button '{text}' clicked!");
                
                if (window.Content is StackPanel panel)
                {
                    panel.Children.Add(button);
                }
            });
        }
    }

    private void RunUI()
    {
        var app = new Application();
        app.Run(CancellationToken.None);
    }
}