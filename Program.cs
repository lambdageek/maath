using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;
using MathNet.Numerics.LinearAlgebra;

public partial class MyClass
{
    [JSExport]
    internal static async Task DoMath(int n)
    {
        SetText("Running...");

        var matrixA = Matrix<double>.Build.Random(n,n);
        var vectorB = Vector<double>.Build.Random(n);

        TaskCompletionSource<TimedResult> tcs = new();

        var thread = new Thread(() => {
                TimedResult result = TimeIt(() => matrixA.Solve(vectorB)); // Solve Ax = b
                tcs.SetResult(result);
        });
        thread.Start();

        var result = await tcs.Task;

        SetText($"{result.Result}\nTime taken: {result.Time} ms");
    }

    struct TimedResult
    {
        public double Time;
        public Vector<double> Result;
    }

    static TimedResult TimeIt(Func<Vector<double>> f)
    {
        var start = DateTimeOffset.Now;
        var result = f();
        var end = DateTimeOffset.Now;
        var time = (end - start).TotalMilliseconds;
        return new TimedResult { Time = time, Result = result };
    }

    [JSImport("MyClass.updateTick", "main.js")]
    static partial void UpdateTick(string message);

    [JSImport("MyClass.setText", "main.js")]
    static partial void SetText(string message);

    [JSExport]
    public static async Task Tick()
    {
        string[] pix = {"🅰", "🅱", "🅲", "🅳", "🅴", "🅵", "🅶", "🅷", "🅸", "🅹", "🅺", "🅻", "🅼", "🅽", "🅾", "🅿︎", "🆀", "🆁", "🆂", "🆃", "🆄", "🆅", "🆆", "🆇", "🆈", "🆉"};
        int i = 0;;
        while (true) {
            UpdateTick(pix[i]);
            if (++i >= pix.Length) {
                i = 0;
            }
            await Task.Delay (500);
        }
    }

    public static void Main() {}

}
