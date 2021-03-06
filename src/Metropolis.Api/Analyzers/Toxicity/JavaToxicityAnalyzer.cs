﻿using Metropolis.Api.Domain;

namespace Metropolis.Api.Analyzers.Toxicity
{
    /// <summary>
    /// Adapted from Erik Doernenberg's 2008 toxicity reloaded article 
    /// http://erik.doernenburg.com/2008/11/how-toxic-is-your-code/
    /// </summary>
    public class JavaToxicityAnalyzer : ToxicityAnalyzer
    {
        //class level thresholds
        public const int ThresholdLinesOfCode = 500;
        public const int ThresholdNumberOfMethods = 20;
        public const int ThresholdAnonymousInnerClassLength = 35;
        public const int ThresholdClassDataAbstractionCoupling = 10;
        public const int ThresholdClassFanOutComplexity = 30;

        // method level thresholds
        public const int ThresholdMethodLength = 30;
        public const int ThresholdCyclomaticComplexity = 15;
        public const int ThresholdDefaultCase = 0; //not acceptable to miss a default: - could cause unexpected bug
        public const int ThresholdBooleanComplexity = 3;
        public const int ThresholdNestedIfDepth = 3;
        public const int ThresholdNestedTryDepth = 2;
        public const int ThresholdParameterNumber = 6;


        public override ToxicityScore CalculateToxicity(Instance instanceToScore)
        {
            // Class Level Toxicity
            var linesOfCode = ComputeToxicity(instanceToScore.LinesOfCode, ThresholdLinesOfCode);
            var numberOfMethods = ComputeToxicity(instanceToScore.Members.Count, ThresholdNumberOfMethods);
            var innerClassAnonymous = ComputeToxicity(instanceToScore.AnonymousInnerClassLength, ThresholdAnonymousInnerClassLength);
            var classDataAbstractionCoupling = ComputeToxicity(instanceToScore.ClassDataAbstractionCoupling, ThresholdClassDataAbstractionCoupling);
            var classFanOutComplexity = ComputeToxicity(instanceToScore.ClassFanOutComplexity, ThresholdClassFanOutComplexity);

            // Method Level Toxicity
            double cyclomaticComplexity = 0;
            double methodLength = 0;
            double missingDefaultCase = 0;
            double booleanComplexity = 0;
            double nestedIfDepth = 0;
            double nestedTryDepth = 0;
            double parameterNumber = 0;

            foreach (var method in instanceToScore.Members)
            {
                cyclomaticComplexity += ComputeToxicity(method.CylomaticComplexity, ThresholdCyclomaticComplexity);
                methodLength += ComputeToxicity(method.LinesOfCode, ThresholdMethodLength);
                missingDefaultCase += ComputeToxicity(method.MissingDefaultCase, ThresholdDefaultCase);
                booleanComplexity += ComputeToxicity(method.BooleanExpressionComplexity, ThresholdBooleanComplexity);
                nestedIfDepth += ComputeToxicity(method.NestedIfDepth, ThresholdNestedIfDepth);
                nestedTryDepth += ComputeToxicity(method.NestedTryDepth, ThresholdNestedTryDepth);
                parameterNumber += ComputeToxicity(method.NumberOfParameters, ThresholdParameterNumber);
            }

            // Rationalize
            var score = new ToxicityScore
            {
                // class level
                LinesOfCode = Rationalize(linesOfCode),
                NumberOfMethods = Rationalize(numberOfMethods),
                AnonInnerLength = Rationalize(innerClassAnonymous),
                ClassDataAbstractionCoupling = Rationalize(classDataAbstractionCoupling),
                ClassFanOutComplexity = Rationalize(classFanOutComplexity),

                // method level
                MethodLength = Rationalize(methodLength),
                CyclomaticComplexity = Rationalize(cyclomaticComplexity),
                MissingDefaultCase = Rationalize(missingDefaultCase),
                BooleanExpressionComplexity = Rationalize(booleanComplexity),
                NestedIfDepth = Rationalize(nestedIfDepth),
                NestedTryDepth = Rationalize(nestedTryDepth),
                NumberOfParameters = Rationalize(parameterNumber)
            };

            score.Toxicity = score.LinesOfCode + score.NumberOfMethods + 
                score.ClassFanOutComplexity + score.ClassDataAbstractionCoupling +
                score.AnonInnerLength + score.MethodLength + score.CyclomaticComplexity +
                score.MissingDefaultCase + score.BooleanExpressionComplexity +
                score.NestedIfDepth + score.NestedTryDepth + score.NumberOfParameters;

            return score;
        }
    }
}
