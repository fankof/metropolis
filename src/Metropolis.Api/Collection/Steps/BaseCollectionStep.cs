using System;
using System.Collections.Generic;
using System.IO;
using Metropolis.Api.Collection.PowerShell;
using Metropolis.Common.Models;

namespace Metropolis.Api.Collection.Steps
{
    public abstract class BaseCollectionStep : ICollectionStep
    {
        private readonly IRunPowerShell powerShell;

        protected BaseCollectionStep(IRunPowerShell powerShell)
        {
            this.powerShell = powerShell;
        }

        public abstract string MetricsType { get; }
        public abstract string Extension { get; }
        public abstract ParseType ParseType { get; }

        public IEnumerable<MetricsResult> Run(MetricsCommandArguments args)
        {
            var result = MetricResultFor(args);
            var command = PrepareCommand(args, result);
            SaveAndExecuteCommand(args, command);
            return new[] {result};
        }

        public abstract string PrepareCommand(MetricsCommandArguments args, MetricsResult result);

        private void SaveAndExecuteCommand(MetricsCommandArguments args, string command)
        {
            try
            {
                SaveMetricsCommand(args, command);
                InvokeCommand(command);
            }
            catch (Exception e)
            {
                //TODO: log this exception somewhere fancy 
                throw new ApplicationException("Error occurred trying to exeucte an external process", e);
            }
        }

        private void SaveMetricsCommand(MetricsCommandArguments args, string cmd)
        {
            var fileName = Path.Combine(args.MetricsOutputDirectory, $"{args.ProjectName}_{MetricsType}_command.ps1");
            File.WriteAllText(fileName, cmd);
        }

        protected MetricsResult MetricResultFor(MetricsCommandArguments args)
        {
            return new MetricsResult {ParseType = ParseType, MetricsFile = GetOutputFile(args)};
        }

        protected string GetOutputFile(MetricsCommandArguments args)
        {
            var fileName = $"{args.ProjectName}_{MetricsType}{Extension}".Replace(' ', '_');
            return Path.Combine(args.MetricsOutputDirectory, fileName);
        }

        protected void InvokeCommand(string command)
        {
            powerShell.Invoke(command);
        }

        public static string LocateBinaries(string target)
        {
            return Locate(@"Collection\Binaries\", target);
        }

        public static string LocateSettings(string target)
        {
            return Locate(@"Collection\Settings\", target);
        }

        private static string Locate(string collectionPath, string target)
        {
            return Path.Combine(Environment.CurrentDirectory, collectionPath, target);
        }

        public static string GetNodeBinPath()
        {
#if DEBUG
            return @"..\..\..\..\node_modules\.bin\";
#else
            return @"..\node_modules\.bin\";
#endif
        }
    }
}