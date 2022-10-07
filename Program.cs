using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;
using MathNet.Numerics.LinearAlgebra;

public partial class MyClass
{
    [JSExport]
    internal static void DoMath(int n)
    {
        SetText("Running...");

        var matrixA = Matrix<double>.Build.Random(n,n);
        var vectorB = Vector<double>.Build.Random(n);

        var start = DateTimeOffset.Now;

        var x = matrixA.Solve(vectorB); // Solve Ax = b

        var end = DateTimeOffset.Now;

        var time = (end - start).TotalMilliseconds;

        SetText($"{x}\nTime taken: {time} ms");
    }



    [JSImport("MyClass.updateTick", "main.js")]
    static partial void UpdateTick(string message);

    [JSImport("MyClass.setText", "main.js")]
    static partial void SetText(string message);

    [JSExport]
    [return:JSMarshalAs<JSType.Promise<JSType.Void>>]
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
