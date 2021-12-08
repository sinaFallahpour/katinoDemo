import React, { Component } from "react";
import { Refrences } from "../../components/companies";
// import * as service from "../../components/companies";
import { Link } from "react-router-dom";
import agent from "../../core/agent"
export class BestRefrences extends Component {
  state = {
    refrences: [],
  };

  async componentDidMount() {

    document.getElementById("root").scrollIntoView();

    
    var { data } =await agent.Refrences.refrences()
    // console.log(data);
    // console.log(data.res.resul);
    // console.log(data.res.resul);
    
   this.setState({ refrences:  data   })



    // await service
    //   .bestCompanies()
    //   .then((res) => this.setState({ companies: res.data.resul }));
  }

  render() {
    return (
      <section className="companies container-fluid spx-2 spx-lg-10 smt-10 spt-3 mb-0">
        <header className="companies-header srounded-md spb-5 d-flex flex-column justify-content-center align-items-center">
          <h1 className="ir-b text-white title mb-0">نمایندگان</h1>
          {/* <Link to="/AllCompanies" className="ir-r text-white smt-1">
            سایر  نمایندگان ها
          </Link> */}
        </header>
        <Refrences refrences={this.state.refrences} />
      </section>
    );
  }
}
