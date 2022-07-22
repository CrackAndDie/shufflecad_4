using shufflecad_4.Classes.Variables.Interfaces;


namespace shufflecad_4.Classes.Variables
{
    public class DefaultVariable : IFrontVariable
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Direction { get; set; }

        public bool GetBool()
        {
            throw new System.NotImplementedException();
        }

        public float GetFloat()
        {
            throw new System.NotImplementedException();
        }

        public string GetString()
        {
            throw new System.NotImplementedException();
        }

        public void SetBool(bool value)
        {
            throw new System.NotImplementedException();
        }

        public void SetFloat(float value)
        {
            throw new System.NotImplementedException();
        }

        public void SetString(string value)
        {
            throw new System.NotImplementedException();
        }
    }
}
