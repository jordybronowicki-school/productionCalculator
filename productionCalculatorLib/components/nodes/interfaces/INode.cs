﻿using productionCalculatorLib.components.calculator.limitors;
using productionCalculatorLib.components.connections;

namespace productionCalculatorLib.components.nodes.interfaces;

public interface INode
{
    long Id { get; }
    void RemoveConnnection(long connectionId);
    List<LimitProduction> ProductionLimits { get; }
    void AddProductionLimit(LimitProduction limit);
    void RemoveProductionLimit(LimitProduction limit);
}