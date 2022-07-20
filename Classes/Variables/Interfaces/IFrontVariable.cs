namespace shufflecad_4.Classes.Variables.Interfaces
{
    public interface IFrontVariable
    {
        string Name { get; set; }
        string Type { get; set; }
        string Value { get; set; }
        string Direction { get; set; }

        bool GetBool();
        float GetFloat();
        string GetString();
        void SetBool(bool value);
        void SetFloat(float value);
        void SetString(string value);
    }
}
