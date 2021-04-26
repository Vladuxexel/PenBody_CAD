using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PenBody_Cad.UnitTests
{
	/// <summary>
	/// Класс для нагрузочного тестирования.
	/// </summary>
    [TestFixture]
	public class StressTest
	{
		[TestCase(TestName = 
			"Нагрузочный тест потребления памяти и времени построения")]
		public void Start()
		{
			var writer = new StreamWriter(
				$@"{AppDomain.CurrentDomain.BaseDirectory}\StressTest.txt");

			var count = 200;

			var processes = Process.GetProcessesByName("kStudy");
			var process = processes.First();

			var ramCounter = new PerformanceCounter("Process", "Working Set - Private", process.ProcessName);
			var cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
			var stopwatch = new Stopwatch();

			for (int i = 0; i < count; i++)
			{
				stopwatch.Start();

				cpuCounter.NextValue();
				var parameters = new PenBodyParametersList();
				var builder = new PenBodyBuilder();
				builder.Build(parameters);

				stopwatch.Stop();

				var ram = ramCounter.NextValue();
				var cpu = cpuCounter.NextValue();

				writer.Write($"{i}. ");
				writer.Write($"RAM: {Math.Round(ram / 1024 / 1024)} MB");
				writer.Write($"\tCPU: {cpu} %");
				writer.Write($"\ttime: {stopwatch.Elapsed}");
				writer.Write(Environment.NewLine);
				writer.Flush();

				stopwatch.Reset();
			}
		}
	}
}
