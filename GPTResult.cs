public class GPTResult {
    public string Id { get; set; }
    public string Created { get; set; } 
    public string Model { get; set; }
    public Choices[] Choices { get; set; }
}

public class Choices {
    public string Text { get; set; }
    public float Index { get; set; }
    public float? Logprobs { get; set; }
}