import {Node} from "./Node";
import './NodeProduction.css';

export class NodeProduction extends Node {
  constructor(props) {
    super(props);
  }
  
  render () {
    let generateProductList = (list) => {
      return (
        <div className="node-list">
          {list.map((value, index) => 
            <div key={index}>
              <div className="recipeProduct">{value.product}: </div>
              <div>{value.amount}</div>
            </div>)}
        </div>
      );
    };
    
    let recipeField = <div>
      <select value={super.recipe()} onChange={e => this.RecipeChanged(e.target.value)}>
        <option value="" disabled hidden></option>
        {super.recipes().map(v => <option key={v.name} value={v.name}>{v.name}</option>)}
      </select></div>;
    let amountField = <div>{super.amount()}</div>;
    let productInList = generateProductList(super.requiredInProducts());
    let productOutList = generateProductList(super.requiredOutProducts());
    
    if (super.previewMode()) {
      recipeField = <div className="previewField">name</div>;
      amountField = <div className="previewField">0</div>;
      productInList = <div className="node-list"><div>
          <div className="previewField">name</div>
          <div className="previewField">0</div>
        </div></div>;
      productOutList = <div className="node-list"><div>
          <div className="previewField">name</div>
          <div className="previewField">0</div>
        </div></div>;
    }
    
    return (
      <div className="node-container">
        <div className="node-top">
          <h3>Production</h3>
          <div className="targets">
            <div>a</div>
            <div>b</div>
            <div>c</div>
          </div>
        </div>
        <div className="node-content node-product-table">
          <div>Recipe: </div>
          {recipeField}
          <div>Amount: </div>
          {amountField}
          
          <div className="node-table">
            <div>Required in</div>
            <div>Required out</div>
            {productInList}
            {productOutList}
          </div>
        </div>
      </div>
    );
  }

  RecipeChanged(name) {
    console.log(name)
    // TODO load actual value
  }
}