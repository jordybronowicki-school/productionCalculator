﻿using productionCalculatorLib.components.targets;

namespace productionCalculatorLib.components.nodes.interfaces;

public interface INode
{
    Guid Id { get; init; }
    float Amount { get; set; }
    NodePosition Position { get; set; }
    ICollection<TargetProduction> Targets { get; set; }
    void SetExactTarget(float amount);
    void SetMinMaxTarget(float? minAmount, float? maxAmount);
    void ClearTargets();
}