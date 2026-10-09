using Infrastructure.Logging;
using Xunit;

namespace ARQ_lim.Tests.Infrastructure.Logging;

public class LoggerTests
{
    [Fact]
    public void Log_WhenEnabled_WritesMessageToConsole()
    {
        var originalOutput = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.SetOut(output);
            Logger.Enabled = true;

            Logger.Log("Prueba");

            Assert.Contains("[LOG]", output.ToString());
            Assert.Contains("Prueba", output.ToString());
        }
        finally
        {
            Console.SetOut(originalOutput);
            Logger.Enabled = true;
            output.Dispose();
        }
    }

    [Fact]
    public void Log_WhenDisabled_DoesNotWriteToConsole()
    {
        var originalOutput = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.SetOut(output);
            Logger.Enabled = false;

            Logger.Log("Prueba");

            Assert.Equal(string.Empty, output.ToString());
        }
        finally
        {
            Console.SetOut(originalOutput);
            Logger.Enabled = true;
            output.Dispose();
        }
    }

}