public class MarkovLinkEnemy
{
    public MarkovLinkEnemy(string enemy, int weight = 1)
    {
        _enemy = enemy;
        _weight = weight;
    }

    private string _enemy;
    private int _weight;

    public string Enemy => _enemy;

    public int Weight => _weight;
}