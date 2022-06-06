﻿using productionCalculatorLib.components.products;

namespace SiteReact.Controllers.dto.nodes;

public class NodeDto
{
    public int Id { get; set; }
    public string Type { get; set; }
    
    public float Amount { get; set; }
    public Recipe? Recipe { get; set; }
    public Product? Product { get; set; }
    
    public IEnumerable<DtoConnection>? InputNodes { get; set; }
    public IEnumerable<DtoConnection>? OutputNodes { get; set; }
}