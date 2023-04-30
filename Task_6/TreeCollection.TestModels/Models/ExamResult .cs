using System;
using TreeCollection.TestModels.Enums;

namespace TreeCollection.TestModels.Models
{
    public class ExamResult : IComparable<ExamResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Exams Exam { get; set; }
        public Score Score { get; set; }
        public DateTime Date { get; set; }

        public ExamResult(int id, string name, Exams exam, Score score, DateTime date)
        {
            Id = id;
            Name = name;
            Exam = exam;
            Score = score;
            Date = date;
        }

        public int CompareTo(ExamResult? examResult)
        {
            int result;
            if (examResult is null) throw new ArgumentException("Impossible to compare objects");
            else
            {
                result = Name.CompareTo(examResult.Name);
                if (result == 0) 
                {
                    result = Date.CompareTo(examResult.Date);
                    if (result == 0)
                    {
                        return Id.CompareTo(examResult.Id);
                    }
                }
            }
            return result; 
        }
    }
}
