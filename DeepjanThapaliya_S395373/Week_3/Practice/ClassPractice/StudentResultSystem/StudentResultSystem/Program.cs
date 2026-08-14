using StudentResultSystem.Interfaces;
using StudentResultSystem.Models;
using StudentResultSystem.ScoreCalculators;
using StudentResultSystem.services;

IScoreCalculator scoreCalculator1 = new ExamScoreCalculator();
IScoreCalculator scoreCalculator2 = new AssignmentScoreCalculator();

List<Assessment> assessments = new List<Assessment>();
assessments.Add(new ExamAssessment("Deepjan", 385m, scoreCalculator1));
assessments.Add(new AssignmentAssessment("DeepjanTH", 38m, scoreCalculator2));


foreach (Assessment assessment in assessments)
{
    GradePrintingService printingService = new GradePrintingService(assessment);
    printingService.printGrade();
}



















//IScoreCalculator scoreCalcultor = new AssignmentScoreCalculator();
//List<Assessment> assessments = new List<Assessment>();

//assessments.Add(new AssignmentAssessment("Deepjan", 38m, scoreCalcultor));

//foreach (Assessment assement in assessments)
//{
//    Console.WriteLine(assement.Score());
//}