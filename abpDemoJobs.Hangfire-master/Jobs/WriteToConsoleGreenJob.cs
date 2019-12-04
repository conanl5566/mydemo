using System;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace demo3 {
    public class WriteToConsoleGreenJob : BackgroundJob<WriteToConsoleGreenJobArgs>, ITransientDependency {
        public override void Execute (WriteToConsoleGreenJobArgs args) {
            if (RandomHelper.GetRandom (0, 100) < 70) {
                //throw new ApplicationException("A sample exception from the WriteToConsoleGreenJob!");
            }

            lock (Console.Out) {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine ();
                Console.WriteLine ($"############### WriteToConsoleGreenJob: {args.Value} - {args.Time:HH:mm:ss} ###############");
                Console.WriteLine ();

                Console.ForegroundColor = oldColor;
            }
        }
    }
}