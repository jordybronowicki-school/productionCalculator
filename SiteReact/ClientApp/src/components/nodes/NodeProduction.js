import {Node} from "./Node";
import './NodeProduction.css';
import {nodeEditRecipe} from "./NodeAPI";
import Store from "../../dataStore/DataStore";
import {TargetManager} from "../targets/TargetManager";

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
    
    let recipeField, amountField, productInList, productOutList, targets, targetEditor;
    
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
      targets = <div></div>;
      targetEditor = <div></div>
    } else {
      recipeField = <div>
        <select value={super.recipe()} onChange={e => this.RecipeChanged(e.target.value)}>
          <option value="" disabled hidden></option>
          {super.recipes().map(v => <option key={v.name} value={v.name}>{v.name}</option>)}
        </select></div>;
      amountField = <div>{super.amount()}</div>;
      productInList = generateProductList(super.requiredInProducts());
      productOutList = generateProductList(super.requiredOutProducts());
      targets =
        <div className="targets" onClick={e => this.setState({targetEditorOpen: true})}>
          <div>a</div>
          <div>b</div>
          <div>c</div>
          <i className='bx bx-target-lock bx-rotate-90'></i>
        </div>;
      targetEditor = 
        <div className="targetEditor" hidden={!this.state.targetEditorOpen}>
          <button type="button" className="popup-close-button" onClick={() => this.setState({targetEditorOpen: false})}>
            <i className='bx bx-x'></i>
          </button>
          <TargetManager nodeId={this.state.data.id} targets={this.state.data.targets}></TargetManager>
        </div>
    }
    
    return (
      <div className="node-container">
        <div className="node-top">
          <h3>Production</h3>
          {targets}
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
        {targetEditor}
      </div>
    );
  }

  RecipeChanged(name) {
    nodeEditRecipe(Store.getState().worksheet.id, this.state.data.id, name);
  }
}