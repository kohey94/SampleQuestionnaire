using System.Text.Json.Serialization;

namespace SampleQuestionnaire.Models
{
    public class QuestionnaireResult
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("question1")]
        public Question1 Question1 { get; set; }
        [JsonPropertyName("question2")]
        public Question2 Question2 { get; set; }
        [JsonPropertyName("question3")]
        public Question3 Question3 { get; set; }
        [JsonPropertyName("question4")]
        public Question3 Question4 { get; set; }
        [JsonPropertyName("question5")]
        public Question1 Question5 { get; set; }
    }

    public class Question1
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }
        [JsonPropertyName("answer")]
        public Answer1[] Answer { get; set; }
    }

    public class Question2
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }
        [JsonPropertyName("answer")]
        public Answer2[] Answer { get; set; }
    }

    public class Question3
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }
        [JsonPropertyName("answer")]
        public string[] Answer { get; set; }
    }

    public class Answer1
    {
        [JsonPropertyName("votes")]
        public int Votes { get; set; }
        [JsonPropertyName("answer")]
        public string Answer { get; set; }
    }

    public class Answer2
    {
        [JsonPropertyName("reason")]
        public string[] Reason { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
