namespace Argo_Core.Shared.Core.System.Graphics.UIText;

public class Kerning
{

    #region Public Constructors

    public Kerning(char firstCharacter, char secondCharacter, int amount)
    {
        FirstCharacter = firstCharacter;
        SecondCharacter = secondCharacter;
        Amount = amount;
    }

    #endregion Public Constructors

    #region Public Methods

    public override string ToString()
    {
        return $"{FirstCharacter} to {SecondCharacter} = {Amount}";
    }

    #endregion Public Methods

    #region Public Properties

    public char FirstCharacter { get; set; }

    public char SecondCharacter { get; set; }

    public int Amount { get; set; }

    #endregion Public Properties

}