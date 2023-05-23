using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using WeStandTogether.Backend.Models.Form;

namespace WeStandTogether.Backend.Controllers
{
    [ApiController]
    public class FormController : ControllerBase
    {
        [HttpGet("form")]
        public IActionResult GetForm()
        {
            return Ok(System.IO.File.ReadAllText("Form.json"));
        }

        [HttpGet("form/result")]
        public IActionResult GetFormResult([FromBody] FormAnswers formAnswers)
        {
            Console.WriteLine("JI");
            return Ok(GetResult(formAnswers));
        }

        private string GetResult(FormAnswers formAnswers)
        {
            var form = JsonSerializer.Deserialize<FormQuestions>(System.IO.File.ReadAllText("Form.json")).form.ToList();
            var abuseType = new AbuseType();
            foreach (var answer in formAnswers.Answers)
            {
                var question = form.Find(q => q.id == answer.QuestionId);
                if (question.answers.ToList().Find(answer1 => answer1.Id == answer.AnswerId).answer.Equals("yes"))
                {
                    abuseType.Add(question.abuseType);
                }
            }

            var maxAbuse = new AbuseType();
            foreach (var question in form)
            {
                maxAbuse.Add(question.abuseType);
            }

            var maxScore = maxAbuse.GetSum();

            var abuseTypePercentage = abuseType.GetPercentage(maxScore);

            return JsonSerializer.Serialize(abuseTypePercentage);
        }
    }
}