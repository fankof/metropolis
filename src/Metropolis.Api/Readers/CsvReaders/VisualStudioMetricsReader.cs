﻿using System.Collections.Generic;
using System.Linq;
using Metropolis.Api.Analyzers.Toxicity;
using Metropolis.Api.Domain;

namespace Metropolis.Api.Readers.CsvReaders
{
    public class VisualStudioMetricsReader : CsvInstanceReader<VisualStudioCsvLineItem, VisualStudioCsvLineItemClassMap>
    {
        public VisualStudioMetricsReader() : base(hasHeaderRecord: true)
        { }

        protected override CodeBase ParseLines(IEnumerable<VisualStudioCsvLineItem> lines)
        {
            var results = new List<Instance>();

            var visualStudioCsvLineItems = lines as VisualStudioCsvLineItem[] ?? lines.ToArray();
            var allTypes = visualStudioCsvLineItems.Where(x => x.Scope == "Type");
            
            foreach (var type in allTypes)
            {
                var methodsOfType = CollectMethods(visualStudioCsvLineItems, type).ToList();

                var classToAdd = new Instance(type.Namespace, type.Type, methodsOfType.Count, 
                    type.LinesOfCode, type.CyclomaticComplexity, type.DepthOfInheritance, type.ClassCoupling);

                classToAdd.AddMembers(methodsOfType.Select(x =>
                        new Member(x.Member, x.LinesOfCode, x.CyclomaticComplexity, x.ClassCoupling)));

                results.Add(classToAdd);
            }

            var toxicityAnalyzer = new CSharpToxicityAnalyzer();

            return toxicityAnalyzer.Analyze(results);
        }

        private static IEnumerable<VisualStudioCsvLineItem> CollectMethods(IEnumerable<VisualStudioCsvLineItem> lines, VisualStudioCsvLineItem type)
        {
            return lines.Where(x => x.Namespace == type.Namespace &&
                                    x.Type == type.Type && x.Scope == "Member");
        }
    }
}