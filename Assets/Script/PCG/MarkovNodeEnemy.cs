
using System.Collections.Generic;

public class MarkovNodeEnemy
{
    private string _enemy;

    private List<MarkovNodeEnemy> _nextNodes;

    public MarkovNodeEnemy(string _nodeEnemy)
    {
        _enemy = _nodeEnemy;
        _nextNodes = new List<MarkovNodeEnemy>();
    }
    
    public void AddNode(string _nodeEnemy)
    {
        if (_nextNodes != null)
        {
            _nextNodes.Add(new MarkovNodeEnemy(_nodeEnemy));
        }
    }

}