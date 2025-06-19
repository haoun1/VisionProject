using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisionProject.Domain
{
    public class ResourceMonitor
    {
        // 결과 구조체
        public class CpuUsageResult
        {
            public long ElapsedMilliseconds { get; set; }
            public float AverageCpuUsage { get; set; }
        }

        // 측정 함수
        public static async Task<CpuUsageResult> MeasureCpuDuringAsync(Func<Task> taskToRun)
        {
            var cpuUsages = new List<float>();
            var cancellation = new CancellationTokenSource();
            var sw = Stopwatch.StartNew();

            string instanceName = GetProcessInstanceName(Process.GetCurrentProcess().Id);
            var cpuCounter = new PerformanceCounter("Process", "% Processor Time", instanceName, true);

            cpuCounter.NextValue(); // 초기화
            await Task.Delay(100);

            // 백그라운드 측정 시작
            var monitorTask = Task.Run(async () =>
            {
                while (!cancellation.Token.IsCancellationRequested)
                {
                    float usage = cpuCounter.NextValue() / Environment.ProcessorCount;
                    cpuUsages.Add(usage);
                    await Task.Delay(100);
                }
            });

            // 작업 실행
            await taskToRun();

            // 종료
            sw.Stop();
            cancellation.Cancel();
            await monitorTask;

            float avg = cpuUsages.Count > 0 ? cpuUsages.Average() : 0;
            return new CpuUsageResult
            {
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                AverageCpuUsage = avg
            };
        }

        // 프로세스 인스턴스 이름 얻기
        static string GetProcessInstanceName(int pid)
        {
            var category = new PerformanceCounterCategory("Process");
            foreach (var name in category.GetInstanceNames())
            {
                using (var counter = new PerformanceCounter("Process", "ID Process", name, true))
                {
                    if ((int)counter.RawValue == pid)
                        return name;
                }
            }
            return null;
        }
    }
}
