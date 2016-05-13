using System.Collections.Generic;
using System.Linq;
using Metropolis.Common.Models;

namespace Metropolis.Api.Services.Tasks.Commands
{
    public class CompositeMetricsCommand : IMetricsCommand
    {
        private readonly IEnumerable<IMetricsCommand> commands;
        private readonly bool runParallel;

        public CompositeMetricsCommand(IEnumerable<IMetricsCommand> commands, bool runParallel = true)
        {
            this.commands = commands;
            this.runParallel = runParallel;
        }

        public IEnumerable<MetricsResult> Run(MetricsCommandArguments args)
        {
            return runParallel ? RunInParallel(args) : commands.SelectMany(each => each.Run(args));
        }

        private IEnumerable<MetricsResult> RunInParallel(MetricsCommandArguments args)
        {
            var results = new List<MetricsResult>();
            commands.AsParallel().ForAll(x => results.AddRange(x.Run(args)));
            return results;
        }
    }
}