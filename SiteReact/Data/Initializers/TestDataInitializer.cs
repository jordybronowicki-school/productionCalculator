﻿using productionCalculatorLib.components.nodes.nodeTypes;
using productionCalculatorLib.components.products;
using productionCalculatorLib.components.worksheet;
using SiteReact.Data.DbContexts;

namespace SiteReact.Data.Initializers;

public static class TestDataInitializer
{
    public static void InitializeAllData(ProjectContext context)
    {
        InitializeSimpleOneWay(context);
        InitializeDoubleSpawn(context);
    }
    
    public static void InitializeSimpleOneWay(ProjectContext context)
    {
        var worksheet = new Worksheet
        {
            Name = "Iron ingot smelting"
        };
        context.Worksheets.Add(worksheet);

        var productIronOre = worksheet.EntityContainer.GetOrGenerateProduct("Iron ore");
        var productIronIngot = worksheet.EntityContainer.GetOrGenerateProduct("Iron ingot");

        var recipeIronIngot = worksheet.EntityContainer.GenerateRecipe("Iron ingot");
        recipeIronIngot.InputThroughPuts.Add(new ThroughPut(productIronOre, 30));
        recipeIronIngot.OutputThroughPuts.Add(new ThroughPut(productIronIngot, 10));

        var node1 = worksheet.GetNodeBuilder<SpawnNode>().SetProduct(productIronOre).Build();
        var node2 = worksheet.GetNodeBuilder<ProductionNode>().SetRecipe(recipeIronIngot).Build();
        var node3 = worksheet.GetNodeBuilder<EndNode>().SetProduct(productIronIngot).SetExactTarget(20).Build();

        context.SaveChanges();

        worksheet.GetConnectionBuilder(node1, node2, productIronOre).Build();
        worksheet.GetConnectionBuilder(node2, node3, productIronIngot).Build();
        
        context.SaveChanges();
    }

    public static void InitializeDoubleSpawn(ProjectContext context)
    {
        var worksheet = new Worksheet
        {
            Name = "Steel ingot smelting"
        };
        context.Worksheets.Add(worksheet);

        var productIronOre = worksheet.EntityContainer.GetOrGenerateProduct("Iron ore");
        var productCoal = worksheet.EntityContainer.GetOrGenerateProduct("Coal");
        var productSteelIngot = worksheet.EntityContainer.GetOrGenerateProduct("Steel ingot");
        
        var recipeIronIngot = worksheet.EntityContainer.GenerateRecipe("Steel ingot");
        recipeIronIngot.InputThroughPuts.Add(new ThroughPut(productIronOre, 30));
        recipeIronIngot.InputThroughPuts.Add(new ThroughPut(productCoal, 5));
        recipeIronIngot.OutputThroughPuts.Add(new ThroughPut(productSteelIngot, 10));
        
        var node1 = worksheet.GetNodeBuilder<SpawnNode>().SetProduct(productIronOre).Build();
        var node2 = worksheet.GetNodeBuilder<SpawnNode>().SetProduct(productCoal).Build();
        var node3 = worksheet.GetNodeBuilder<ProductionNode>().SetRecipe(recipeIronIngot).SetExactTarget(2).Build();
        var node4 = worksheet.GetNodeBuilder<EndNode>().SetProduct(productSteelIngot).Build();
        
        context.SaveChanges();
        
        worksheet.GetConnectionBuilder(node1, node3, productIronOre).Build();
        worksheet.GetConnectionBuilder(node2, node3, productCoal).Build();
        worksheet.GetConnectionBuilder(node3, node4, productSteelIngot).Build();
        
        context.SaveChanges();
    }
}