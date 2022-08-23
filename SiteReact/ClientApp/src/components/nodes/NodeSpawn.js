import {Node} from "./Node";
import './NodeSpawn.css';
import {nodeEditProduct} from "./NodeAPI";
import Store from "../../dataStore/DataStore";

export class NodeSpawn extends Node {
  constructor(props) {
    super(props);
  }

  render () {
    let productField = <div className="previewField">name</div>;
    let targets = <div></div>;
    
    if (!super.previewMode()) {
      productField =
        <select value={super.product()} onChange={e => this.productChanged(e.target.value)}>
          {super.products().map(v => (
            <option key={v.name} value={v.name}>{v.name}</option>))}
        </select>;
      targets = 
        <div className="targets" onClick={e => this.setState({targetEditorOpen: true})}>
          <div>a</div>
          <div>b</div>
          <div>c</div>
          <i className='bx bx-target-lock bx-rotate-90'></i>
        </div>;
    }
    
    return (
      <div className="node-container">
        <div className="node-top">
          <h3>Spawn</h3>
          {targets}
        </div>
        <div className="node-content node-table">
          <div>Product</div>
          {productField}
          <div>Amount</div>
          <div className={super.previewMode()?"previewField":""}>{super.amount()}</div>
        </div>
        <div className="targetEditor" hidden={!this.state.targetEditorOpen}>
          test
          <button onClick={e => this.setState({targetEditorOpen: false})}>x</button>
        </div>
      </div>
    );
  }
  
  productChanged(name) {
    nodeEditProduct(Store.getState().worksheet.id, this.state.data.id, name);
  }
}