﻿using System.Collections;
using System.Collections.Generic;
using Metropolis.Api.Domain;
using Metropolis.Common.Models;

namespace Metropolis.Api.Services
{
    public interface ICodebaseService
    {
        void Save(CodeBase workspace, string fileName);
        CodeBase Load(string projectFileName);
        CodeBase LoadDefault();
        CodeBase Get(string filename, ParseType parseType);
        CodeBase GetToxicity(string fileName);
        CodeBase GetVisualStudioMetrics(string fileName);
        ProjectBuildResult BuildSolution(ProjectBuildArguments buildArgs);
        string ProjectBuildFolder { get; }
        void WriteIgnoreFile(string projectName, string projectFolder, IEnumerable<FileDto> filesToIgnore);
    }
}
