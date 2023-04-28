namespace Project;

public class UnitTest1 : IClassFixture<ServicesFixture>
{
    private readonly ServicesFixture _fixture;

    public UnitTest1(ServicesFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void Test1()
    {
    }
}