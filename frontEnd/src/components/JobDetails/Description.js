import React, { Component } from "react";
import parse from 'html-react-parser';

export class Description extends Component {

  render() {
    return (
      <div

        style={{ color: "rgb(56,56,56)", backgroundColor: "rgb(227,230,239)", fontSize: ".9rem !important", fontWeight: "600" }}
        className="ad-description col-10 mx-auto ir-b fs-m  p-3 pb-0 my-3 text-center srounded-md"
      >
        {parse(this.props.description.toString())}
        <hr className="desc-separator my-2 d-block mx-auto" />
        {this.props.isImmediate === "فوری" && <span className="m-0 p-2 px-3 rounded-pill badge badge-danger ir-b">{this.props.isImmediate}</span>}
        {/* adver code */}
        <sapn className="m-0 px-3 py-0 my-2 d-flex flex-row justify-content-center align-items-baseline ">
          <span className="m-0  ml-1 p-0 ir-r" >کد آگهی :</span>
          {this.props.adverId && <strong className="m-0 p-0 ir-r" >{this.props.adverId}</strong>}
        </sapn>
      </div>
    );
  }
}
