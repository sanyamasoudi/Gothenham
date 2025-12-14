namespace Development.Scripts.Dialogue
{
    [System.Serializable]
    public class Dialogue
    {
        public string[] sentences;
        public float delayBetweenSentences = 0.1f;
        public float delayBetweenCharacters = 0.2f;
    }
}