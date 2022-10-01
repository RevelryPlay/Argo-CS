namespace Argo_Core.Shared.Core.System.Graphics.UIText;

public class Page
{

    #region Public Constructors

    public Page(int id, string fileName)
    {
        FileName = fileName;
        Id = id;
    }

    #endregion Public Constructors

    #region Public Methods

    public override string ToString()
    {
        return $"{Id} ({Path.GetFileName(FileName)})";
    }

    #endregion Public Methods

    #region Public Properties

    public string FileName { get; set; }

    public int Id { get; set; }

    #endregion Public Properties

}
