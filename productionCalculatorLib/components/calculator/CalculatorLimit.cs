﻿using productionCalculatorLib.components.calculator.limitors;
using productionCalculatorLib.components.connections;
using productionCalculatorLib.components.nodes.interfaces;
using productionCalculatorLib.components.nodes.nodeTypes;
using productionCalculatorLib.components.products;
using productionCalculatorLib.components.worksheet;

namespace productionCalculatorLib.components.calculator;

public class CalculatorLimit
{
    private Worksheet _worksheet;
    private int _amountOfTimesCalculated;
    
    private CalculatorLimit(Worksheet worksheet)
    {
        _worksheet = worksheet;
    }

    public static void ReCalculateAmounts(Worksheet worksheet)
    {
        var w = new CalculatorLimit(worksheet);
        w.CheckLimits();
        w.ResetAmounts();
        while (w._amountOfTimesCalculated < 20)
        {
            w.Calculate();
            if (w.CheckResult()) return;
            w._amountOfTimesCalculated++;
        }
    }

    private void CheckLimits()
    {
        if (!_worksheet.Nodes.Any(node => 
                node.ProductionLimits.Any(limit => 
                    limit.Type == LimitProductionTypes.ExactAmount)))
            throw new ArgumentException("Worksheet must have at least 1 'ExactAmount' limit");
    }
    
    private void ResetAmounts()
    {
        foreach (var node in _worksheet.Nodes)
        {
            if (node is IHasProduct productNode) productNode.Amount = 0;
            if (node is IHasRecipe recipeNode) recipeNode.ProductionAmount = 0;
            if (node is INodeIn inNode) foreach (var connection in inNode.InputConnections) connection.Amount = 0;
        }
    }
    
    private bool CheckResult()
    {
        foreach (var node in _worksheet.Nodes)
        {
            switch (node)
            {
                case SpawnNode spawnNode:
                    if (spawnNode.Amount - spawnNode.OutputConnections.Sum(connection => connection.Amount) > 0.1) return false;
                    break;
                case ProductionNode productionNode:
                    if ((from throughPut in productionNode.Recipe.InputThroughPuts 
                         let amountRequired = productionNode.InputConnections
                             .Where(c => c.Product.Equals(throughPut.Product))
                             .Sum(connection => connection.Amount) 
                         where productionNode.ProductionAmount * throughPut.Amount - amountRequired > 0.1 
                         select throughPut).Any()) return false;
                    
                    if ((from throughPut in productionNode.Recipe.OutputThroughPuts 
                         let amountProvided = productionNode.OutputConnections
                             .Where(c => c.Product.Equals(throughPut.Product))
                             .Sum(connection => connection.Amount) 
                         where productionNode.ProductionAmount * throughPut.Amount - amountProvided > 0.1 
                         select throughPut).Any()) return false;
                    break;
                case EndNode endNode:
                    if (endNode.Amount - endNode.InputConnections.Sum(connection => connection.Amount) > 0.1) return false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(node));
            }
        }
        return true;
    }

    private void Calculate()
    {
        foreach (var node in _worksheet.Nodes)
        {
            switch (node)
            {
                case IHasProduct productNode:
                    CalculateProductAmounts(productNode);
                    break;
                case IHasRecipe recipeNode:
                    CalculateRecipeAmounts(recipeNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(node));
            }
        }
    }

    private void CalculateProductAmounts(IHasProduct productNode)
    {
        foreach (var limit in productNode.ProductionLimits)
        {
            switch (limit.Type)
            {
                case LimitProductionTypes.ExactAmount:
                    productNode.Amount = limit.Amount;
                    break;
            }
        }
        
        switch (productNode)
        {
            case SpawnNode spawnNode:
                DistributeProductsOut(spawnNode.OutputConnections, spawnNode.Product, spawnNode.Amount);
                break;
            case EndNode endNode:
                DistributeProductsIn(endNode.InputConnections,  endNode.Product, endNode.Amount);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(productNode));
        }
    }

    private void DistributeProductsIn(IEnumerable<Connection> connections, Product product, float amount)
    {
        var connectionsFiltered = connections.Where(connection => connection.Product.Equals(product)).ToList();
        foreach (var connection in connectionsFiltered)
        {
            var newAmount = amount / connectionsFiltered.Count;
            if (newAmount > connection.Amount) connection.Amount = newAmount;
            
            switch (connection.NodeIn)
            {
                case IHasProduct productNode:
                    productNode.Amount = connection.Amount;
                    break;
                case IHasRecipe recipeNode:
                    var newRecipeAmount = connection.Amount / recipeNode.Recipe.OutputThroughPuts.Find(put => put.Product.Equals(product))!.Amount;
                    if (newRecipeAmount > recipeNode.ProductionAmount) recipeNode.ProductionAmount = newRecipeAmount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(connection.NodeIn));
            }
        }
    }
    
    private void DistributeProductsOut(IEnumerable<Connection> connections, Product product, float amount)
    {
        var connectionsFiltered = connections.Where(connection => connection.Product.Equals(product)).ToList();
        foreach (var connection in connectionsFiltered)
        {
            var newAmount = amount / connectionsFiltered.Count;
            if (newAmount > connection.Amount) connection.Amount = newAmount;
            
            switch (connection.NodeOut)
            {
                case IHasProduct productNode:
                    productNode.Amount = connection.Amount;
                    break;
                case IHasRecipe recipeNode:
                    var newRecipeAmount = connection.Amount / recipeNode.Recipe.InputThroughPuts.Find(put => put.Product.Equals(product))!.Amount;
                    if (newRecipeAmount > recipeNode.ProductionAmount) recipeNode.ProductionAmount = newRecipeAmount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(connection.NodeOut));
            }
        }
    }
    
    private void CalculateRecipeAmounts(IHasRecipe recipeNode)
    {
        foreach (var limit in recipeNode.ProductionLimits)
        {
            switch (limit.Type)
            {
                case LimitProductionTypes.ExactAmount:
                    recipeNode.ProductionAmount = limit.Amount;
                    break;
            }
        }
        
        switch (recipeNode)
        {
            case ProductionNode productionNode:
                foreach (var inputThroughPut in productionNode.Recipe.InputThroughPuts)
                {
                    DistributeProductsIn(productionNode.InputConnections, inputThroughPut.Product, inputThroughPut.Amount*productionNode.ProductionAmount);
                }
                foreach (var outputThroughPut in productionNode.Recipe.OutputThroughPuts)
                {
                    DistributeProductsOut(productionNode.OutputConnections, outputThroughPut.Product, outputThroughPut.Amount*productionNode.ProductionAmount);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(recipeNode));
        }
    }
}