import React, { Component } from "react";
import { Refrences } from "../../components/companies";
import * as service from "../../components/companies";
import { Link } from "react-router-dom";
import agent from "../../core/agent"


export class BestRefrence extends Component {
  state = {
    refrences: [],
  };

  async componentDidMount() {
    const { data } = agent.Refrences.refrences()
    this.setState({ refrences: data.resul })

    // await service
    //   .bestCompanies()
    //   .then((res) => this.setState({ companies: res.data.resul }));

  }

  render() {
    return (
      <section className="companies container-fluid spx-2 spx-lg-10 smt-10 spt-3 mb-0">
        <header className="companies-header srounded-md spb-5 d-flex flex-column justify-content-center align-items-center">
          <h1 className="ir-b text-white title mb-0">نمایندگان</h1>
          <Link to="/AllCompanies" className="ir-r text-white smt-1">
            سایر نمایندگان
          </Link>
        </header>
        <Refrences refrences={this.state.refrences} />
      </section>
    );
  }
}
