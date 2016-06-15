﻿using System;
using System.IO;
using Metropolis.Api.Analyzers;
using Metropolis.Api.Collection;
using Metropolis.Api.Collection.Steps.CSharp;
using Metropolis.Api.Domain;
using Metropolis.Api.Utilities;
using Metropolis.Common.Extensions;
using Metropolis.Common.Models;

namespace Metropolis.Api.Services
{
    public class AnalysisServices : IAnalysisService
    {
        private readonly ICollectionStepFactory collectionStepFactory;
        private readonly IAnalyzerFactory analyzerFactory;
        private readonly IFileSystem fileSystem;
        private readonly ICodebaseService codebaseService;
        private readonly IBuildEnvironment fxCopMetricsTool;
        
        public static string MetricsOutputFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Metropolis.Metrics");
        public string FxCopMetricsPath => fxCopMetricsTool.FxCopMetricsToolPath;

        string IAnalysisService.MetricsOutputFolder => MetricsOutputFolder;

        public AnalysisServices() : this(new CollectionStepFactory(), new CodebaseService(), new AnalyzerFactory(), new FileSystem(), new BuildEnvironment())
        {
        }

        public AnalysisServices(ICollectionStepFactory collectionStepFactory, ICodebaseService codebaseService, IAnalyzerFactory analyzerFactory, IFileSystem fileSystem, IBuildEnvironment fxCopMetricsTool)
        {
            this.collectionStepFactory = collectionStepFactory;
            this.codebaseService = codebaseService;
            this.analyzerFactory = analyzerFactory;
            this.fileSystem = fileSystem;
            this.fxCopMetricsTool = fxCopMetricsTool;
            fileSystem.CreateFolder(MetricsOutputFolder);
        }

        public CodeBase Analyze(MetricsCommandArguments details)
        {
            var command = collectionStepFactory.GetStep(details.RepositorySourceType);
            details.MetricsOutputFolder = MetricsOutputFolder;
            var metricsResults = command.Run(details);

            var codeBase = CodeBase.Empty();
            foreach (var x in metricsResults)
            {
                string filename = x.MetricsFile;
                var cb = codebaseService.Get(fileSystem.OpenFileStream(filename), x.ParseType);
                codeBase.Enrich(new CodeGraph(cb.AllInstances));
            }

            var codebase = analyzerFactory.For(details.RepositorySourceType).Analyze(codeBase.AllInstances);
            codebase.SourceType = details.RepositorySourceType;
            codebase.Name = details.ProjectName;

            return codebase;
        }
    }
}
