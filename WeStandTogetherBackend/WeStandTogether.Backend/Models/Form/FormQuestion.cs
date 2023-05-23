namespace WeStandTogether.Backend.Models.Form;

public record FormQuestion(int id, string question, Answer[] answers, NextQuestionId[] nextQuestionId,AbuseType abuseType);