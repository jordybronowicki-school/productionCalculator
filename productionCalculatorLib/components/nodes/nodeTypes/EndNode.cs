﻿using System.Collections.ObjectModel;
using productionCalculatorLib.components.calculator.limitors;
using productionCalculatorLib.components.connections;
using productionCalculatorLib.components.nodes.interfaces;
using productionCalculatorLib.components.products;

namespace productionCalculatorLib.components.nodes.nodeTypes;

public class EndNode: INodeIn, IHasProduct
{
    public long Id { get; } = IdGenerators.NodeId;
    private readonly List<Connection> _inputConnections = new();
    public Product Product { get; set; } = null!;
    public float Amount { get; set; }
    
    public EndNode() {}

    public EndNode(Product product)
    {
        Product = product;
    }

    public IList<Connection> InputConnections => new ReadOnlyCollection<Connection>(_inputConnections);
    public void AddInputConnection(Connection connection)
    {
        if (!_inputConnections.Contains(connection)) _inputConnections.Add(connection);
    }
    public void RemoveConnnection(long connectionId)
    {
        var connection = _inputConnections.Find(c => c.Id == connectionId);
        if (connection == null) return;
        _inputConnections.Remove(connection);
    }

    public List<LimitProduction> ProductionLimits { get; } = new();
    public void AddProductionLimit(LimitProduction limit)
    {
        if (!ProductionLimits.Contains(limit)) ProductionLimits.Add(limit);
    }
    public void RemoveProductionLimit(LimitProduction limit)
    {
        ProductionLimits.Remove(limit);
    }
}