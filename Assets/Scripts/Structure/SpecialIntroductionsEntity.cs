using System.Collections.Generic;

[System.Serializable]
public class SpecialIntroductionsItem
{
    public string name;

    public string introTitle;

    public string introText;

    public string effectText;
}

[System.Serializable]
public class SpecialIntroductionsEntity
{
    public List <SpecialIntroductionsItem> SpecialIntroductions;
}
