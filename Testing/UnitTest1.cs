using _19033684_Kumar_Pulami.Models.ViewModel.Exam;
using NUnit.Framework;

namespace Testing
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            float totalmarks = 100;
            float obtainedmarks = 90;
            SubjectGradeAndGradePointAndRemarksModel model = new SubjectGradeAndGradePointAndRemarksModel();
            model.Grade = "A+";
            model.GradePoint = 4.0F;
            model.Remarks = "";

        }


        [Test]
        public void Test1()
        {
           
        }
    }
}