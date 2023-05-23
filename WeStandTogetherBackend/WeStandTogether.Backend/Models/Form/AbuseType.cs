namespace WeStandTogether.Backend.Models.Form;

public class AbuseType
{
    public double controllingBehaviour { get; set; }
    public double forcingSubmissiveBehaviour { get; set; }
    public double imposingIdeasOrBehaviour { get; set; }
    public double techAbuse { get; set; }
    public double jealousy { get; set; }
    public double abuseByIsolation { get; set; }
    public double onlineHarassment { get; set; }
    public double psychologicalAbuse { get; set; }
    public double physicalAbuse { get; set; }
    public double verbalAbuse { get; set; }
    public double financialAbuse { get; set; }

    public AbuseType()
    {
        controllingBehaviour = 0;
        forcingSubmissiveBehaviour = 0;
        imposingIdeasOrBehaviour = 0;
        techAbuse = 0;
        jealousy = 0;
        abuseByIsolation = 0;
        onlineHarassment = 0;
        psychologicalAbuse = 0;
        physicalAbuse = 0;
        verbalAbuse = 0;
        financialAbuse = 0;
    }

    public void Add(AbuseType abuseType)
    {
        controllingBehaviour += abuseType.controllingBehaviour;
        forcingSubmissiveBehaviour += abuseType.forcingSubmissiveBehaviour;
        imposingIdeasOrBehaviour += abuseType.imposingIdeasOrBehaviour;
        techAbuse += abuseType.techAbuse;
        jealousy += abuseType.jealousy;
        abuseByIsolation += abuseType.abuseByIsolation;
        onlineHarassment += abuseType.onlineHarassment;
        psychologicalAbuse += abuseType.psychologicalAbuse;
        physicalAbuse += abuseType.physicalAbuse;
        verbalAbuse += abuseType.verbalAbuse;
        financialAbuse += abuseType.financialAbuse;
    }

    public double GetSum()
    {
        return controllingBehaviour + forcingSubmissiveBehaviour + imposingIdeasOrBehaviour + techAbuse + jealousy +
               abuseByIsolation + onlineHarassment + psychologicalAbuse + physicalAbuse + verbalAbuse + financialAbuse;
    }

    public AbuseType GetPercentage(double max)
    {
        var abuseType = new AbuseType();
        abuseType.controllingBehaviour /= max;
        abuseType.forcingSubmissiveBehaviour /= max;
        abuseType.imposingIdeasOrBehaviour /= max;
        abuseType.techAbuse /= max;
        abuseType.jealousy /= max;
        abuseType.abuseByIsolation /= max;
        abuseType.onlineHarassment /= max;
        abuseType.psychologicalAbuse /= max;
        abuseType.physicalAbuse /= max;
        abuseType.verbalAbuse /= max;
        abuseType.financialAbuse /= max;

        return abuseType;
    }
}